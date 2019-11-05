using Nota2.Models;
using System.Collections.Generic;

namespace Nota2.ModelsView
{
    public class VotoFormViewModel
    {
        public Voto Voto { get; set; }
        public Campanha Campanha { get; set; }

        public string Error { get; set; }
    }
}