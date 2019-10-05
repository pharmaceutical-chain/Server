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
        Task<int> Create(string name);

        Task<int> GetTotalCompanies();

        Task<List<CompanyInformation>> GetInformationOfAllCompanies();

        Task<List<DrugStorageInformation>> GetStorageInformation(uint companyId);
        void Test();
    }
}
