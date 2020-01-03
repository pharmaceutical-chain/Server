using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Controllers.Models.Commands
{
    public class CreateMedicineBatchCommand
    {
        public string BatchNumber { get; set; }
        public Guid MedicineId { get; set; }
        public Guid ManufacturerId { get; set; }
        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }
        public uint Quantity { get; set; }
        public string Unit { get; set; }
        public string Certificates { get; set; }
        public bool IsApprovedByAdmin { get; set; }
    }
}
