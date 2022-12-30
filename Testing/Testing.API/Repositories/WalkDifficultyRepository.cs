using Microsoft.EntityFrameworkCore;
using Testing.API.Data;
using Testing.API.Models.Domain;

namespace Testing.API.Repositories
{
    public class WalkDifficultyRepository : IWalkDifficultyRepository
    {
        private readonly TestingDbContext testingDbContext;

       
        public WalkDifficultyRepository(TestingDbContext testingDbContext)
        {
            this.testingDbContext = testingDbContext;
        }

        public  async Task<WalkDifficulty> AddAsync(WalkDifficulty walkDifficulty)
        {
            walkDifficulty.Id = Guid.NewGuid();
            await testingDbContext.WalkDifficulty.AddAsync(walkDifficulty);
            await testingDbContext.SaveChangesAsync();

            return walkDifficulty;
        }

        public async Task<WalkDifficulty> DeleteAsync(Guid id)
        {
            var existingWalkDifficulty = await testingDbContext.WalkDifficulty.FindAsync(id);

            if  (existingWalkDifficulty != null)
            {
                testingDbContext.WalkDifficulty.Remove(existingWalkDifficulty);
                await testingDbContext.SaveChangesAsync();
                return existingWalkDifficulty;
            }
            return null;
        }

        public async Task<IEnumerable<WalkDifficulty>> GetAllAsync()
        {
            return await testingDbContext.WalkDifficulty.ToListAsync();
        }


        public async Task<WalkDifficulty> GetAsync(Guid id)
        {
            return await testingDbContext.WalkDifficulty.FirstOrDefaultAsync(x => x.Id == id);
        }

        public async Task<WalkDifficulty> UpdateAsync(Guid id, WalkDifficulty walkDifficulty)
        {
            var existingWalkDifficulty = await testingDbContext.WalkDifficulty.FindAsync(id);

            if(existingWalkDifficulty == null)
            {
                return null;
            }

            existingWalkDifficulty.Code = walkDifficulty.Code;
            await testingDbContext.SaveChangesAsync();
            return existingWalkDifficulty;
        }
    }
}
