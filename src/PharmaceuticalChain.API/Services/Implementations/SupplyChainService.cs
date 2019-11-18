using PharmaceuticalChain.API.Controllers.Models.Queries;
using PharmaceuticalChain.API.Models.Database;
using PharmaceuticalChain.API.Repositories.Interfaces;
using PharmaceuticalChain.API.Services.Interfaces;
using PharmaceuticalChain.API.Utilities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Services.Implementations
{
    public class SupplyChainService : ISupplyChainService
    {
        private readonly IMedicineBatchRepository medicineBatchRepository;
        private readonly IMedicineBatchTransferRepository medicineBatchTransferRepository;
        public SupplyChainService(
            IMedicineBatchRepository medicineBatchRepository,
            IMedicineBatchTransferRepository medicineBatchTransferRepository)
        {
            this.medicineBatchRepository = medicineBatchRepository;
            this.medicineBatchTransferRepository = medicineBatchTransferRepository;
        }

        BatchSupplyChainQueryData ISupplyChainService.GetBatchSupplyChain(Guid batchId)
        {
            var result = new BatchSupplyChainQueryData();

            var transfersOfThatBatch = medicineBatchTransferRepository.GetAll()
                .Where(t => t.MedicineBatchId == batchId)
                .ToList();

            // Get all last tier transfers
            var higestTier = transfersOfThatBatch.Max(t => t.Tier);
            List<MedicineBatchTransfer> lastTierTransfers = transfersOfThatBatch
                .Where(t => t.Tier == higestTier)
                .ToList();

            result.TransferChains = new List<List<MedicineBatchTransferQueryData>>();
            // Trace backwards
            foreach (var transfer in lastTierTransfers)
            {
                List<MedicineBatchTransferQueryData> transferChain = new List<MedicineBatchTransferQueryData>();
                var iterator = transfer;
                var transferPool = transfersOfThatBatch;
                bool parentMatchCondition(MedicineBatchTransfer t) => t.ToId == iterator.FromId;

                transferChain.Add(iterator.ToMedicineBatchTransferQueryData());

                while (iterator.HasParent(transferPool, parentMatchCondition))
                {
                    iterator = iterator.Parent(transferPool, parentMatchCondition).Single();
                    transferChain.Add(iterator.ToMedicineBatchTransferQueryData());
                }

                result.TransferChains.Add(transferChain);
            }

            // Finalize the result
            

            return result;
        }
    }
}
