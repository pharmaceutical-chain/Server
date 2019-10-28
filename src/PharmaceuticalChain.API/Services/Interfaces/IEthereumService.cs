using Nethereum.Contracts;
using Nethereum.RPC.Eth.DTOs;
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

        string GetMasterContractAddress();

        /// <summary>
        /// Get address of object contracts on the blockchain network.
        /// </summary>
        /// <param name="id">
        ///     Each object contract contains an id.
        ///     This id is created by the backend in the creation process (before it is used to in creation transaction on blockchain.)
        /// </param>
        Task<string> GetObjectContractAddress(Guid id);

        Task<TransactionReceipt> GetTransactionReceipt(string transactionHash);
    }
}
