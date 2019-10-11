﻿using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace PharmaceuticalChain.API.Models.Database
{
    public class Tenant
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }        
        public string Name { get; set; }
        public string Address { get; set; }
        public string PhoneNumber { get; set; }
        public string TaxCode { get; set; }
        public string BRCLink { get; set; }
        public string GPCLink { get; set; }

        /// <summary>
        /// Transaction hash of the master transaction used to initialize internal contract to create this tenant.
        /// </summary>
        public string TransactionHash { get; set; }
        /// <summary>
        /// Address of the Tenant contract object on the blockchain network.
        /// </summary>
        public string ContractAddress { get; set; }
    }
}