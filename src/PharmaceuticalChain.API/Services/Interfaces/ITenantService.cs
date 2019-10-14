using PharmaceuticalChain.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Interfaces
{
    public interface ITenantService
    {
        /// <summary>
        /// Create a new company in the database and Ethereum network.
        /// </summary>
        /// <returns>Return the Id of the newly created company</returns>
        Task<Guid> Create(string name, string address, string phoneNumber, string taxCode, string BRCLink, string GPCLink);

        /// <summary>
        /// Remove a tenant in the blockchain.
        /// </summary>
        /// <param name="tenantId"></param>
        /// <returns></returns>
        Task Remove(Guid tenantId);

        Task<int> GetTotalCompanies();

        Task<List<TenantQueryData>> GetAllTenants();

        Task<List<DrugStorageInformation>> GetStorageInformation(uint companyId);

        /// <summary>
        /// Get contract address of a tenant on blockchain network.
        /// </summary>
        /// <param name="id"></param>
        Task<string> GetContractAddress(Guid id);
    }
}
