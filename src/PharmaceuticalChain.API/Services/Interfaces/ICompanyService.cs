using PharmaceuticalChain.API.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Interfaces
{
    public interface ICompanyService
    {
        /// <summary>
        /// Create a new company in the database and Ethereum network.
        /// </summary>
        /// <returns>Return the Id of the newly created company</returns>
        Task<Guid> Create(string name, string address, string phoneNumber, string taxCode, string BRCLink, string GPCLink);

        Task<int> GetTotalCompanies();

        Task<List<CompanyInformation>> GetInformationOfAllCompanies();

        Task<List<DrugStorageInformation>> GetStorageInformation(uint companyId);

        /// <summary>
        /// Get contract address of a tenant on blockchain network.
        /// </summary>
        /// <param name="id"></param>
        Task<string> GetContractAddress(Guid id);
    }
}
