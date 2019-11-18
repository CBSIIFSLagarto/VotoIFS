using System.Collections.Generic;
using Core_RBS.Models;

namespace Core_RBS.Services 
{
    public interface ICampanhaService
    {
        List<Campanha> FindAll();
        List<Campanha> FindAll(int camId);
        List<Campanha> FindAllUser(string userId);
    }
}