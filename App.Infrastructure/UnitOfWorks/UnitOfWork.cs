using App.Application.Contracts.Infrastructure.Repositories;
using App.Application.Contracts.Infrastructure.UnitOfWork;
using App.Infrastructure.Context;
using App.Infrastructure.Repositories;
using System.Data.Common;

namespace App.Infrastructure.UnitOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        public readonly DataContext _context;

        public UnitOfWork(DataContext context)
        {
            _context = context;
            UserRepository = new UserRepository(context);
        }

        public IUserRepository UserRepository { get; }

        public Task BeginTransactionAsync()
        {
            return Task.CompletedTask;
        }

        public Task CancelAsync()
        {
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
