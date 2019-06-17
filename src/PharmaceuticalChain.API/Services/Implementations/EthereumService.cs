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
        private readonly string abi = "[\r\n\t{\r\n\t\t\"constant\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"companyName\",\r\n\t\t\t\t\"type\": \"bytes32\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"addCompany\",\r\n\t\t\"outputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"nonpayable\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"fromCompanyId\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"toCompanyId\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"drugName\",\r\n\t\t\t\t\"type\": \"bytes32\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"amount\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"packageId\",\r\n\t\t\t\t\"type\": \"bytes32\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"mfgDate\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"expDate\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"send\",\r\n\t\t\"outputs\": [],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"nonpayable\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"anonymous\": false,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"indexed\": false,\r\n\t\t\t\t\"name\": \"companyId\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"AddedCompany\",\r\n\t\t\"type\": \"event\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": true,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"companyId\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"getCompany\",\r\n\t\t\"outputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"bytes32\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"view\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": true,\r\n\t\t\"inputs\": [],\r\n\t\t\"name\": \"getTotalCompanies\",\r\n\t\t\"outputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"view\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": true,\r\n\t\t\"inputs\": [],\r\n\t\t\"name\": \"getTotalTransactions\",\r\n\t\t\"outputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"view\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": true,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"transactionId\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"getTransaction\",\r\n\t\t\"outputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"bytes32\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"view\",\r\n\t\t\"type\": \"function\"\r\n\t},\r\n\t{\r\n\t\t\"constant\": true,\r\n\t\t\"inputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"name\": \"transactions\",\r\n\t\t\"outputs\": [\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"fromCompanyId\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"toCompanyId\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"drugName\",\r\n\t\t\t\t\"type\": \"bytes32\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"amount\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"packageId\",\r\n\t\t\t\t\"type\": \"bytes32\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"manufactureDate\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t},\r\n\t\t\t{\r\n\t\t\t\t\"name\": \"expirationDate\",\r\n\t\t\t\t\"type\": \"uint256\"\r\n\t\t\t}\r\n\t\t],\r\n\t\t\"payable\": false,\r\n\t\t\"stateMutability\": \"view\",\r\n\t\t\"type\": \"function\"\r\n\t}\r\n]";

        private readonly string contractAddress = "0xd25846647620595f7e2626356bb554797b968991";

        // Local
        //private readonly string contractAddress = "0xafa0e5114e8c8cac9ae6addc93d380d27b790b54";

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
