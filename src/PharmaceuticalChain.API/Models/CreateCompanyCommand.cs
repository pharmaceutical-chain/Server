using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models
{
    public class CreateCompanyCommand
    {
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string TaxCode { get; set; }
        public string BRCLink { get; set; }
        public string GPCLink { get; set; }
    }
}
