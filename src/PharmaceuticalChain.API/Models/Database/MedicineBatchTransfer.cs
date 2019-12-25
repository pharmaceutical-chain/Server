using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models.Database
{
    [Newtonsoft.Json.JsonObject(IsReference = true)]
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

        /// <summary>
        /// The sender creates the transfer, but it's not guaranteed on the other end. Did the recipient of the transfer confirm? 
        /// </summary>
        public bool IsConfirmed { get; set; } = false;

        /// <summary>
        /// Tier of the transfer in the supply chain, zero-based.
        /// Tier of a transfer is determined by the tier of the sending tenant.
        /// The focal tenant is the manufacturer of a batch. Tier (tier customer) is count from there.
        /// </summary>
        /// <example>
        ///     - Tier 0 means the transfer is made from the manufacturer.
        ///     - If 2nd-tier tenant sends a transfer to 4th-tier tenant, tier of the transfer is 2.
        /// </example>
        [Required]
        public uint Tier { get; set; }

        internal bool HasParent(
            IEnumerable<MedicineBatchTransfer> transfers,
            Func<MedicineBatchTransfer, bool> parentMatchCondition)
        {
            if (transfers.Any(parentMatchCondition))
            {
                return true;
            }
            else
            {
                return false;
            }
        }

        internal IEnumerable<MedicineBatchTransfer> Parent(
            IEnumerable<MedicineBatchTransfer> transfers,
            Func<MedicineBatchTransfer, bool> parentMatchCondition)
        {
            return transfers.Where(parentMatchCondition);
        }
    }
}
