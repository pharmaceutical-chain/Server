using Microsoft.Extensions.Options;
using Nethereum.Contracts;
using Nethereum.Hex.HexTypes;
using Nethereum.Web3;
using Nethereum.Web3.Accounts;
using Nethereum.Web3.Accounts.Managed;
using PharmaceuticalChain.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Implementations
{
    public class EthereumService : IEthereumService
    {
        private readonly string ethereumAccount;
        private readonly string ethereumPassword;

        private Web3 web3;
        private readonly string abi = "[\n\t{\n\t\t\"constant\": false,\n\t\t\"inputs\": [\n\t\t\t{\n\t\t\t\t\"name\": \"companyName\",\n\t\t\t\t\"type\": \"bytes32\"\n\t\t\t}\n\t\t],\n\t\t\"name\": \"addCompany\",\n\t\t\"outputs\": [\n\t\t\t{\n\t\t\t\t\"name\": \"\",\n\t\t\t\t\"type\": \"uint256\"\n\t\t\t}\n\t\t],\n\t\t\"payable\": false,\n\t\t\"stateMutability\": \"nonpayable\",\n\t\t\"type\": \"function\"\n\t},\n\t{\n\t\t\"constant\": false,\n\t\t\"inputs\": [\n\t\t\t{\n\t\t\t\t\"name\": \"uid\",\n\t\t\t\t\"type\": \"bytes32\"\n\t\t\t},\n\t\t\t{\n\t\t\t\t\"name\": \"fromCompanyId\",\n\t\t\t\t\"type\": \"uint256\"\n\t\t\t},\n\t\t\t{\n\t\t\t\t\"name\": \"toCompanyId\",\n\t\t\t\t\"type\": \"uint256\"\n\t\t\t},\n\t\t\t{\n\t\t\t\t\"name\": \"drugName\",\n\t\t\t\t\"type\": \"bytes32\"\n\t\t\t},\n\t\t\t{\n\t\t\t\t\"name\": \"amount\",\n\t\t\t\t\"type\": \"uint256\"\n\t\t\t},\n\t\t\t{\n\t\t\t\t\"name\": \"packageId\",\n\t\t\t\t\"type\": \"string\"\n\t\t\t}\n\t\t],\n\t\t\"name\": \"send\",\n\t\t\"outputs\": [],\n\t\t\"payable\": false,\n\t\t\"stateMutability\": \"nonpayable\",\n\t\t\"type\": \"function\"\n\t},\n\t{\n\t\t\"anonymous\": false,\n\t\t\"inputs\": [\n\t\t\t{\n\t\t\t\t\"indexed\": false,\n\t\t\t\t\"name\": \"companyId\",\n\t\t\t\t\"type\": \"uint256\"\n\t\t\t}\n\t\t],\n\t\t\"name\": \"AddedCompany\",\n\t\t\"type\": \"event\"\n\t},\n\t{\n\t\t\"constant\": true,\n\t\t\"inputs\": [\n\t\t\t{\n\t\t\t\t\"name\": \"companyId\",\n\t\t\t\t\"type\": \"uint256\"\n\t\t\t}\n\t\t],\n\t\t\"name\": \"getCompany\",\n\t\t\"outputs\": [\n\t\t\t{\n\t\t\t\t\"name\": \"\",\n\t\t\t\t\"type\": \"uint256\"\n\t\t\t},\n\t\t\t{\n\t\t\t\t\"name\": \"\",\n\t\t\t\t\"type\": \"bytes32\"\n\t\t\t}\n\t\t],\n\t\t\"payable\": false,\n\t\t\"stateMutability\": \"view\",\n\t\t\"type\": \"function\"\n\t},\n\t{\n\t\t\"constant\": true,\n\t\t\"inputs\": [\n\t\t\t{\n\t\t\t\t\"name\": \"companyId\",\n\t\t\t\t\"type\": \"uint256\"\n\t\t\t}\n\t\t],\n\t\t\"name\": \"getCurrentStorageOfCompany\",\n\t\t\"outputs\": [\n\t\t\t{\n\t\t\t\t\"name\": \"\",\n\t\t\t\t\"type\": \"bytes32[]\"\n\t\t\t},\n\t\t\t{\n\t\t\t\t\"name\": \"\",\n\t\t\t\t\"type\": \"uint256[]\"\n\t\t\t}\n\t\t],\n\t\t\"payable\": false,\n\t\t\"stateMutability\": \"view\",\n\t\t\"type\": \"function\"\n\t},\n\t{\n\t\t\"constant\": true,\n\t\t\"inputs\": [],\n\t\t\"name\": \"getTotalCompanies\",\n\t\t\"outputs\": [\n\t\t\t{\n\t\t\t\t\"name\": \"\",\n\t\t\t\t\"type\": \"uint256\"\n\t\t\t}\n\t\t],\n\t\t\"payable\": false,\n\t\t\"stateMutability\": \"view\",\n\t\t\"type\": \"function\"\n\t},\n\t{\n\t\t\"constant\": true,\n\t\t\"inputs\": [],\n\t\t\"name\": \"getTotalTransactions\",\n\t\t\"outputs\": [\n\t\t\t{\n\t\t\t\t\"name\": \"\",\n\t\t\t\t\"type\": \"uint256\"\n\t\t\t}\n\t\t],\n\t\t\"payable\": false,\n\t\t\"stateMutability\": \"view\",\n\t\t\"type\": \"function\"\n\t},\n\t{\n\t\t\"constant\": true,\n\t\t\"inputs\": [\n\t\t\t{\n\t\t\t\t\"name\": \"transactionId\",\n\t\t\t\t\"type\": \"uint256\"\n\t\t\t}\n\t\t],\n\t\t\"name\": \"getTransaction\",\n\t\t\"outputs\": [\n\t\t\t{\n\t\t\t\t\"name\": \"\",\n\t\t\t\t\"type\": \"uint256\"\n\t\t\t},\n\t\t\t{\n\t\t\t\t\"name\": \"\",\n\t\t\t\t\"type\": \"uint256\"\n\t\t\t},\n\t\t\t{\n\t\t\t\t\"name\": \"\",\n\t\t\t\t\"type\": \"uint256\"\n\t\t\t},\n\t\t\t{\n\t\t\t\t\"name\": \"\",\n\t\t\t\t\"type\": \"bytes32\"\n\t\t\t},\n\t\t\t{\n\t\t\t\t\"name\": \"\",\n\t\t\t\t\"type\": \"uint256\"\n\t\t\t}\n\t\t],\n\t\t\"payable\": false,\n\t\t\"stateMutability\": \"view\",\n\t\t\"type\": \"function\"\n\t}\n]";
        private readonly string contractAddress = "0x6d1a4ab28a99ff0f8afee8929acf2ea5d263bd41";

        public EthereumService(IOptions<EthereumSettings> options)
        {
            ethereumAccount = options.Value.EthereumAccount;
            ethereumPassword = options.Value.EthereumPassword;

            // Currently using testing account provided on Nethereum Docs.
            var privateKey = "0x8B0081C342F0E99CE0EE725F78446BACACA5F260B2D9E69ABC9494EF5FE6DD86";
            var account = new Account(privateKey);
            web3 = new Web3(account, "https://ropsten.infura.io/v3/ad8ea364154b464eb6c7ff37f66ffc94");

            
        }

        Contract IEthereumService.GetContract()
        {
           return web3.Eth.GetContract(abi, contractAddress);
        }

        async void IEthereumService.Set(uint value)
        {
            var method = (this as IEthereumService).GetFunction("set");
            var estimate = await method.EstimateGasAsync();
            try
            {
                var result = await method.SendTransactionAsync(ethereumAccount,
                    new HexBigInteger(300000),
                    new HexBigInteger(0),
                    value);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        async Task<string> IEthereumService.Get()
        {
            //var isUnlokced = await web3.Personal.UnlockAccount.SendRequestAsync(ethereumAccount, ethereumPassword, 60);
            //if (!isUnlokced) { throw new UnauthorizedAccessException(); }

            var method = (this as IEthereumService).GetFunction("get");
            var estimate = await method.EstimateGasAsync();
            try
            {
                var result = await method.CallAsync<int>(ethereumAccount,
                    new HexBigInteger(estimate.Value / 100 * 140),
                    new HexBigInteger(0));
                return result.ToString();
            }
            catch (Exception ex)
            {
                throw;
            }
        }


        Function IEthereumService.GetFunction(string name)
        {
            var contract = (this as IEthereumService).GetContract();
            return contract.GetFunction(name);
        }

        async Task<int> IEthereumService.CallFunction(Function function, params object[] functionInput)
        {
            var estimate = await function.EstimateGasAsync();
            try
            {
                var result = await function.CallAsync<int>(
                    ethereumAccount,
                    new HexBigInteger(300000),
                    new HexBigInteger(0),
                    functionInput);
                return result;
            }
            catch (Exception)
            {
                throw;
            }
        }

        Task IEthereumService.SendTransaction(Function function, params object[] functionInput)
        {
            throw new NotImplementedException();
        }

        string IEthereumService.GetContractAddress()
        {
            return contractAddress;
        }
    }
}
