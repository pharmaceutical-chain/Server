using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Controllers.Models.Queries
{
    public class BatchSupplyChainQueryData
    {
        public uint TotalTenants { get; set; } = 0;
        public uint TotalTransfers { get; set; } = 0;

        public List<MedicineBatchTransferQueryData> Transfers { get; set; }

        public BatchSupplyChainQueryData()
        {
            Transfers = new List<MedicineBatchTransferQueryData>();
        }
    }
}
