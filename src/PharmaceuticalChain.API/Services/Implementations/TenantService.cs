using Nethereum.Hex.HexTypes;
using PharmaceuticalChain.API.Models;
using PharmaceuticalChain.API.Repositories.Interfaces;
using PharmaceuticalChain.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaceuticalChain.API.Models.Database;
using Nethereum.Util;
using PharmaceuticalChain.API.Auth0.Services.Interfaces;
using PharmaceuticalChain.API.Controllers.Models.Queries;
using Hangfire;
using PharmaceuticalChain.API.Services.BackgroundJobs.Interfaces;
using Microsoft.Extensions.Options;
using PharmaceuticalChain.API.Utilities;

namespace PharmaceuticalChain.API.Services.Implementations
{
    public class TenantService : ITenantService
    {
        private readonly IAuth0Service auth0Service;
        private readonly IEthereumService ethereumService;
        private readonly IDrugTransactionService drugTransactionService;
        private readonly ITenantRepository tenantRepository;
        private readonly string TenantAbi;

        public TenantService(
            IAuth0Service auth0Service,
            IEthereumService ethereumService,
            IDrugTransactionService drugTransactionService,
            ITenantRepository tenantRepository,
            IOptions<EthereumSettings> options)
        {
            this.auth0Service = auth0Service;
            this.ethereumService = ethereumService;
            this.drugTransactionService = drugTransactionService;
            this.tenantRepository = tenantRepository;
            TenantAbi = options.Value.TenantAbi;
        }

