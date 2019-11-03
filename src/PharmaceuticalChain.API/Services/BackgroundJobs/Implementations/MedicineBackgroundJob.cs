using Microsoft.Extensions.Options;
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

namespace PharmaceuticalChain.API.Services.BackgroundJobs.Implementations
{
    public class MedicineBackgroundJob : IMedicineBackgroundJob
    {
        private readonly IEthereumService ethereumService;
        private readonly IMedicineRepository medicineBatchRepository;
        private readonly string MedicineAbi;
        public MedicineBackgroundJob(
            IEthereumService ethereumService,
            IMedicineRepository medicineBatchRepository,
            IOptions<EthereumSettings> options
            )
        {
            this.ethereumService = ethereumService;
            this.medicineBatchRepository = medicineBatchRepository;
            MedicineAbi = options.Value.MedicineAbi;
        }

        void IMedicineBackgroundJob.WaitForTransactionToSuccessThenFinishCreatingMedicine(Medicine medicine)
        {
            bool isTransactionSuccess = false;
            do
            {
                var receipt = ethereumService.GetTransactionReceipt(medicine.TransactionHash).Result;
                if (receipt == null)
                    continue;
                if (receipt.Status.Value == (new HexBigInteger(1)).Value)
                {
                    isTransactionSuccess = true;
                    var contractAddress = ethereumService.GetObjectContractAddress(medicine.Id).Result;
                    medicine.ContractAddress = contractAddress;
                    medicine.TransactionStatus = Models.Database.TransactionStatuses.Success;
                    medicineBatchRepository.Update(medicine);

                    var medicineContract = ethereumService.GetContract(MedicineAbi, medicine.ContractAddress);
                    var updateFunction = ethereumService.GetFunction(medicineContract, EthereumFunctions.UpdateMedicineInformation);
                    var updateReceipt = updateFunction.SendTransactionAndWaitForReceiptAsync(
                        ethereumService.GetEthereumAccount(),
                        new HexBigInteger(6000000),
                        new HexBigInteger(Nethereum.Web3.Web3.Convert.ToWei(30, UnitConversion.EthUnit.Gwei)),
                        new HexBigInteger(0),
                        functionInput: new object[] {
                            medicine.CommercialName,
                            medicine.RegistrationCode,
                            medicine.IsPrescriptionMedicine,
                            medicine.IngredientConcentration,
                            medicine.PackingSpecification,
                            medicine.DosageForm,
                            medicine.DeclaredPrice,
                            medicine.SubmittedTenantId.ToString()
                        }).Result;
                }

            }
            while (isTransactionSuccess != true);

        }
    }
}
