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
    public class MedicineService : IMedicineService
    {
        private readonly IEthereumService ethereumService;
        private readonly IMedicineRepository medicineRepository;
        private readonly IMedicineBackgroundJob medicineBackgroundJob;

        public MedicineService(
            IEthereumService ethereumService,
            IMedicineRepository medicineBatchRepository,
            IMedicineBackgroundJob medicineBatchBackgroundJob)
        {
            this.ethereumService = ethereumService;
            this.medicineRepository = medicineBatchRepository;
            this.medicineBackgroundJob = medicineBatchBackgroundJob;
        }

        async Task<Guid> IMedicineService.Create(
            string commercialName,
            string registrationCode,
            bool isPrescriptionMedicine,
            string dosageForm,
            string ingredientConcentration,
            string packingSpecification,
            uint declaredPrice,
            Guid submittedTenantId)
        {
            try
            {
                var medicine = new Medicine()
                {
                    CommercialName = commercialName,
                    RegistrationCode = registrationCode,
                    IsPrescriptionMedicine = isPrescriptionMedicine,
                    DosageForm = dosageForm,
                    IngredientConcentration = ingredientConcentration,
                    PackingSpecification = packingSpecification,
                    DeclaredPrice = declaredPrice,
                    DateCreated = DateTime.UtcNow
                };
                Guid newMedicineBatchId = medicineRepository.CreateAndReturnId(medicine);

                var function = ethereumService.GetFunction(EthereumFunctions.AddMedicineBatch);
                var transactionHash = await function.SendTransactionAsync(
                    ethereumService.GetEthereumAccount(),
                    new HexBigInteger(6000000),
                    new HexBigInteger(Nethereum.Web3.Web3.Convert.ToWei(50, UnitConversion.EthUnit.Gwei)),
                    new HexBigInteger(0),
                    functionInput: new object[] {
                        newMedicineBatchId.ToString(),
                        commercialName,
                        registrationCode
                    });

                medicine.TransactionHash = transactionHash;
                medicineRepository.Update(medicine);

                BackgroundJob.Schedule<IMedicineBackgroundJob>(
                    medicineBackgroundJob => medicineBackgroundJob.WaitForTransactionToSuccessThenFinishCreatingMedicine(medicine),
                    TimeSpan.FromSeconds(3)
                    );

                return newMedicineBatchId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }
    }
}
