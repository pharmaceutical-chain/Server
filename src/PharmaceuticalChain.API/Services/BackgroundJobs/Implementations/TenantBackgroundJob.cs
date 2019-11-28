using Microsoft.Extensions.Options;
using Nethereum.Hex.HexTypes;
using Nethereum.Util;
using PharmaceuticalChain.API.Models.Database;
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
        private readonly string TenantAbi;

        public TenantBackgroundJob(
            IEthereumService ethereumService,
            ITenantRepository tenantRepository,
            ITenantService tenantService,
            IOptions<EthereumSettings> options)
        {
            this.ethereumService = ethereumService;
            this.tenantRepository = tenantRepository;
            this.tenantService = tenantService;
            TenantAbi = options.Value.TenantAbi;
        }

        void ITenantBackgroundJob.SyncDatabaseWithBlockchain()
        {
            //var allTenants = tenantRepository.GetAll();

            //// Sync contract address
            //var tenantNotHaveContractAddressList = allTenants.Where(t => t.ContractAddress == null).ToList();
            //foreach (var tenantNotHaveContractAddress in tenantNotHaveContractAddressList)
            //{
            //    var receipt = ethereumService.GetTransactionReceipt(tenantNotHaveContractAddress.TransactionHash).Result;
            //    if (receipt == null)
            //        return;
            //    if (receipt.Status.Value == (new HexBigInteger(1)).Value)
            //    {
            //        var contractAddress = tenantService.GetContractAddress(tenantNotHaveContractAddress.Id).Result;
            //        tenantNotHaveContractAddress.ContractAddress = contractAddress;
            //        tenantNotHaveContractAddress.TransactionStatus = Models.Database.TransactionStatuses.Success;
            //        tenantRepository.Update(tenantNotHaveContractAddress);
            //    }
            //}
        }

        void ITenantBackgroundJob.WaitForTransactionToSuccessThenFinishCreatingTenant(Tenant tenant)
        {
            bool isTransactionSuccess = false;
            do
            {
                var receipt = ethereumService.GetTransactionReceipt(tenant.TransactionHash).Result;
                if (receipt == null)
                    continue;
                if (receipt.Status.Value == (new HexBigInteger(1)).Value)
                {
                    isTransactionSuccess = true;
                    var contractAddress = ethereumService.GetObjectContractAddress(tenant.Id).Result;
                    tenant.ContractAddress = contractAddress;
                    tenant.TransactionStatus = TransactionStatuses.Success;
                    tenantRepository.Update(tenant);

                    var tenantContract = ethereumService.GetContract(TenantAbi, tenant.ContractAddress);
                    var updateFunction = ethereumService.GetFunction(tenantContract, EthereumFunctions.UpdateTenantInformation);
                    var updateReceipt = updateFunction.SendTransactionAndWaitForReceiptAsync(
                        ethereumService.GetEthereumAccount(),
                        new HexBigInteger(6000000),
                        new HexBigInteger(Nethereum.Web3.Web3.Convert.ToWei(50, UnitConversion.EthUnit.Gwei)),
                        new HexBigInteger(0),
                        functionInput: new object[] {
                            tenant.Name,
                            tenant.Email,
                            tenant.PrimaryAddress,
                            tenant.PhoneNumber,
                            tenant.TaxCode,
                            tenant.RegistrationCode,
                            tenant.Certificates,
                            (uint)tenant.Type
                        }).Result;
                }

            }
            while (isTransactionSuccess != true);
        }
    }
}
