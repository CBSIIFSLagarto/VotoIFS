using Core_RBS.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace Core_RBS.ModelsView
{
    public class RelatorioViewModel
    {
        public string AutoAvaliacaoChecked { get; set; }
        public DateTime? DataInicio { get; set; }
        public DateTime? DataFim { get; set; }
        public ICollection<Campanha> Campanhas { get; set; }
        public ICollection<Campanha> ListCampanhas { get; set; }
    }
}