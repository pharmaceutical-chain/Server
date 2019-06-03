using Nethereum.Hex.HexTypes;
using PharmaceuticalChain.API.Models;
using PharmaceuticalChain.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Implementations
{
    public class CompanyService : ICompanyService
    {
        private readonly IEthereumService ethereumService;

        private readonly IDrugTransactionService drugTransactionService;

        public CompanyService(IEthereumService ethereumService, IDrugTransactionService drugTransactionService)
        {
            this.ethereumService = ethereumService;
            this.drugTransactionService = drugTransactionService;
        }

        async Task<int> ICompanyService.Create(string name)
        {
            var function = ethereumService.GetFunction("addCompany");
            var getTotalFunction = ethereumService.GetFunction("getTotalCompanies");
            try
            {
                var result = await function.SendTransactionAsync(
                    "0xa5eE58Df60d9f6c2FE211D287926948292DffbD3",
                    new HexBigInteger(300000),
                    new HexBigInteger(0),
                    functionInput: new object[] { name });
                return await ethereumService.CallFunction(getTotalFunction); // Total company is the Id of this new company.
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        async Task<List<CompanyInformation>> ICompanyService.GetInformationOfAllCompanies()
        {
            try
            {
                var getCompanyFunction = ethereumService.GetFunction("getCompany");
                var getTotalFunction = ethereumService.GetFunction("getTotalCompanies");

                var total = await ethereumService.CallFunction(getTotalFunction);
                var result = new List<CompanyInformation>();
                CompanyInformation companyInfo = null;
                for (int i = 0; i < total; i++)
                {
                    companyInfo = await getCompanyFunction.CallDeserializingToObjectAsync<CompanyInformation>(
                        "0xa5eE58Df60d9f6c2FE211D287926948292DffbD3",
                        new HexBigInteger(300000),
                        new HexBigInteger(0),
                        functionInput: new object[] { i }
                        );
                    result.Add(companyInfo);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        async Task<List<StorageInformation>> ICompanyService.GetStorageInformation(uint companyId)
        {
            List<StorageInformation> result = new List<StorageInformation>();

            var allTransactions = await drugTransactionService.GetInformationOfAllDrugTransactions();

            var lastTransactions = new List<DrugTransactionInformation>();
            foreach (var transaction in allTransactions)
            {
                if (transaction.ToCompanyId == companyId)
                {
                    lastTransactions.Add(transaction);
                }
            }

            // Find parent transactions all the way to the first
            var chainedTransactions = new List<DrugTransactionInformation>();
            chainedTransactions.AddRange(lastTransactions);
            foreach (var transaction in lastTransactions)
            {
                var iteratorTransaction = transaction;
                var transactionPool = allTransactions.Where(t => t.PackageId == iteratorTransaction.PackageId).ToList();
                bool parentMatchCondition(DrugTransactionInformation t) => t.ToCompanyId == iteratorTransaction.FromCompanyId;
                while (iteratorTransaction.HasParent(transactionPool, parentMatchCondition))
                {
                    iteratorTransaction = iteratorTransaction.Parent(transactionPool, parentMatchCondition);
                    chainedTransactions.Add(iteratorTransaction);
                }
            }

            var transactionsGroupedByDrugName = (from t in chainedTransactions
                                                 group t by t.DrugName into g
                                                 select new { DrugName= g.Key, Transactions = g.ToList() }).ToList();

            return result;
        }

        async Task<int> ICompanyService.GetTotalCompanies()
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
    }
}
