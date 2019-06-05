using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models.Ethereum
{
    public class CreateDrugTransactionResult : TransactionResult
    {
        public int TransactionId { get; set; }
    }
}
