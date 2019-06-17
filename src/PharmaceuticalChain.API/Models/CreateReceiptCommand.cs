using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models
{
    public class CreateReceiptCommand
    {
        public int companyId { get; set; }
        public int toCompanyId { get; set; }
    }
}
