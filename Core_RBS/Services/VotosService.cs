using Core_RBS.Data;
using Core_RBS.Models;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Core_RBS.Services
{
    public class VotosService
    {
        private readonly ApplicationDbContext _context;

        public VotosService(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<List<Voto>> FindAllAsync(int camId, int autoavaliacao, DateTime? minDate, DateTime? maxDate, string userId)
        {
            var result = from obj in _context.Votos select obj;
            if (camId > 0)
            {
                result = result.Where(x => x.CamId == camId);
            }
            if (autoavaliacao > 0)
            {
                result = result.Include(x => x.Campanha).Where(x => x.Campanha.AutoAvaliacao == true);
            }
            if (minDate.HasValue)
            {
                result = result.Where(x => x.DataVoto >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.DataVoto <= maxDate.Value);
            }
            return await result.Include(x => x.Campanha).Include(x => x.Campanha.Usuario) .Where(x => x.Campanha.Usuario.Id == userId).OrderByDescending(x => x.DataVoto).ToListAsync();
        }

        public double GetMediaVotos(List<Voto> votos)
        {
            var soma = 0.0;
            var qtd = 0.0;
            foreach(var item in votos)
            {
                soma += item.Nota;
                qtd++;
            }
            return soma/qtd;
        }
    }
}
