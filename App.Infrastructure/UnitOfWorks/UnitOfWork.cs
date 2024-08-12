using App.Application.Contracts.Infrastructure.UnitOfWork;
using App.Infrastructure.Context;
using System.Data.Common;

namespace App.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;
        }

        public Task BeginTransactionAsync()
        {
            // we can use context.Database.BeginTransaction(), but since this is an UoW, we just silently discard changes if SaveChangesAsync is not called

            return Task.CompletedTask;
        }

        public Task CancelAsync()
        {
            // we can use transaction.Commit(), but since this is an UoW, we just silently discard changes if SaveChangesAsync is not called

            return Task.CompletedTask;
        }

        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        public DbConnection DbConnection()
        {
            return _context.GetDbConnect();
        }
    }
}
