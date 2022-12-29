using Microsoft.EntityFrameworkCore;
using Testing.API.Data;
using Testing.API.Models.Domain;

namespace Testing.API.Repositories
{
    public class RegionRepository : IRegionRepository
    {
        private readonly TestingDbContext testingDbContext;

        public RegionRepository(TestingDbContext testingDbContext)
        {
        
            this.testingDbContext = testingDbContext;
        }

        public async Task<Region> AddAsync(Region region)
        {
            region.Id = Guid.NewGuid();
            await testingDbContext.AddAsync(region);
            await testingDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<Region> DeleteAsync(Guid id)
        {
            var region = await testingDbContext.Region.FirstOrDefaultAsync(x => x.Id == id);
            if(region == null)
            {
                return null;
            }

            //delete the region
            testingDbContext.Region.Remove(region);
           await testingDbContext.SaveChangesAsync();
            return region;
        }

        public async Task<IEnumerable<Region>> GetAllAsync()
        {
            return await testingDbContext.Region.ToListAsync();
        }

        public async Task<Region> GetAsync(Guid id)
        {
            return await testingDbContext.Region.FirstOrDefaultAsync(x => x.Id == id);
            
        }

        public async Task<Region> UpdateAsync(Guid id, Region region)
        {
            var existingRegion = await testingDbContext.Region.FirstOrDefaultAsync(x => x.Id == id);

            if (existingRegion == null)
            {
                return null;
            }

            existingRegion.Code = region.Code;
            existingRegion.Name = region.Name;
            existingRegion.Area = region.Area;
            existingRegion.Lat = region.Lat;
            existingRegion.Long = region.Long;
            existingRegion.Population = region.Population;

           await  testingDbContext.SaveChangesAsync();

            return existingRegion;

        }
    }
}