        async Task<Guid> ITenantService.Create(
            string name,
            string email,
            string address,
            string phoneNumber,
            string taxCode,
            string registrationCode,
            string goodPractices,
            TenantTypes type)
        {
            var function = ethereumService.GetFunction(EthereumFunctions.AddChainPoint);
            try
            {
                var tenant = new Tenant()
                {
                    Name = name,
                    Email = email,
                    PrimaryAddress = address,
                    PhoneNumber = phoneNumber,
                    TaxCode = taxCode,
                    RegistrationCode = registrationCode,
                    GoodPractices = goodPractices,
                    DateCreated = DateTime.UtcNow,
                    Type = type
                };
                Guid newTenantId = tenantRepository.CreateAndReturnId(tenant);

                var transactionHash = await function.SendTransactionAsync(
                    ethereumService.GetEthereumAccount(),
                    new HexBigInteger(4000000),
                    new HexBigInteger(Nethereum.Web3.Web3.Convert.ToWei(50, UnitConversion.EthUnit.Gwei)),
                    new HexBigInteger(0),
                    functionInput: new object[] {
                        newTenantId.ToString(),
                        name,
                        address,
                        phoneNumber,
                        taxCode,
                        registrationCode
                    });

                tenant.TransactionHash = transactionHash;
                tenantRepository.Update(tenant);

                // Create auth0 user.
                var userRole = (type == TenantTypes.Manufacturer) ? "manufacturer" : (type == TenantTypes.Distributor ? "distributor" : (type == TenantTypes.Retailer ? "retailer" : "unknown"));
                var userAuth0 = auth0Service.CreateUser(newTenantId.ToString(), email, "123456789?a", userRole);

                BackgroundJob.Schedule<ITenantBackgroundJob>(
                    backgroundJob => backgroundJob.WaitForTransactionToSuccessThenFinishCreatingTenant(tenant),
                    TimeSpan.FromSeconds(3)
                );

                return newTenantId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async Task<string> ITenantService.GetContractAddress(Guid id)
        {
            var function = ethereumService.GetFunction("getAddressByID");
            try
            {
                var result = await function.CallAsync<string>(
                   ethereumService.GetEthereumAccount(),
                   new HexBigInteger(600000),
                   new HexBigInteger(0),
                   functionInput: new object[]
                   {
                       id.ToString()
                   });
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async Task ITenantService.Remove(Guid tenantId)
        {
            var function = ethereumService.GetFunction(EthereumFunctions.RemoveChainPoint);
            try
            {
                var transactionHash = await function.SendTransactionAsync(
                                  ethereumService.GetEthereumAccount(),
                                  new HexBigInteger(1000000),
                                  new HexBigInteger(0),
                                  functionInput: new object[] {
                                       tenantId.ToString()
                });
                tenantRepository.Delete(tenantId);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        async Task<List<TenantQueryData>> ITenantService.GetAllTenants()
        {
            try
            {
                var tenants = tenantRepository.GetAll();
                List<TenantQueryData> result = new List<TenantQueryData>();
                foreach (var tenant in tenants)
                {
                    result.Add(tenant.ToTenantQueryData());
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        TenantQueryData ITenantService.GetTenant(Guid id)
        {
            var tenant = tenantRepository.Get(id);
            var result = tenant.ToTenantQueryData();
            return result;
        }

        async Task ITenantService.Update(
            Guid id, 
            string name, 
            string email, 
            string address, 
            string phoneNumber, 
            string taxCode, 
            string registrationCode, 
            string goodPractices, 
            TenantTypes type)
        {
            var tenant = tenantRepository.Get(id);
            if (tenant == null)
            {
                throw new ArgumentException("Id does not exist in the system.", nameof(id));
            }

            tenant.Name = name;
            tenant.Email = email;
            tenant.PrimaryAddress = address;
            tenant.PhoneNumber = phoneNumber;
            tenant.TaxCode = taxCode;
            tenant.RegistrationCode = registrationCode;
            tenant.GoodPractices = goodPractices;
            tenant.Type = type;

            tenantRepository.Update(tenant);

            // Update the blockchain.
            var tenantContract = ethereumService.GetContract(TenantAbi, tenant.ContractAddress);
            var updateFunction = ethereumService.GetFunction(tenantContract, EthereumFunctions.UpdateTenantInformation);
            var updateReceipt = await updateFunction.SendTransactionAsync(
                ethereumService.GetEthereumAccount(),
                new HexBigInteger(6000000),
                new HexBigInteger(Nethereum.Web3.Web3.Convert.ToWei(20, UnitConversion.EthUnit.Gwei)),
                new HexBigInteger(0),
                functionInput: new object[] {
                    tenant.Name,
                    tenant.Email,
                    tenant.PrimaryAddress,
                    tenant.PhoneNumber,
                    tenant.TaxCode,
                    tenant.RegistrationCode,
                    tenant.GoodPractices,
                    (uint)tenant.Type
                });

        }


        #region OLD & WAITING TO BE REFACTOR-ED
        async Task<List<DrugStorageInformation>> ITenantService.GetStorageInformation(uint companyId)
        {
            //List<StorageInformation> result = new List<StorageInformation>();

            //var allTransactions = await drugTransactionService.GetInformationOfAllDrugTransactions();

            //var lastTransactions = new List<DrugTransactionInformation>();
            //foreach (var transaction in allTransactions)
            //{
            //    if (transaction.ToCompanyId == companyId)
            //    {
            //        lastTransactions.Add(transaction);
            //    }
            //}

            //// Find parent transactions all the way to the first
            //var chainedTransactions = new List<DrugTransactionInformation>();
            //chainedTransactions.AddRange(lastTransactions);
            //foreach (var transaction in lastTransactions)
            //{
            //    var iteratorTransaction = transaction;
            //    var transactionPool = allTransactions.Where(t => t.PackageId == iteratorTransaction.PackageId).ToList();
            //    bool parentMatchCondition(DrugTransactionInformation t) => t.ToCompanyId == iteratorTransaction.FromCompanyId;
            //    while (iteratorTransaction.HasParent(transactionPool, parentMatchCondition))
            //    {
            //        iteratorTransaction = iteratorTransaction.Parent(transactionPool, parentMatchCondition);
            //        chainedTransactions.Add(iteratorTransaction);
            //    }
            //}

            //var transactionsGroupedByDrugName = (from t in chainedTransactions
            //                                     group t by new { t.DrugName, t.PackageId } into g
            //                                     select new DrugStorageInformation(
            //                                         g.Key.DrugName,
            //                                         g.Key.PackageId,
            //                                         g.Select(p => new ChainHistory(p.FromCompanyId, p.Amount)).Reverse().ToList()
            //                                     )).ToList();

            //var companies = await (this as ITenantService).GetAllTenants();

            //foreach (var transaction in transactionsGroupedByDrugName)
            //{
            //    foreach (var item in transaction.ChainHistories)
            //    {
            //        item.CompanyName = companies.Where(c => c.CompanyId == item.CompanyId).Single().Name;
            //    }

            //    // Add the last node which is the company is being the parent search.
            //    ChainHistory rootCompany = new ChainHistory(companyId, 0);
            //    rootCompany.CompanyName = companies.Where(c => c.CompanyId == rootCompany.CompanyId).Single().Name;
            //    transaction.ChainHistories.Add(rootCompany);
            //}

            //return transactionsGroupedByDrugName;
            throw new NotImplementedException();
        }

        async Task<int> ITenantService.GetTotalCompanies()
        {
            var function = ethereumService.GetFunction("getTotalCompanies");
            try
            {
                var result = await ethereumService.CallFunction(function);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }
        #endregion
    }
}
