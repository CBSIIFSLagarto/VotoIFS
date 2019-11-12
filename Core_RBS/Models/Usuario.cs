using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace Core_RBS.Models
{
    public class Usuario : IdentityUser
    {
        [Required(ErrorMessage = "Nome do usuario é obrigatorio.")]
        [MinLength(6,ErrorMessage ="Nome inválido")]
        public string Nome { get; set; }
    }
}