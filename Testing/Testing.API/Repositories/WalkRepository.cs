using Microsoft.EntityFrameworkCore;
using System.Text.RegularExpressions;
using Testing.API.Data;
using Testing.API.Models.Domain;

namespace Testing.API.Repositories
{
    public class WalkRepository : IWalkRepository
    {
        private readonly TestingDbContext testingDbContext;

        public WalkRepository(TestingDbContext testingDbContext)
        {
            this.testingDbContext = testingDbContext;
        }

        public async Task<Walk> AddAsync(Walk walk)
        {
            //Assign New ID
            walk.Id = Guid.NewGuid();
            await testingDbContext.Walks.AddAsync(walk);
            await testingDbContext.SaveChangesAsync();

            return walk;
        }

        public async  Task<Walk> DeleteAsync(Guid id)
        {
            var existingWalk = await testingDbContext.Walks.FindAsync(id);

            if (existingWalk == null)
            {
                return null;
            }
            testingDbContext.Walks.Remove(existingWalk);
            await testingDbContext.SaveChangesAsync();

            return existingWalk;

        }

        public async Task<IEnumerable<Walk>> GetAllAsync()
        {
           return await 
                testingDbContext.Walks
                .Include(x => x.Region)
                .Include(x => x.WalkDifficulty)
                .ToListAsync();
        }

        public Task<Walk> GetAsync(Guid id)
        {
            return  testingDbContext.Walks
                 .Include(x => x.Region)
                 .Include(x => x.WalkDifficulty)
                 .FirstOrDefaultAsync( x => x.Id == id);
        }

        public async Task<Walk> UpdateAsync(Guid id, Walk walk)
        {
            var existingWalk = await testingDbContext.Walks.FindAsync(id);

            if (existingWalk != null)
            {
                existingWalk.Length = walk.Length;
                existingWalk.Name = walk.Name;
                existingWalk.WalkDifficultyId = walk.WalkDifficultyId;
                existingWalk.RegionId = walk.RegionId;

                await testingDbContext.SaveChangesAsync();
                return existingWalk;
            }

            return null;
        }
    }
}
