using Microsoft.Extensions.Options;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using PharmaceuticalChain.API.Repositories.Interfaces;
using PharmaceuticalChain.API.Services.BackgroundJobs.Interfaces;
using PharmaceuticalChain.API.Services.Interfaces;
using PharmaceuticalChain.API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.BackgroundJobs.Implementations
{
    public class MedicineBatchBackgroundJob : IMedicineBatchBackgroundJob
    {
        private readonly IEthereumService ethereumService;
        private readonly IMedicineBatchRepository medicineBatchRepository;
        private readonly string MedicineBatchAbi;
        public MedicineBatchBackgroundJob(
            IEthereumService ethereumService,
            IMedicineBatchRepository medicineBatchRepository,
            IOptions<EthereumSettings> options
            )
        {
            this.ethereumService = ethereumService;
            this.medicineBatchRepository = medicineBatchRepository;
            MedicineBatchAbi = options.Value.MedicineBatchAbi;
        }

        void IMedicineBatchBackgroundJob.WaitForTransactionToSuccessThenFinishCreatingMedicineBatch(Guid medicineBatchId)
        {
            var medicineBatch = medicineBatchRepository.Get(medicineBatchId);
            bool isTransactionSuccess = false;
            do
            {
                var receipt = ethereumService.GetTransactionReceipt(medicineBatch.TransactionHash).Result;
                if (receipt == null)
                    continue;
                if (receipt.Status.Value == (new HexBigInteger(1)).Value)
                {
                    isTransactionSuccess = true;
                    var contractAddress = ethereumService.GetObjectContractAddress(medicineBatch.Id).Result;
                    medicineBatch.ContractAddress = contractAddress;
                    medicineBatch.TransactionStatus = Models.Database.TransactionStatuses.Success;
                    medicineBatchRepository.Update(medicineBatch);

                    var medicineBatchContract = ethereumService.GetContract(MedicineBatchAbi, medicineBatch.ContractAddress);
                    var updateFunction = ethereumService.GetFunction(medicineBatchContract, EthereumFunctions.UpdateMedicineBatchInformation);
                    var updateReceipt = updateFunction.SendTransactionAndWaitForReceiptAsync(
                        ethereumService.GetEthereumAccount(),
                        new HexBigInteger(6000000),
                        new HexBigInteger(Nethereum.Web3.Web3.Convert.ToWei(50, UnitConversion.EthUnit.Gwei)),
                        new HexBigInteger(0),
                        functionInput: new object[] {
                            medicineBatch.CommercialName,
                            medicineBatch.RegistrationCode,
                            medicineBatch.BatchNumber,
                            medicineBatch.IsPrescriptionMedicine,
                            medicineBatch.IngredientConcentration,
                            medicineBatch.PackingSpecification,
                            medicineBatch.Quantity,
                            medicineBatch.ManufactureDate.ToUnixTimestamp(),
                            medicineBatch.ExpiryDate.ToUnixTimestamp(),
                            medicineBatch.DosageForm,
                            medicineBatch.DeclaredPrice
                        }).Result;
                }

            }
            while (isTransactionSuccess != true);

        }
    }
}
