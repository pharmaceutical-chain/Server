using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models.Database
{
    public class Transaction
    {
        public uint Id { get; set; }

        public string EthereumTransactionHash { get; set; }

        public Guid ReceiptId { get; set; }
        public Receipt Receipt { get; set; }


    }
}
