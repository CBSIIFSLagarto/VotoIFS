using System;
using System.ComponentModel.DataAnnotations;
using Nota2.Util;


namespace Nota2.ModelsView
{
    public class Login
    {
        private String temp = "";


        [Required(ErrorMessage = "O e-mail do usuário é obrigatório", AllowEmptyStrings = false)]
        [RegularExpression(".+\\@.+\\..+", ErrorMessage = "Informe um email válido...")]
        [Display(Name = "E-mail:")]
        [StringLength(100, MinimumLength = 4)]
        public String Email { get; set; }

        [Required(ErrorMessage = "Senha do usuário é obrigatório", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 4)]
        [Display(Name = "Senha:")]
        public String senha
        {
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

    }
}
