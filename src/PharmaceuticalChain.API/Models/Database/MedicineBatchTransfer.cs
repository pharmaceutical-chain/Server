using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models.Database
{
    public class MedicineBatchTransfer : BlockchainObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }

        public Guid MedicineBatchId { get; set; }
        public MedicineBatch MedicineBatch { get; set; }

        [Required]
        public Guid FromId { get; set; }
        public Tenant From { get; set; }

        [Required]
        public Guid ToId { get; set; }
        public Tenant To { get; set; }

        public uint Quantity { get; set; }
    }
}
