using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Nota2.Models
{
    public class Campanha : IValidatableObject
    {
        private static Random random = new Random();
        private string chave = "";

        [Key]
        public int CamID { get; set; }

        [Display(Name = "Chave de acesso:")]
        //[Required(ErrorMessage = "Chave de acesso é obrigatória", AllowEmptyStrings = false)]
        [StringLength(20, MinimumLength = 4)]
        public String Chave {
            get
            {
                return this.chave;
            }
            set
            {
                if (value == null) {
                    int tamanho = 5;
                    const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
                    this.chave = new string(Enumerable.Repeat(chars, tamanho).Select(s => s[random.Next(s.Length)]).ToArray());                
                }
                else
                {
                    this.chave = value;
                }
                
            }
        }

        [Display(Name = "Descrição:")]
        [Required(ErrorMessage = "Descrição é obrigatória", AllowEmptyStrings = false)]
        [StringLength(255, MinimumLength = 4)]
        public String Descricao { get; set; }

        [Display(Name = "Data/Hora Inicio:")]
        [Required(ErrorMessage = "Data/Hora do inicio é obrigatória", AllowEmptyStrings = false)]
        public DateTime DataHoraInicio { get; set; }

        [Display(Name = "Data/Hora Fim:")]
        [Required(ErrorMessage = "Data/Hora do fim é obrigatória", AllowEmptyStrings = false)]                
        public DateTime DataHoraFim { get; set; }

        [DefaultValue(true)]
        [Display(Name = "Autoavaliação:")]
        public bool AutoAvaliacao { get; set; }
        public int UseId { get; set; }
        public Usuario Usuario { get; set; }
        public ICollection<Voto> Votos { get; set; } = new HashSet<Voto>();


        public IEnumerable<ValidationResult> Validate(ValidationContext validationContext)
        {
            if (this.DataHoraInicio > this.DataHoraFim)
            {
                yield return
                  new ValidationResult(errorMessage: "Data final menor que data inicial",
                                       memberNames: new[] { "DataHoraFim" });
            }
        }

    }
}
