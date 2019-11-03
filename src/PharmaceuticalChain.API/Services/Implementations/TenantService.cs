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

namespace PharmaceuticalChain.API.Services.Implementations
{
    public class TenantService : ITenantService
    {
        private readonly IAuth0Service auth0Service;

        private readonly IEthereumService ethereumService;

        private readonly IDrugTransactionService drugTransactionService;

        private readonly ITenantRepository tenantRepository;

        public TenantService(
            IAuth0Service auth0Service,
            IEthereumService ethereumService,
            IDrugTransactionService drugTransactionService,
            ITenantRepository companyRepository)
        {
            this.auth0Service = auth0Service;
            this.ethereumService = ethereumService;
            this.drugTransactionService = drugTransactionService;
            this.tenantRepository = companyRepository;
        }

        async Task<Guid> ITenantService.Create(
            string name,
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
                        registrationCode,
                        goodPractices
                    });

                tenant.TransactionHash = transactionHash;
                tenantRepository.Update(tenant);

                // Create auth0 user
                var userRole = (type == TenantTypes.Manufacturer) ? "manufacturer" : (type == TenantTypes.Distributor ? "distributor" : (type == TenantTypes.Retailer ? "retailer" : "unknown"));
                var userAuth0 = auth0Service.CreateUser(newTenantId.ToString(), $"{name}.pca@yopmail.com", "123456789?a", userRole);

                //var updateFunction = ethereumService.GetFunction(
                //    ethereumService.GetContract(ethereumService.GetTenantABI(), await (this as ITenantService).GetContractAddress(tenant.Id)),
                //    EthereumFunctions.UpdateTenantType);
                //var t = await updateFunction.SendTransactionAsync(
                //    ethereumService.GetEthereumAccount(),
                //    new HexBigInteger(700000),
                //    new HexBigInteger(0),
                //    functionInput: new object[] {
                //        type
                //    });

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
                    result.Add(new TenantQueryData()
                    {
                        Id = tenant.Id,
                        Name = tenant.Name,
                        PrimaryAddress = tenant.PrimaryAddress,
                        TaxCode = tenant.TaxCode,
                        PhoneNumber = tenant.PhoneNumber,
                        RegistrationCode = tenant.RegistrationCode,
                        GoodPractices = tenant.GoodPractices,
                        TransactionHash = tenant.TransactionHash,
                        TransactionStatus = tenant.TransactionStatus.ToString(),
                        DateCreated = tenant.DateCreated,
                        ContractAddress = tenant.ContractAddress,
                        Type = tenant.Type.ToString()
                    });
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
            var result = new TenantQueryData()
            {
                Id = tenant.Id,
                Name = tenant.Name,
                PrimaryAddress = tenant.PrimaryAddress,
                TaxCode = tenant.TaxCode,
                PhoneNumber = tenant.PhoneNumber,
                RegistrationCode = tenant.RegistrationCode,
                GoodPractices = tenant.GoodPractices,
                TransactionHash = tenant.TransactionHash,
                TransactionStatus = tenant.TransactionStatus.ToString(),
                DateCreated = tenant.DateCreated,
                ContractAddress = tenant.ContractAddress,
                Type = tenant.Type.ToString()
            };
            return result;
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
