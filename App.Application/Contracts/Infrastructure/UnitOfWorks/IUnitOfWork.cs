﻿using App.Application.Contracts.Infrastructure.Repositories;
using System.Data.Common;

namespace App.Application.Contracts.Infrastructure.UnitOfWork
{
    public interface IUnitOfWork
    {
        public DbConnection DbConnection();
        IUserRepository UserRepository { get; }
        Task BeginTransactionAsync();
        Task SaveChangesAsync();
        Task CancelAsync(); // this method should be called ASAP before leaving, a ITransactionUnitOfWork implementation should implement IDisposable to handle tha
    }
}
