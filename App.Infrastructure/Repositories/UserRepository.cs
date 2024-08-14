using App.Application.Contracts.Infrastructure.Repositories;
using App.Domain.Entities;
using App.Infrastructure.Context;
using App.Infrastructure.Repository;

namespace App.Infrastructure.Repositories
{
    public class UserRepository : BaseRepository<User, Guid>, IUserRepository
    {
        public UserRepository(DataContext context) : base(context)
        {
        }
    }
}
