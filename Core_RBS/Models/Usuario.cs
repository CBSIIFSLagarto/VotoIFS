using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Core_RBS.Models
{
    public class Usuario : IdentityUser
    {


        [NotMapped]
        public  bool IsAdmin { get; set; }

        [Required(ErrorMessage = "Nome do usuario é obrigatorio.")]
        [MinLength(6, ErrorMessage = "Nome inválido")]
        public string Nome { get; set; }
        
    }
}