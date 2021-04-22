using LocatieService.Database.Datamodels;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatieService.Repositories
{
    public interface ICityRepository : IRepository<City>
    {
        Task<City> GetByIdAsync(Guid id);
        Task<City> GetByNameAsync(string name);
        Task<List<City>> GetAllAsync();
    }
}
