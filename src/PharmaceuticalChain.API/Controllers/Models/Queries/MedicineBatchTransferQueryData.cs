﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Controllers.Models.Queries
{
    public class MedicineBatchTransferQueryData
    {
        public Guid Id { get; set; }

        public MedicineBatchQueryData MedicineBatch { get; set; }

        public TenantQueryData From { get; set; }
        public TenantQueryData To { get; set; }

        public uint Quantity { get; set; }
    }
}
