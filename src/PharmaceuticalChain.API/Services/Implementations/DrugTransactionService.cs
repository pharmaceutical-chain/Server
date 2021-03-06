﻿using Nethereum.Hex.HexTypes;
using PharmaceuticalChain.API.Models;
using PharmaceuticalChain.API.Models.Database;
using PharmaceuticalChain.API.Models.Ethereum;
using PharmaceuticalChain.API.Repositories.Interfaces;
using PharmaceuticalChain.API.Services.Interfaces;
using PharmaceuticalChain.API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Implementations
{
    public class DrugTransactionService : IDrugTransactionService
    {
        private readonly IEthereumService ethereumService;
        private readonly ITransactionRepository transactionRepository;
        public DrugTransactionService(
            IEthereumService ethereumService,
            ITransactionRepository transactionRepository)
        {
            this.ethereumService = ethereumService;
            this.transactionRepository = transactionRepository;
        }

        async Task<CreateDrugTransactionResult> IDrugTransactionService.Create(
            uint fromCompany, uint toCompany, string pillName, string packageId, uint amount, DateTime manufacureDate, DateTime expirationDate, Guid receiptId)
        {
            try
            {
                var id = await (this as IDrugTransactionService).GetTotalTransactions();

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
                        packageId.ToString(),
                        manufacureDate.ToUnixTimestamp(),
                        expirationDate.ToUnixTimestamp()
                    });

                

                transactionRepository.Create(new Transaction()
                {
                    EthereumTransactionHash = result,
                    Id = (uint)id,
                    ReceiptId = receiptId,
                    DrugName = pillName,
                    Amount = amount,
                    PackageId = packageId
                });

                return new CreateDrugTransactionResult()
                {
                    TransactionHash = result,
                    TransactionId = id

                };
            }
            catch (Exception)
            {
                throw;
            }
        }

        Guid IDrugTransactionService.CreateAndReturnReceipt(int companyId, int toCompanyId)
        {
            // TODO: Remove
            throw new NotImplementedException();
        }

        bool IDrugTransactionService.DoesReceiptExist(Guid receiptId)
        {
            throw new NotImplementedException();
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
