using Nethereum.Hex.HexTypes;
using PharmaceuticalChain.API.Repositories.Interfaces;
using PharmaceuticalChain.API.Services.BackgroundJobs.Interfaces;
using PharmaceuticalChain.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.BackgroundJobs.Implementations
{
    public class TenantBackgroundJob : ITenantBackgroundJob
    {
        private readonly IEthereumService ethereumService;
        private readonly ITenantRepository tenantRepository;
        private readonly ITenantService tenantService;
        
        public TenantBackgroundJob(
            IEthereumService ethereumService,
            ITenantRepository tenantRepository,
            ITenantService tenantService)
        {
            this.ethereumService = ethereumService;
            this.tenantRepository = tenantRepository;
            this.tenantService = tenantService;
        }

        void ITenantBackgroundJob.SyncDatabaseWithBlockchain()
        {
            var allTenants = tenantRepository.GetAll();

            // Sync contract address
            var tenantNotHaveContractAddressList = allTenants.Where(t => t.ContractAddress == null).ToList();
            foreach (var tenantNotHaveContractAddress in tenantNotHaveContractAddressList)
            {
                var receipt = ethereumService.GetTransactionReceipt(tenantNotHaveContractAddress.TransactionHash).Result;
                if (receipt == null)
                    return;
                if (receipt.Status.Value == (new HexBigInteger(1)).Value)
                {
                    var contractAddress = tenantService.GetContractAddress(tenantNotHaveContractAddress.Id).Result;
                    tenantNotHaveContractAddress.ContractAddress = contractAddress;
                    tenantNotHaveContractAddress.TransactionStatus = Models.Database.TransactionStatuses.Success;
                    tenantRepository.Update(tenantNotHaveContractAddress);
                }
            }
        }
    }
}
