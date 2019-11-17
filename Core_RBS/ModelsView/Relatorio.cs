using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Core_RBS.Models;

namespace Core_RBS.ModelsViews
{
    public class Relatorio : IValidatableObject
    {
        [Display(Name = "Data Inicio:")]
        [Required(ErrorMessage = "Data do inicio é obrigatória", AllowEmptyStrings = false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime DataInicio { get; set; }

        [Display(Name = "Data Fim:")]
        [Required(ErrorMessage = "Data do fim é obrigatória", AllowEmptyStrings = false)]
        [DisplayFormat(ApplyFormatInEditMode = true, DataFormatString = "{0:dd/MM/yyyy}")]
        [DataType(DataType.Date, ErrorMessage = "Data em formato inválido")]
        public DateTime DataFim { get; set; }
        public ICollection<Campanha> Campanha { get; set; }

        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.DataInicio > this.DataFim)
            {
                yield return
                  new ValidationResult(errorMessage: "Data final menor que data inicial",
                                       memberNames: new[] { "DataInicio" });
            }
        }
    }
}
