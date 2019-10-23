using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace PharmaceuticalChain.API.Models.Database
{
    public class MedicineBatch
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
        public uint Quantity { get; set; }
        public uint DeclaredPrice { get; set; }
    }
}
