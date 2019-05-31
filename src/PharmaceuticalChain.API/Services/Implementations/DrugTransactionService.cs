using Nethereum.Hex.HexTypes;
using PharmaceuticalChain.API.Models;
using PharmaceuticalChain.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Implementations
{
    public class DrugTransactionService : IDrugTransactionService
    {
        private readonly IEthereumService ethereumService;
        public DrugTransactionService(IEthereumService ethereumService)
        {
            this.ethereumService = ethereumService;
        }

        async Task<int> IDrugTransactionService.Create(uint fromCompany, uint toCompany, string pillName, string packageId, uint amount)
        {
            try
            {
                var sendFunction = ethereumService.GetFunction("send");


                var result = await sendFunction.SendTransactionAsync(
                    "0xa5eE58Df60d9f6c2FE211D287926948292DffbD3",
                    new HexBigInteger(300000),
                    new HexBigInteger(0),
                    functionInput: new object[]
                    {
                        fromCompany,
                        toCompany,
                        pillName,
                        amount,
                        packageId.ToString()
                    });
                return await (this as IDrugTransactionService).GetTotalTransactions(); // Id of the new transaction
            }
            catch (Exception)
            {
                throw;
            }
        }

        async Task<List<DrugTransactionInformation>> IDrugTransactionService.GetInformationOfAllDrugTransactions()
        {
            try
            {
                var transactionsFunction = ethereumService.GetFunction("transactions");

                var totalTransactions = await (this as IDrugTransactionService).GetTotalTransactions();
                var result = new List<DrugTransactionInformation>();
                DrugTransactionInformation drugTransactionInformation = null;
                for (int i = 0; i < totalTransactions; i++)
                {
                    drugTransactionInformation = await transactionsFunction.CallDeserializingToObjectAsync<DrugTransactionInformation>(
                        "0xa5eE58Df60d9f6c2FE211D287926948292DffbD3",
                        new HexBigInteger(300000),
                        new HexBigInteger(0),
                        functionInput: new object[] { i }
                        );

                    result.Add(drugTransactionInformation);
                }

                return result;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        async Task<int> IDrugTransactionService.GetTotalTransactions()
        {
            try
            {
                var totalTransactions = ethereumService.GetFunction("getTotalTransactions");

                var total = await ethereumService.CallFunction(totalTransactions);
                return total;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
