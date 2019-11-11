using Nota2.Data;
using Nota2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nota2.Services
{
    public class CampanhaService
    {
        private readonly MyContext _context;

        public CampanhaService(MyContext context)
        {
            _context = context;
        }

        public List<Campanha> FindAll()
        {
            return _context.Campanhas.OrderBy(x => x.Descricao).ToList();
        }
        public List<Campanha> FindAll(int camId)
        {
            return _context.Campanhas.OrderBy(x => x.Descricao).Where(obj => obj.CamID == camId).ToList();
        }
        public List<Campanha> FindAllUser(int userId)
        {
            return _context.Campanhas.OrderBy(x => x.Descricao).Where(obj => obj.UseId == userId).ToList();
        }
    }
}
