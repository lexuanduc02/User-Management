using App.Domain.Entities.Contracts;
using System.Linq.Expressions;

namespace App.Application.Contracts.Infrastructure.Repository
{
    public interface IBaseRepository<TEntity, in TKey> where TEntity : IEntity<TKey>
    {
        TEntity GetById(TKey id);
        Task<TEntity> GetByIdAsync(TKey id);
        IEnumerable<TEntity> GetAll();
        Task<IEnumerable<TEntity>> GetAllAsync();
        TEntity Find(Expression<Func<TEntity, bool>> criteria, string[] includes = null);
        TEntity FindInclude(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, object>> orderBy = null, string orderByDirection = OrderBy.Ascending, string[] includes = null);
        Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> criteria, string[] includes = null);
        TEntity FindNoTracking(Expression<Func<TEntity, bool>> criteria, string[] includes = null);
        TEntity FindNoTracking(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, bool>> criteria2, string[] includes = null);
        TEntity FindNoTracking(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, bool>> criteria2, Expression<Func<TEntity, bool>> criteria3, string[] includes = null);
        TEntity FindNoTracking(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, object>> orderBy = null);
        TEntity FindNoTracking(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, bool>> criteria2,
            Expression<Func<TEntity, object>> orderBy = null, string orderByDirection = OrderBy.Ascending, string[] includes = null);
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria, string[] includes = null);
        IEnumerable<TEntity> FindAlike(Expression<Func<TEntity, bool>> criteria, string[] includes = null);

        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria, int take, int skip);
        IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria, int? take, int? skip,
            Expression<Func<TEntity, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, string[] includes = null);
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, int skip, int take);
        Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, int? skip, int? take,
            Expression<Func<TEntity, object>> orderBy = null, string orderByDirection = OrderBy.Ascending);
        IEnumerable<TEntity> FindAllInclude(string[] includes = null);
        IEnumerable<TEntity> FindAllInclude(int skip, int take, string[] includes = null);
        IEnumerable<TEntity> FindAllInclude(Expression<Func<TEntity, object>> orderBy, int skip, int take, string[] includes = null);
        IQueryable<TEntity> GetAllNolist();
        TEntity Add(TEntity entity);
        Task<TEntity> AddAsync(TEntity entity);
        IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities);
        Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities);
        TEntity Update(TEntity entity);
        IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities);
        void Delete(TEntity entity);
        void DeleteRange(IEnumerable<TEntity> entities);
        void Attach(TEntity entity);
        void AttachRange(IEnumerable<TEntity> entities);
        int Count();
        int Count(Expression<Func<TEntity, bool>> criteria);
        Task<int> CountAsync();
        Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria);
        Task<IEnumerable<TEntity>> GetChildsAsync(Expression<Func<TEntity, bool>> include, Expression<Func<TEntity, bool>> criteria);
        bool isExist(Expression<Func<TEntity, bool>> criteria);
        void Commit();
        void RollBack();
        IQueryable<TEntity> GetTableNoTracking();
        IQueryable<TEntity> GetTableAsTracking();
        Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate);
    }

    public static class OrderBy
    {
        public const string Ascending = "ASC";
        public const string Descending = "DESC";
    }
}
