using Nethereum.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Interfaces
{
    public interface IEthereumService
    {
        string GetEthereumAccount();
        string GetTenantABI();

        Contract GetMasterContract();
        Contract GetContract(string abi, string address);
        Function GetFunction(string name);
        Function GetFunction(Contract contract, string name);

        Task<int> CallFunction(Function function, params object[] functionInput);
        Task SendTransaction(Function function, params object[] functionInput);

        string GetContractAddress();
    }
}
