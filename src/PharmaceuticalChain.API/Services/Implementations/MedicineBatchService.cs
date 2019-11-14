using Hangfire;
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
        public MedicineBatchService(
            IMedicineBatchRepository medicineBatchRepository,
             IEthereumService ethereumService
            )
        {
            this.medicineBatchRepository = medicineBatchRepository;
            this.ethereumService = ethereumService;
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

        List<MedicineBatchQueryData> IMedicineBatchService.GetAll()
        {
            var result = new List<MedicineBatchQueryData>();
            var rawList = medicineBatchRepository.GetAll();
            foreach(var item in rawList)
            {
                result.Add(item.ToMedicineBatchQueryData());
            }
            return result;
        }
    }
}
