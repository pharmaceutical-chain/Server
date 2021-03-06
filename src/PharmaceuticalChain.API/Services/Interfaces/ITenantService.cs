﻿using PharmaceuticalChain.API.Controllers.Models.Queries;
using PharmaceuticalChain.API.Models;
using PharmaceuticalChain.API.Models.Database;
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
        Task<Guid> Create(
            string name, 
            string email, 
            string address, 
            string phoneNumber, 
            string taxCode, 
            string registrationCode, 
            string certificates, 
            TenantTypes type);

        Task Update(
            Guid id, 
            string name, 
            string email, 
            string address, 
            string phoneNumber, 
            string taxCode, 
            string registrationCode, 
            string goodPractices,
            TenantTypes type);

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

        TenantQueryData GetTenant(Guid id);
    }
}
