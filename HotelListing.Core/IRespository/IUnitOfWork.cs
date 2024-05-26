using HotelListing.Data;

namespace HotelListing.Core.IRespository
{
    public interface IUnitOfWork : IDisposable
    {
        IGenericRepository<Country> Countries { get; }
        IGenericRepository<Hotel> Hotles { get; }
        Task Save();
    }
}
