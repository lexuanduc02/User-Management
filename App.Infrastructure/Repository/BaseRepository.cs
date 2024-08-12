using App.Application.Contracts.Infrastructure.Repository;
using App.Domain.Entities.Contracts;
using App.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace App.Infrastructure.Repository
{
    public abstract class BaseRepository<TEntity, TKey> where TEntity : class, IEntity<TKey>, new()
    {
        private DataContext _context;
        private readonly DbSet<TEntity> DbSet;

        public BaseRepository(DataContext context)
        {
            _context = context;
            DbSet = context.Set<TEntity>();
        }

        public IEnumerable<TEntity> GetAll()
        {
            return DbSet.ToList();
        }

        public virtual async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            return await DbSet.ToListAsync();
        }

        public TEntity GetById(TKey id) => DbSet.Find(id);

        public async Task<TEntity> GetByIdAsync(TKey id)
        {
            return await DbSet.FindAsync(id);
        }

        public TEntity Find(Expression<Func<TEntity, bool>> criteria) => DbSet.SingleOrDefault(criteria);

        public TEntity FindInclude(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, object>> orderBy = null, string orderByDirection = OrderBy.Ascending, string[] includes = null)
        {
            IQueryable<TEntity> query = DbSet.Where(criteria);

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);
            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return query.FirstOrDefault();
        }

        public TEntity FindNoTracking(Expression<Func<TEntity, bool>> criteria, string[] includes = null) => DbSet.AsNoTracking().SingleOrDefault(criteria);

        public TEntity FindNoTracking(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, bool>> criteria2, string[] includes = null) => DbSet.Where(criteria).Where(criteria2).AsNoTracking().FirstOrDefault();

        public TEntity FindNoTracking(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, bool>> criteria2, Expression<Func<TEntity, bool>> criteria3, string[] includes = null) => DbSet.Where(criteria).Where(criteria2).Where(criteria3).AsNoTracking().FirstOrDefault();

        public TEntity FindNoTracking(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, object>> orderBy = null) => DbSet.Where(criteria).OrderBy(orderBy).AsNoTracking().FirstOrDefault();

        public TEntity FindNoTracking(Expression<Func<TEntity, bool>> criteria, Expression<Func<TEntity, bool>> criteria2, Expression<Func<TEntity, object>> orderBy = null, string orderByDirection = OrderBy.Ascending, string[] includes = null) => DbSet.Where(criteria).Where(criteria2).OrderBy(orderBy).AsNoTracking().FirstOrDefault();

        public TEntity Find(Expression<Func<TEntity, bool>> criteria, string[] includes = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query.SingleOrDefault(criteria);
        }

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> criteria) => await DbSet.SingleOrDefaultAsync(criteria);

        public async Task<TEntity> FindAsync(Expression<Func<TEntity, bool>> criteria, string[] includes = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (includes != null)
                foreach (var incluse in includes)
                    query = query.Include(incluse);

            return await query.SingleOrDefaultAsync(criteria);
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria) => DbSet.Where(criteria).ToList();

        public IEnumerable<TEntity> FindAlike(Expression<Func<TEntity, bool>> criteria, string[] includes = null)
        {
            return DbSet.Where(criteria).AsEnumerable();
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria, string[] includes = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query.Where(criteria).ToList();
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria, int skip, int take)
        {
            return DbSet.Where(criteria).Skip(skip).Take(take).ToList();
        }

        public IEnumerable<TEntity> FindAll(Expression<Func<TEntity, bool>> criteria, int? skip, int? take, Expression<Func<TEntity, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<TEntity> query = DbSet.Where(criteria);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (take.HasValue)
                query = query.Take(take.Value);

            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return query.ToList();
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria) => await _context.Set<TEntity>().Where(criteria).ToListAsync();

        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, string[] includes = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return await query.Where(criteria).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, int take, int skip)
        {
            return await DbSet.Where(criteria).Skip(skip).Take(take).ToListAsync();
        }

        public async Task<IEnumerable<TEntity>> FindAllAsync(Expression<Func<TEntity, bool>> criteria, int? take, int? skip, Expression<Func<TEntity, object>> orderBy = null, string orderByDirection = OrderBy.Ascending)
        {
            IQueryable<TEntity> query = DbSet.Where(criteria);

            if (take.HasValue)
                query = query.Take(take.Value);

            if (skip.HasValue)
                query = query.Skip(skip.Value);

            if (orderBy != null)
            {
                if (orderByDirection == OrderBy.Ascending)
                    query = query.OrderBy(orderBy);
                else
                    query = query.OrderByDescending(orderBy);
            }

            return await query.ToListAsync();
        }

        public IEnumerable<TEntity> FindAllInclude(string[] includes = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query.ToList();
        }

        public IEnumerable<TEntity> FindAllInclude(Expression<Func<TEntity, object>> orderBy, int skip, int take, string[] includes = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query.OrderByDescending(orderBy).Skip(skip).Take(take).ToList();
        }

        public IEnumerable<TEntity> FindAllInclude(int skip, int take, string[] includes = null)
        {
            IQueryable<TEntity> query = DbSet;

            if (includes != null)
                foreach (var include in includes)
                    query = query.Include(include);

            return query.Skip(skip).Take(take).ToList();
        }

        public virtual IQueryable<TEntity> GetAllNolist()
        {
            return DbSet.AsNoTracking();
        }

        public TEntity Add(TEntity entity)
        {
            DbSet.Add(entity);
            return entity;
        }

        public async Task<TEntity> AddAsync(TEntity entity)
        {
            await DbSet.AddAsync(entity);
            return entity;
        }

        public IEnumerable<TEntity> AddRange(IEnumerable<TEntity> entities)
        {
            DbSet.AddRange(entities);
            return entities;
        }

        public async Task<IEnumerable<TEntity>> AddRangeAsync(IEnumerable<TEntity> entities)
        {
            await DbSet.AddRangeAsync(entities);
            return entities;
        }

        public TEntity Update(TEntity entity)
        {
            _context.Update(entity);
            return entity;
        }

        public IEnumerable<TEntity> UpdateRange(IEnumerable<TEntity> entities)
        {
            _context.UpdateRange(entities);
            return entities;
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }

        public void DeleteRange(IEnumerable<TEntity> entities)
        {
            DbSet.RemoveRange(entities);
        }

        public void Attach(TEntity entity)
        {
            DbSet.Attach(entity);
        }

        public void AttachRange(IEnumerable<TEntity> entities)
        {
            DbSet.AttachRange(entities);
        }

        public int Count()
        {
            return DbSet.Count();
        }

        public int Count(Expression<Func<TEntity, bool>> criteria)
        {
            return DbSet.Count(criteria);
        }

        public async Task<int> CountAsync()
        {
            return await DbSet.CountAsync();
        }

        public async Task<int> CountAsync(Expression<Func<TEntity, bool>> criteria)
        {
            return await DbSet.CountAsync(criteria);
        }

        public async Task<IEnumerable<TEntity>> GetChildsAsync(Expression<Func<TEntity, bool>> include, Expression<Func<TEntity, bool>> criteria)
        {
            return await DbSet.Include(include).Where(criteria).ToListAsync();
        }

        public bool isExist(Expression<Func<TEntity, bool>> criteria)
        {
            return DbSet.AsNoTracking().Any(criteria);
        }

        public void Commit()
        {
            _context.Database.CommitTransaction();
        }

        public void RollBack()
        {
            _context.Database.RollbackTransaction();
        }

        public IQueryable<TEntity> GetTableAsTracking()
        {
            return DbSet.AsQueryable();
        }

        public IQueryable<TEntity> GetTableNoTracking()
        {
            return DbSet.AsNoTracking().AsQueryable();
        }

        public async Task<bool> IsExistAsync(Expression<Func<TEntity, bool>> predicate)
        {
            return await DbSet.AsNoTracking().AnyAsync(predicate);
        }
    }
}
