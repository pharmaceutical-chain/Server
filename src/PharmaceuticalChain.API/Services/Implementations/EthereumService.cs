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
        private readonly string abi = "[ { \"constant\": false, \"inputs\": [ { \"name\": \"_value\", \"type\": \"uint256\" } ], \"name\": \"set\", \"outputs\": [], \"payable\": false, \"stateMutability\": \"nonpayable\", \"type\": \"function\" }, { \"constant\": true, \"inputs\": [], \"name\": \"get\", \"outputs\": [ { \"name\": \"\", \"type\": \"uint256\" } ], \"payable\": false, \"stateMutability\": \"view\", \"type\": \"function\" } ]";
        private readonly string contractAddress = "0xbde685f99327b4107960bc955b029a0c47e18c82";

        public EthereumService(IOptions<EthereumSettings> options)
        {
            ethereumAccount = options.Value.EthereumAccount;
            ethereumPassword = options.Value.EthereumPassword;

            // Currently using testing account provided on Nethereum Docs.
            var privateKey = "0xb5b1870957d373ef0eeffecc6e4812c0fd08f554b37b233526acc331bf1544f7";
            var account = new Account(privateKey);
            web3 = new Web3(account, "https://ropsten.infura.io/v3/ad8ea364154b464eb6c7ff37f66ffc94");

            
        }

        Contract IEthereumService.GetContract()
        {
           return web3.Eth.GetContract(abi, contractAddress);
        }

        async void IEthereumService.Set(uint value)
        {
            var method = GetFunction("set");
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

            var method = GetFunction("get");
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

        private Function GetFunction(string name)
        {
            var contract = (this as IEthereumService).GetContract();
            return contract.GetFunction(name);
        }
    }
}
