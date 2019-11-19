using Core_RBS.Data;
using Core_RBS.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.EntityFrameworkCore;
using System.Threading.Tasks;

namespace Core_RBS.Services
{
    public class CampanhaService : ICampanhaService
    {
        private readonly ApplicationDbContext _context;

        public CampanhaService(ApplicationDbContext context)
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
        public List<Campanha> FindAllUser(string userId)
        {
            return _context.Campanhas.Include(c => c.Usuario).Where(obj => obj.Usuario.Id == userId).OrderBy(x => x.Descricao).ToList();
        }
    }
}
