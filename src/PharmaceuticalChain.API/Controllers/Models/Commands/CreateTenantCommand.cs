﻿using PharmaceuticalChain.API.Models.Database;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Controllers.Models.Commands
{
    public class CreateTenantCommand
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string PrimaryAddress { get; set; }
        public string PhoneNumber { get; set; }
        public string TaxCode { get; set; }
        public string RegistrationCode { get; set; }
        public string Certificates { get; set; }
        public string Type { get; set; }
    }
}
