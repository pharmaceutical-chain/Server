using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models.Database
{
    public class Receipt
    {
        public Guid Id { get; set; }

        public int CompanyId { get; set; }
        public Company Company { get; set; }
        

        public ICollection<Transaction> Transactions { get; set; }
    }
}
