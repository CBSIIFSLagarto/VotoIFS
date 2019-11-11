using Nota2.Models;
using System.Collections.Generic;

namespace Nota2.ModelsView
{
    public class VotoFormViewModel
    {
        public Voto Voto { get; set; }
        public Campanha Campanha { get; set; }

        public string Error { get; set; }
        public ICollection<Voto> Votos { get; set; }
        public ICollection<Campanha> Campanhas { get; set; }
        public double MediaVotos { get; set; }
    }
}