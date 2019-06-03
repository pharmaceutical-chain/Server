using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models
{
    public class DrugStorageInformation
    {
        public string DrugName { get; set; }

        public List<ChainHistory> ChainHistories { get; set; }
    }
}
