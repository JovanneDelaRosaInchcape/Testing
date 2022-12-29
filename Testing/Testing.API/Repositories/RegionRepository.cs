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

      

        public IEnumerable<Region> GetAll()
        {
            return testingDbContext.Region.ToList();
        }
    }
}
