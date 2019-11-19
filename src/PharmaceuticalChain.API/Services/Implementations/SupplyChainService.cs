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

        DetailedBatchSupplyChainQueryData ISupplyChainService.GetDetailedBatchSupplyChain(Guid batchId)
        {
            var result = new DetailedBatchSupplyChainQueryData();

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

            // Calculate the summary section
            var tempTransfers = new List<MedicineBatchTransferQueryData>();
            foreach (var chain in result.TransferChains)
            {
                tempTransfers.AddRange(chain);
            }
            var tempTenants = new List<TenantQueryData>();
            foreach(var transfer in tempTransfers)
            {
                tempTenants.Add(transfer.From);
                tempTenants.Add(transfer.To);
            }
            result.TotalTransfers = (uint)tempTransfers.Count();
            result.TotalTenants = (uint)tempTenants.GroupBy(t => t.Id).Count();

            // Format the result
            foreach(var chain in result.TransferChains)
            {
                chain.Reverse();
            }

            return result;
        }

        BatchSupplyChainQueryData ISupplyChainService.GetSimpleBatchSupplyChain(Guid batchId)
        {
            var result = new BatchSupplyChainQueryData();

            var tempTenants = new List<Tenant>();

            var transfersOfThatBatch = medicineBatchTransferRepository.GetAll()
                .Where(t => t.MedicineBatchId == batchId)
                .ToList();

            transfersOfThatBatch = transfersOfThatBatch.OrderBy(t => t.Tier).ToList();

            foreach(var transfer in transfersOfThatBatch)
            {
                result.Transfers.Add(transfer.ToMedicineBatchTransferQueryData());

                tempTenants.Add(transfer.From);
                tempTenants.Add(transfer.To);
            }

            result.TotalTransfers = (uint)result.Transfers.Count();
            result.TotalTenants = (uint)tempTenants.GroupBy(t => t.Id).Count();

            return result;
        }
    }
}
