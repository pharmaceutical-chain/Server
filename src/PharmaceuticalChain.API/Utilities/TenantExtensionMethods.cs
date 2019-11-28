using PharmaceuticalChain.API.Controllers.Models.Queries;
using PharmaceuticalChain.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Utilities
{
    public static class TenantExtensionMethods
    {
        public static TenantQueryData ToTenantQueryData (this Tenant tenant)
        {
            if (tenant == null) return null;
            var result = new TenantQueryData()
            {
                Id = tenant.Id,
                ContractAddress = tenant.ContractAddress,
                DateCreated = tenant.DateCreated,
                Certificates = tenant.Certificates,
                Name = tenant.Name,
                Email = tenant.Email,
                PhoneNumber = tenant.PhoneNumber,
                PrimaryAddress = tenant.PrimaryAddress,
                RegistrationCode = tenant.RegistrationCode,
                TaxCode = tenant.TaxCode,
                TransactionHash = tenant.TransactionHash,
                TransactionStatus = tenant.TransactionStatus.ToString(),
                Type = tenant.Type.ToString()
            };
            return result;
        }
    }
}
