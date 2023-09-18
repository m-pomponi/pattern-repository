using GenericRepository.DapperInterfaces;
using GenericRepository.Repository.Interfaces;
using Microsoft.EntityFrameworkCore.Storage;

namespace GenericRepository.UnitOfWork
{
    public interface IUnitOfWork : IAsyncDisposable, IDapperRepository
    {
        Task CommitAsync();
        Task CancelAsync();
        void Commit();
        void Cancel();
        Task<IDbContextTransaction> CreateTransaction(CancellationToken cancellationToken = default);
    }
}
