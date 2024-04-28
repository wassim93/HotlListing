using HotlListing.IRespository;
using HotlListing.Models;

namespace HotlListing.Repository
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly DatabaseContext _dbContext;
        public UnitOfWork(DatabaseContext context)
        {
            _dbContext = context;
        }
        private IGenericRepository<Country> countries;

        private IGenericRepository<Hotel> hotles;

        public IGenericRepository<Country> Countries => countries ??= new GenericRepository<Country>(_dbContext);

        public IGenericRepository<Hotel> Hotles => hotles ??= new GenericRepository<Hotel>(_dbContext);

        public void Dispose()
        {
            _dbContext.Dispose();
            GC.SuppressFinalize(this);
        }

        public async Task Save()
        {
            await _dbContext.SaveChangesAsync();
        }
    }
}
