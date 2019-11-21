using Core_RBS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_RBS.ModelsView
{
    public class HomeViewModel
    {
        public ICollection<Campanha> Campanhas { get; set; }
    }
}
