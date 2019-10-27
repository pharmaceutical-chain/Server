using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models.Database
{
    public class BlockchainObject
    {
        /// <summary>
        /// Transaction hash of the master transaction used to initialize internal contract to create this tenant.
        /// </summary>
        public string TransactionHash { get; set; }
        /// <summary>
        /// Address of the Tenant contract object on the blockchain network.
        /// </summary>
        public string ContractAddress { get; set; }

        public DateTime DateCreated { get; set; }

        public TransactionStatuses TransactionStatus { get; set; } = TransactionStatuses.Pending;
    }
}
