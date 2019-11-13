using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Controllers.Models.Commands
{
    public class CreateMedicineBatchTransferCommand
    {
        public string MedicineBatchNumber { get; set; }

        public Guid FromTenantId { get; set; }
        public Guid ToTenantId { get; set; }

        public uint Quantity { get; set; }
    }
}
