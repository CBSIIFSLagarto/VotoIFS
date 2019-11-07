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

        public async Task<List<Voto>> FindAllAsync(int? id, DateTime? minDate, DateTime? maxDate)
        {
            var result = from obj in _context.Votos select obj;
            if (id.HasValue)
            {
                result = result.Where(x => x.CamId == id.Value);
            }
            if (minDate.HasValue)
            {
                result = result.Where(x => x.DataVoto >= minDate.Value);
            }
            if (maxDate.HasValue)
            {
                result = result.Where(x => x.DataVoto <= maxDate.Value);
            }
            return await result.Include(x => x.Campanha).OrderByDescending(x => x.DataVoto).ToListAsync();
        }
    }
}
