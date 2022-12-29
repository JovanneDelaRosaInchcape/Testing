using Testing.API.Models.Domain;

namespace Testing.API.Repositories
{
    public interface IRegionRepository
    {
       IEnumerable<Region> GetAll();

    }
}
