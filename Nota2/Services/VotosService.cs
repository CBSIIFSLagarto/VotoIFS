using Microsoft.EntityFrameworkCore;
using Nota2.Data;
using Nota2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nota2.Services
{
    public class VotosService
    {
        private readonly MyContext _context;

        public VotosService(MyContext context)
        {
            _context = context;
        }

        public async Task<List<Voto>> FindAllAsync(int camId, int autoavaliacao, DateTime? minDate, DateTime? maxDate, int userId)
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
            return await result.Include(x => x.Campanha).Where(x => x.Campanha.UseId == userId).OrderBy(x => x.Campanha.Descricao).ToListAsync();
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
