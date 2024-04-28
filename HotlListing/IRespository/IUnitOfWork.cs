using HotlListing.Models;

namespace HotlListing.IRespository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Country> Countries { get; }
        IGenericRepository<Hotel> Hotles { get; }
        Task Save();
    }
}
