using PharmaceuticalChain.API.Models.Database;
using PharmaceuticalChain.API.Repositories.Interfaces;
using PharmaceuticalChain.API.Services.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Implementations
{
    public class MedicineBatchTransferService : IMedicineBatchTransferService
    {
        private readonly IMedicineBatchRepository medicineBatchRepository;
        private readonly IMedicineBatchTransferRepository medicineBatchTransferRepository;
        private readonly IEthereumService ethereumService;
        public MedicineBatchTransferService(
            IMedicineBatchRepository medicineBatchRepository,
            IMedicineBatchTransferRepository medicineBatchTransferRepository,
            IEthereumService ethereumService)
        {
            this.medicineBatchRepository = medicineBatchRepository;
            this.medicineBatchTransferRepository = medicineBatchTransferRepository;
            this.ethereumService = ethereumService;
        }

        Task<Guid> IMedicineBatchTransferService.Create(string medicineBatchNumber, Guid fromTenantId, Guid toTenantId, uint quantity)
        {
            var batch = medicineBatchRepository.GetAll().Where(b => b.BatchNumber == medicineBatchNumber).Single();
            var transfer = new MedicineBatchTransfer()
            {
                MedicineBatch = batch,
                MedicineBatchId = batch.Id,
                FromId = fromTenantId,
                ToId = toTenantId,
                Quantity = quantity,
                DateCreated = DateTime.UtcNow
            };
            var newTransferId = medicineBatchTransferRepository.CreateAndReturnId(transfer);

            var function = ethereumService.GetFunction(EthereumFunctions.AddMedicineBatchTransfer);

            return Task.FromResult(Guid.Empty);
        }
    }
}
