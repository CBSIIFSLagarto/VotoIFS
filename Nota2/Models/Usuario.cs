using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Nota2.Util;

namespace Nota2.Models
{
    public class Usuario
    {

        private String temp = "";

        [Key]
        public int UseID { get; set; }

        [Required(ErrorMessage = "O nome do usuário é obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Nome do Usuário:")]
        [StringLength(255, MinimumLength = 4)]
        public String Nome { get; set; }

        [Required(ErrorMessage = "O e-mail do usuário é obrigatório", AllowEmptyStrings = false)]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um email válido...")]
        [Display(Name = "E-mail:")]
        [StringLength(100, MinimumLength = 4)]
        public String Email { get; set; }

        [Required(ErrorMessage = "Senha do usuário é obrigatório", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 4)]
        [Display(Name = "Senha:")]     
        public String senha {             
            get 
            {
                return this.temp;
            }
            set
            {
                if (value.Length > 1)
                {
                    this.temp = SHA.GenerateSHA256String(value);
                }
                else
                {
                    this.temp = value;
                }               
            }
        }
        [Required(ErrorMessage = "O tipo do usuário é obrigatório", AllowEmptyStrings = false)]
        [Display(Name = "Tipo Usuário:")]
        public int TipoId { get; set; }
        public TipoUsuario TipoUsuario { get; set; }
        public ICollection<Campanha> Campanhas { get; set; } = new HashSet<Campanha>();
    }
}
