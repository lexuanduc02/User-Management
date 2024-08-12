using App.Application.Contracts.Infrastructure.Repository;
using App.Domain.Entities;

namespace App.Application.Contracts.Infrastructure.Repositories
{
    public interface IUserRepository : IBaseRepository<User, Guid>
    {
    }
}
