using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Core_RBS.Models;

namespace Core_RBS.Services 
{
    public interface IVotosService
    {
        Task<List<Voto>> FindAllAsync(int camId, int autoavaliacao, DateTime? minDate, DateTime? maxDate, string userId);
        double GetMediaVotos(List<Voto> votos);
    }
}