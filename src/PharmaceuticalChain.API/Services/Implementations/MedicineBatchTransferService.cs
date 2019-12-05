using Hangfire;
using Microsoft.Extensions.Options;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using PharmaceuticalChain.API.Controllers.Models.Queries;
using PharmaceuticalChain.API.Models.Database;
using PharmaceuticalChain.API.Repositories.Interfaces;
using PharmaceuticalChain.API.Services.BackgroundJobs.Interfaces;
using PharmaceuticalChain.API.Services.Interfaces;
using PharmaceuticalChain.API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Implementations
{
    public class MedicineBatchTransferService : IMedicineBatchTransferService
    {
        private readonly IMedicineBatchRepository medicineBatchRepository;
        private readonly IMedicineBatchTransferRepository medicineBatchTransferRepository;
        private readonly IEthereumService ethereumService;
        private readonly string MedicineBatchTransferAbi;
        public MedicineBatchTransferService(
            IMedicineBatchRepository medicineBatchRepository,
            IMedicineBatchTransferRepository medicineBatchTransferRepository,
            IEthereumService ethereumService,
            IOptions<EthereumSettings> options)
        {
            this.medicineBatchRepository = medicineBatchRepository;
            this.medicineBatchTransferRepository = medicineBatchTransferRepository;
            this.ethereumService = ethereumService;
            MedicineBatchTransferAbi = options.Value.MedicineBatchTransferAbi;
        }

        async Task<Guid> IMedicineBatchTransferService.Create(Guid medicineBatchId, Guid fromTenantId, Guid toTenantId, uint quantity)
        {
            var allTransfer = medicineBatchTransferRepository.GetAll();
            var batch = medicineBatchRepository.Get(medicineBatchId);
            var transfer = new MedicineBatchTransfer()
            {
                MedicineBatchId = batch.Id,
                FromId = fromTenantId,
                ToId = toTenantId,
                Quantity = quantity,
                DateCreated = DateTime.UtcNow
            };

            // TODO: Check inventory of fromTenant

            // Set the tier.
            uint tier = 999;
            if (fromTenantId == batch.ManufacturerId)
            {
                tier = 0;
            }
            else
            {
                var firstTransaction = allTransfer
                    .Where(t => t.MedicineBatchId == batch.Id && t.FromId == batch.ManufacturerId)
                    .SingleOrDefault();
                if (fromTenantId == firstTransaction.ToId)
                {
                    tier = 1;
                }
                else
                {
                    // Loop through all parent transactions of FromTenantId
                    var transferPool = allTransfer.Where(t => t.MedicineBatchId == batch.Id).ToList();
                    bool parentMatchCondition(MedicineBatchTransfer t) => t.ToId == transfer.FromId;
                    if (transfer.HasParent(transferPool, parentMatchCondition))
                    {
                        tier = transfer.Parent(transferPool, parentMatchCondition).First().Tier + 1;
                    }
                    // TODO: Some errors occured here.
                }
            }

            transfer.Tier = tier;
            var newTransferId = medicineBatchTransferRepository.CreateAndReturnId(transfer);

            var function = ethereumService.GetFunction(EthereumFunctions.AddMedicineBatchTransfer);
            var transactionHash = await function.SendTransactionAsync(
                ethereumService.GetEthereumAccount(),
                new HexBigInteger(6000000),
                new HexBigInteger(Nethereum.Web3.Web3.Convert.ToWei(10, UnitConversion.EthUnit.Gwei)),
                new HexBigInteger(0),
                functionInput: new object[] {
                    newTransferId.ToString(),
                    transfer.MedicineBatchId.ToString(),
                    transfer.FromId.ToString(),
                    transfer.ToId.ToString(),
                    transfer.Quantity,
                    transfer.DateCreated.ToUnixTimestamp(),
                    tier
                });

            transfer.TransactionHash = transactionHash;
            medicineBatchTransferRepository.Update(transfer);


            BackgroundJob.Schedule<IMedicineBatchTransferBackgroundJob>(
                job => job.WaitForTransactionToSuccessThenFinishCreating(transfer),
                TimeSpan.FromSeconds(3)
            );

            return newTransferId;
        }

        async Task IMedicineBatchTransferService.Delete(Guid id)
        {
            var transfer = medicineBatchTransferRepository.Get(id);
            if (transfer == null)
            {
                throw new ArgumentException("Id does not exist in the system.", nameof(id));
            }

            try
            {
                var function = ethereumService.GetFunction(EthereumFunctions.RemoveMedicineBatchTransfer);
                var transactionHash = await function.SendTransactionAsync(
                                  ethereumService.GetEthereumAccount(),
                                  new HexBigInteger(1000000),
                                  new HexBigInteger(0),
                                  functionInput: new object[] {
                                       id.ToString()
                });

                var tenantContract = ethereumService.GetContract(MedicineBatchTransferAbi, transfer.ContractAddress);
                var deleteFunction = ethereumService.GetFunction(tenantContract, EthereumFunctions.SelfDelete);
                var receipt = await deleteFunction.SendTransactionAsync(
                    ethereumService.GetEthereumAccount(),
                    new HexBigInteger(6000000),
                    new HexBigInteger(Nethereum.Web3.Web3.Convert.ToWei(5, UnitConversion.EthUnit.Gwei)),
                    new HexBigInteger(0),
                    functionInput: new object[] { }
                );

                medicineBatchTransferRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        MedicineBatchTransferQueryData IMedicineBatchTransferService.Get(Guid id)
        {
            var result = medicineBatchTransferRepository.Get(id).ToMedicineBatchTransferQueryData();
            return result;
        }

        List<MedicineBatchTransferQueryData> IMedicineBatchTransferService.GetAll()
        {
            var result = new List<MedicineBatchTransferQueryData>();
            var rawTransfers = medicineBatchTransferRepository.GetAll();
            foreach(var transfer in rawTransfers)
            {
                result.Add(transfer.ToMedicineBatchTransferQueryData());
            }
            return result;
        }

        async Task IMedicineBatchTransferService.Update(
            Guid id,
            Guid medicineBatchId, 
            Guid fromTenantId,
            Guid toTenantId, 
            uint quantity)
        {
            var transfer = medicineBatchTransferRepository.Get(id);
            if (transfer == null)
            {
                throw new ArgumentException("Id does not exist in the system.", nameof(id));
            }

            transfer.MedicineBatchId = medicineBatchId;
            transfer.FromId = fromTenantId;
            transfer.ToId = toTenantId;
            transfer.Quantity = quantity;

            // Update the blockchain.
            var contract = ethereumService.GetContract(MedicineBatchTransferAbi, transfer.ContractAddress);
            var updateFunction = ethereumService.GetFunction(contract, EthereumFunctions.UpdateMedicineBatchTransfer);
            var updateReceipt = await updateFunction.SendTransactionAsync(
                ethereumService.GetEthereumAccount(),
                new HexBigInteger(6000000),
                new HexBigInteger(Nethereum.Web3.Web3.Convert.ToWei(20, UnitConversion.EthUnit.Gwei)),
                new HexBigInteger(0),
                functionInput: new object[] {
                    transfer.MedicineBatchId.ToString(),
                    transfer.FromId.ToString(),
                    transfer.ToId.ToString(),
                    transfer.Quantity,
                    transfer.DateCreated.ToUnixTimestamp()
                });

            medicineBatchTransferRepository.Update(transfer);
        }
    }
}
