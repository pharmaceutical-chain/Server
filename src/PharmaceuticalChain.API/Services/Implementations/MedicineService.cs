﻿using Hangfire;
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
    public class MedicineService : IMedicineService
    {
        private readonly IEthereumService ethereumService;
        private readonly IMedicineRepository medicineRepository;
        private readonly IMedicineBackgroundJob medicineBackgroundJob;
        private readonly string MedicineAbi;

        public MedicineService(
            IEthereumService ethereumService,
            IMedicineRepository medicineRepository,
            IMedicineBackgroundJob medicineBackgroundJob,
            IOptions<EthereumSettings> options)
        {
            this.ethereumService = ethereumService;
            this.medicineRepository = medicineRepository;
            this.medicineBackgroundJob = medicineBackgroundJob;
            MedicineAbi = options.Value.MedicineAbi;
        }

        async Task<Guid> IMedicineService.Create(
            string commercialName,
            string registrationCode,
            bool isPrescriptionMedicine,
            string dosageForm,
            string ingredientConcentration,
            string packingSpecification,
            uint declaredPrice,
            Guid submittedTenantId,
            string certificates)
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
                    DateCreated = DateTime.UtcNow,
                    SubmittedTenantId = submittedTenantId,
                    Certificates = certificates,
                    IsApprovedByAdmin = false
                };
                Guid newMedicineId = medicineRepository.CreateAndReturnId(medicine);

                var function = ethereumService.GetFunction(EthereumFunctions.AddMedicine);
                var transactionHash = await function.SendTransactionAsync(
                    ethereumService.GetEthereumAccount(),
                    new HexBigInteger(6000000),
                    new HexBigInteger(Nethereum.Web3.Web3.Convert.ToWei(10, UnitConversion.EthUnit.Gwei)),
                    new HexBigInteger(0),
                    functionInput: new object[] {
                        newMedicineId.ToString(),
                        commercialName,
                        registrationCode
                    });

                medicine.TransactionHash = transactionHash;
                medicineRepository.Update(medicine);

                BackgroundJob.Schedule<IMedicineBackgroundJob>(
                    medicineBackgroundJob => medicineBackgroundJob.WaitForTransactionToSuccessThenFinishCreatingMedicine(medicine),
                    TimeSpan.FromSeconds(3)
                    );

                return newMedicineId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
           
        }

        async Task IMedicineService.Delete(Guid id)
        {
            var medicine = medicineRepository.Get(id);
            if (medicine == null)
            {
                throw new ArgumentException("Id does not exist in the system.", nameof(id));
            }

            try
            {
                var function = ethereumService.GetFunction(EthereumFunctions.RemoveMedicine);
                var transactionHash = await function.SendTransactionAsync(
                                  ethereumService.GetEthereumAccount(),
                                  new HexBigInteger(1000000),
                                  new HexBigInteger(0),
                                  functionInput: new object[] {
                                       id.ToString()
                });

                var contract = ethereumService.GetContract(MedicineAbi, medicine.ContractAddress);
                var deleteFunction = ethereumService.GetFunction(contract, EthereumFunctions.SelfDelete);
                var receipt = await deleteFunction.SendTransactionAsync(
                    ethereumService.GetEthereumAccount(),
                    new HexBigInteger(6000000),
                    new HexBigInteger(Nethereum.Web3.Web3.Convert.ToWei(5, UnitConversion.EthUnit.Gwei)),
                    new HexBigInteger(0),
                    functionInput: new object[] { }
                );

                medicineRepository.Delete(id);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        MedicineQueryData IMedicineService.GetMedicine(Guid id)
        {
            var result = medicineRepository.Get(id).ToMedicineQueryData();
            return result;
        }

        List<MedicineQueryData> IMedicineService.GetMedicines()
        {
            var result = new List<MedicineQueryData>();
            var rawList = medicineRepository.GetAll();
            foreach(var medicine in rawList)
            {
                result.Add(medicine.ToMedicineQueryData());
            }
            return result;
        }

        async Task IMedicineService.Update(
            Guid medicineId,
            string commercialName, 
            string registrationCode, 
            bool isPrescriptionMedicine, 
            string ingredientConcentration, 
            string packingSpecification, 
            string dosageForm, 
            uint declaredPrice, 
            Guid submittedTenantId,
            string certificates,
            bool isApprovedByAdmin
            )
        {
            var medicine = medicineRepository.Get(medicineId);
            if (medicine == null)
            {
                throw new ArgumentException("MedicineId does not exist in the system.", nameof(medicineId));
            }
            if(medicine.SubmittedTenantId != submittedTenantId)
            {
                throw new ArgumentException("Only the originally submitted tenant can update the medicine.", nameof(submittedTenantId));
            }

            // Update the database
            medicine.CommercialName = commercialName;
            medicine.RegistrationCode = registrationCode;
            medicine.IsPrescriptionMedicine = isPrescriptionMedicine;
            medicine.IngredientConcentration = ingredientConcentration;
            medicine.PackingSpecification = packingSpecification;
            medicine.DosageForm = dosageForm;
            medicine.DeclaredPrice = declaredPrice;
            medicine.SubmittedTenantId = submittedTenantId;
            medicine.Certificates = certificates;
            medicine.IsApprovedByAdmin = isApprovedByAdmin;

            medicineRepository.Update(medicine);

            // Update the blockchain
            var contract = ethereumService.GetContract(MedicineAbi, medicine.ContractAddress);
            var updateFunction = ethereumService.GetFunction(contract, EthereumFunctions.UpdateMedicineInformation);
            var updateReceipt = await updateFunction.SendTransactionAsync(
                ethereumService.GetEthereumAccount(),
                new HexBigInteger(6000000),
                new HexBigInteger(Nethereum.Web3.Web3.Convert.ToWei(10, UnitConversion.EthUnit.Gwei)),
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
                });
        }
    }
}
