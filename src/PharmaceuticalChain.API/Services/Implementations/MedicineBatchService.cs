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
    public class MedicineBatchService : IMedicineBatchService
    {
        private readonly IMedicineBatchRepository medicineBatchRepository;
        private readonly IEthereumService ethereumService;
        private readonly string MedicineBatchAbi;
        public MedicineBatchService(
            IMedicineBatchRepository medicineBatchRepository,
            IEthereumService ethereumService,
            IOptions<EthereumSettings> options
            )
        {
            this.medicineBatchRepository = medicineBatchRepository;
            this.ethereumService = ethereumService;
            MedicineBatchAbi = options.Value.MedicineBatchAbi;
        }

        async Task<Guid> IMedicineBatchService.Create(
            string batchNumber,
            Guid medicineId,
            Guid manufacturerId,
            DateTime manufactureDate,
            DateTime expiryDate,
            uint quantity,
            string unit)
        {
            var medicineBatch = new MedicineBatch()
            {
                BatchNumber = batchNumber,
                MedicineId = medicineId,
                ManufacturerId = manufacturerId,
                ManufactureDate = manufactureDate,
                ExpiryDate = expiryDate,
                Quantity = quantity,
                Unit = unit
            };
            Guid newMedicineBatchId = medicineBatchRepository.CreateAndReturnId(medicineBatch);

            var function = ethereumService.GetFunction(EthereumFunctions.AddMedicineBatch);
            var transactionHash = await function.SendTransactionAsync(
                ethereumService.GetEthereumAccount(),
                new HexBigInteger(6000000),
                new HexBigInteger(Nethereum.Web3.Web3.Convert.ToWei(10, UnitConversion.EthUnit.Gwei)),
                new HexBigInteger(0),
                functionInput: new object[] {
                        newMedicineBatchId.ToString(),
                        medicineBatch.MedicineId.ToString(),
                        medicineBatch.BatchNumber,
                        medicineBatch.ManufacturerId.ToString()
                });

            medicineBatch.TransactionHash = transactionHash;
            medicineBatchRepository.Update(medicineBatch);

            BackgroundJob.Schedule<IMedicineBatchBackgroundJob>(
                medicineBatchBackgroundJob => medicineBatchBackgroundJob.WaitForTransactionToSuccessThenFinishCreatingMedicineBatch(medicineBatch),
                TimeSpan.FromSeconds(3)
                );

            return newMedicineBatchId;
        }

        async Task IMedicineBatchService.Delete(Guid id)
        {
            var batch = medicineBatchRepository.Get(id);
            if (batch == null)
            {
                throw new ArgumentException("Id does not exist in the system.", nameof(id));
            }

            var function = ethereumService.GetFunction(EthereumFunctions.RemoveMedicineBatch);
            var transactionHash = await function.SendTransactionAsync(
                ethereumService.GetEthereumAccount(),
                new HexBigInteger(1000000),
                new HexBigInteger(0),
                functionInput: new object[] {
                    id.ToString()
            });

            var batchContract = ethereumService.GetContract(MedicineBatchAbi, batch.ContractAddress);
            var deleteFunction = ethereumService.GetFunction(batchContract, EthereumFunctions.SelfDelete);
            var updateReceipt = await deleteFunction.SendTransactionAsync(
                ethereumService.GetEthereumAccount(),
                new HexBigInteger(6000000),
                new HexBigInteger(Nethereum.Web3.Web3.Convert.ToWei(5, UnitConversion.EthUnit.Gwei)),
                new HexBigInteger(0),
                functionInput: new object[] { });

            medicineBatchRepository.Delete(id);
        }

        List<MedicineBatchQueryData> IMedicineBatchService.GetAll()
        {
            var result = new List<MedicineBatchQueryData>();
            var rawList = medicineBatchRepository.GetAll();
            foreach (var item in rawList)
            {
                result.Add(item.ToMedicineBatchQueryData());
            }
            return result;
        }

        async Task IMedicineBatchService.Update(
            Guid id,
            string batchNumber,
            Guid medicineId,
            DateTime manufactureDate,
            DateTime expiryDate,
            uint quantity,
            string unit)
        {
            var batch = medicineBatchRepository.Get(id);
            if (batch == null)
            {
                throw new ArgumentException("Id does not exist in the system.", nameof(id));
            }

            batch.BatchNumber = batchNumber;
            batch.MedicineId = medicineId;
            batch.ManufactureDate = manufactureDate;
            batch.ExpiryDate = expiryDate;
            batch.Quantity = quantity;
            batch.Unit = unit;

            medicineBatchRepository.Update(batch);

            // Update the blockchain.
            var batchContract = ethereumService.GetContract(MedicineBatchAbi, batch.ContractAddress);
            var updateFunction = ethereumService.GetFunction(batchContract, EthereumFunctions.UpdateMedicineBatchInformation);
            var updateReceipt = await updateFunction.SendTransactionAsync(
                ethereumService.GetEthereumAccount(),
                new HexBigInteger(6000000),
                new HexBigInteger(Nethereum.Web3.Web3.Convert.ToWei(10, UnitConversion.EthUnit.Gwei)),
                new HexBigInteger(0),
                functionInput: new object[] {
                    batch.MedicineId.ToString(),
                    batch.BatchNumber,
                    batch.ManufacturerId.ToString(),
                    batch.Quantity,
                    batch.Unit,
                    batch.ManufactureDate.ToUnixTimestamp(),
                    batch.ExpiryDate.ToUnixTimestamp()
                });
        }
    }
}
