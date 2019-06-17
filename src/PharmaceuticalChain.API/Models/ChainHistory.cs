using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models
{
    public class ChainHistory
    {
        
        public ChainHistory(uint companyId, uint amount)
        {
            CompanyId = companyId;
            AmountTransferedToNextCompany = amount;
        }
        public uint CompanyId { get; set; }
        public string CompanyName { get; set; }

        public uint AmountTransferedToNextCompany { get; set; }
    }
}
