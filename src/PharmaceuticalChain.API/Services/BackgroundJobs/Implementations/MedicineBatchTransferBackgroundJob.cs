using Nethereum.Hex.HexTypes;
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
    public class MedicineBatchTransferBackgroundJob : IMedicineBatchTransferBackgroundJob
    {
        private readonly IEthereumService ethereumService;
        private readonly IMedicineBatchTransferRepository medicineBatchTransferRepository;
        public MedicineBatchTransferBackgroundJob(
            IEthereumService ethereumService,
            IMedicineBatchTransferRepository medicineBatchTransferRepository)
        {
            this.ethereumService = ethereumService;
            this.medicineBatchTransferRepository = medicineBatchTransferRepository;
        }

        void IMedicineBatchTransferBackgroundJob.WaitForTransactionToSuccessThenFinishCreating(MedicineBatchTransfer transfer)
        {
            bool isTransactionSuccess = false;
            do
            {
                var receipt = ethereumService.GetTransactionReceipt(transfer.TransactionHash).Result;
                if (receipt == null)
                    continue;
                if (receipt.Status.Value == (new HexBigInteger(1)).Value)
                {
                    isTransactionSuccess = true;
                    var contractAddress = ethereumService.GetObjectContractAddress(transfer.Id).Result;
                    transfer.ContractAddress = contractAddress;
                    transfer.TransactionStatus = TransactionStatuses.Success;
                    medicineBatchTransferRepository.Update(transfer);
                    
                }
            }
            while (isTransactionSuccess != true);
        }
    }
}
