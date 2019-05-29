using Nethereum.Contracts;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Interfaces
{
    public interface IEthereumService
    {
        Contract GetContract();

        void Set(uint value);
        Task<string> Get();
    }
}
