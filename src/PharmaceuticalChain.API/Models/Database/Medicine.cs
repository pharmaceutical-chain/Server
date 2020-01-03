using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models.Database
{
    [Newtonsoft.Json.JsonObject(IsReference = true)]
    public class Medicine : BlockchainObject
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public Guid Id { get; set; }
        public string CommercialName { get; set; }
        public string RegistrationCode { get; set; }
        public string BatchNumber { get; set; }
        public bool IsPrescriptionMedicine { get; set; }
        public string DosageForm { get; set; }
        public string IngredientConcentration { get; set; }
        public string PackingSpecification { get; set; }
        public uint DeclaredPrice { get; set; }
        public string Certificates { get; set; }

        public Guid SubmittedTenantId { get; set; }
        public Tenant SubmittedTenant { get; set; }

        public ICollection<MedicineBatch> MedicineBatches { get; set; }

        public bool IsApprovedByAdmin { get; set; }
    }
}
