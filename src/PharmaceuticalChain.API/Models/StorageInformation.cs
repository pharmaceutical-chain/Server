using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models
{
    public class StorageInformation
    {
        public List<DrugStorageInformation> DrugChainHistories { get; set; }
    }
}
