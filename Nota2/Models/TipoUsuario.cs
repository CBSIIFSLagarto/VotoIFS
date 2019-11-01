using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Nota2.Models
{
    public class TipoUsuario
    {
        [Key]
        public int TipoID { get; set; }
        [Required(ErrorMessage = "Descrição obrigatoria", AllowEmptyStrings = false)]
        [StringLength(30, MinimumLength = 3)]
        public String Descricao { get; set; }
        
        public ICollection<Usuario> Usuarios { get; set; } = new HashSet<Usuario>();

    }
}
