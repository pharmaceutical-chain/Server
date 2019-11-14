using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models.Database
{
    [Newtonsoft.Json.JsonObject(IsReference = true)]
    public class MedicineBatch : BlockchainObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string BatchNumber { get; set; }

        public Guid MedicineId { get; set; }
        public Medicine Medicine { get; set; }

        public Guid ManufacturerId { get; set; }
        public Tenant Manufacturer { get; set; }

        public DateTime ManufactureDate { get; set; }
        public DateTime ExpiryDate { get; set; }

        public uint Quantity { get; set; }
        public string Unit { get; set; }

        public ICollection<MedicineBatchTransfer> MedicineBatchTransfers { get; set; }
    }
}
