﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models.Database
{
    public class Receipt
    {
        public Guid Id { get; set; }

        public ICollection<Transaction> Transactions { get; set; }
    }
}