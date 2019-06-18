using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models.Database
{
    public class Transaction
    {
        [DatabaseGenerated(DatabaseGeneratedOption.None)]
        public uint Id { get; set; }

        public string EthereumTransactionHash { get; set; }

        public Guid ReceiptId { get; set; }
        public Receipt Receipt { get; set; }

        public string DrugName { get; set; }
        public uint Amount { get; set; }
        public string PackageId { get; set; }
    }
}
