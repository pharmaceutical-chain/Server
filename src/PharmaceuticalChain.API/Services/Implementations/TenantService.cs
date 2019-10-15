﻿using Nethereum.Hex.HexTypes;
using PharmaceuticalChain.API.Models;
using PharmaceuticalChain.API.Repositories.Interfaces;
using PharmaceuticalChain.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using PharmaceuticalChain.API.Models.Database;

namespace PharmaceuticalChain.API.Services.Implementations
{
    public class TenantService : ITenantService
    {
        private readonly IEthereumService ethereumService;

        private readonly IDrugTransactionService drugTransactionService;

        private readonly IReceiptRepository receiptRepository;

        private readonly ITenantRepository tenantRepository;

        public TenantService(
            IEthereumService ethereumService,
            IDrugTransactionService drugTransactionService,
            IReceiptRepository receiptRepository,
            ITenantRepository companyRepository)
        {
            this.ethereumService = ethereumService;
            this.drugTransactionService = drugTransactionService;
            this.receiptRepository = receiptRepository;
            this.tenantRepository = companyRepository;
        }

        async Task<Guid> ITenantService.Create(string name, string address, string phoneNumber, string taxCode, string BRCLink, string GPCLink)
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
                    BRCLink = BRCLink,
                    GPCLink = GPCLink,
                    DateCreated = DateTime.UtcNow
                };
                Guid newTenantId = tenantRepository.CreateAndReturnId(tenant);

                var transactionHash = await function.SendTransactionAsync(
                    ethereumService.GetEthereumAccount(),
                    new HexBigInteger(1000000),
                    new HexBigInteger(0),
                    functionInput: new object[] {
                        newTenantId.ToString(),
                        name,
                        address,
                        phoneNumber,
                        taxCode,
                        BRCLink,
                        GPCLink
                    });
                tenant.TransactionHash = transactionHash;
                tenantRepository.Update(tenant);

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
                        BRCLink = tenant.BRCLink,
                        GPCLink = tenant.GPCLink,
                        TransactionHash = tenant.TransactionHash
                    });
                }
                return result;
            }
            catch (Exception ex)
            {
                throw ex;
            }
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