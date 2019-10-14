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
        private readonly string abi;

        //private readonly string contractAddress = "0x3e18A6DB759fCB7429f1Bd73C9E1C94875450aB8"; // POA Consortium
        private readonly string contractAddress;

        public EthereumService(IOptions<EthereumSettings> options)
        {
            ethereumAccount = options.Value.EthereumAccount;
            ethereumPassword = options.Value.EthereumPassword;
            abi = options.Value.Abi;
            contractAddress = options.Value.ContractAddress;

            var privateKey = "0xA32C64EBF23356CE1C6E8968802515DF9AD769162741EFA693E48E1F98FE9EBE";
            var account = new Account(privateKey);
            web3 = new Web3(account, "https://ropsten.infura.io/v3/ad8ea364154b464eb6c7ff37f66ffc94");
            //web3 = new Web3(account, "http://ethecctzc-dns-reg1.southeastasia.cloudapp.azure.com:8540");

        }

        Contract IEthereumService.GetContract()
        {
           return web3.Eth.GetContract(abi, contractAddress);
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

        string IEthereumService.GetEthereumAccount()
        {
            return ethereumAccount;
        }
    }
}
