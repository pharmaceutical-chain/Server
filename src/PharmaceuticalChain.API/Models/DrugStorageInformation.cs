using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models
{
    public class DrugStorageInformation
    {
        public DrugStorageInformation(string drugName, string packageId, List<ChainHistory> chainHistories)
        {
            DrugName = drugName;
            PackageId = packageId;
            ChainHistories = chainHistories;
        }
        public string DrugName { get; set; }
        public string PackageId { get; set; }
        public List<ChainHistory> ChainHistories { get; set; }
    }
}
