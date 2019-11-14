using Hangfire;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
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
        public MedicineBatchTransferService(
            IMedicineBatchRepository medicineBatchRepository,
            IMedicineBatchTransferRepository medicineBatchTransferRepository,
            IEthereumService ethereumService)
        {
            this.medicineBatchRepository = medicineBatchRepository;
            this.medicineBatchTransferRepository = medicineBatchTransferRepository;
            this.ethereumService = ethereumService;
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
    }
}
