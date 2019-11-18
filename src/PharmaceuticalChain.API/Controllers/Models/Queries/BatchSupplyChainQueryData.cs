using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Controllers.Models.Queries
{
    public class BatchSupplyChainQueryData
    {
        public uint TotalTenants { get; set; }
        public uint TotalTransfers { get; set; }

        public List<List<MedicineBatchTransferQueryData>> TransferChains { get; set; }
    }
}
