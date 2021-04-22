using LocatieService.Database.Contexts;
using LocatieService.Database.Datamodels;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace LocatieService.Repositories
{
    public class CityRepository : Repository<City>, ICityRepository
    {
        public CityRepository(LocatieContext locatieContext) : base(locatieContext) { }

        public async Task<City> GetByIdAsync(Guid id)
        {
            return await GetAll().FirstOrDefaultAsync(e => e.Id == id);
        }

        public async Task<City> GetByNameAsync(string name)
        {
            return await GetAll().FirstOrDefaultAsync(e => e.Name == name);
        }

        public async Task<List<City>> GetAllAsync()
        {
            return await GetAll().ToListAsync();
        }
    }
}
