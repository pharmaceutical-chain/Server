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

        Contract GetContract();
        Function GetFunction(string name);

        Task<int> CallFunction(Function function, params object[] functionInput);
        Task SendTransaction(Function function, params object[] functionInput);

        void Set(uint value);
        Task<string> Get();

        string GetContractAddress();
    }
}
