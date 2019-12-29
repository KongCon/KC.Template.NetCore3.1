using KC.Template.IRepository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace KC.Template.Repository
{
    /// <summary>
    /// EF实现，Repository基类
    /// </summary>
    public class DefaultRepositoryImpl<TDbContext>
        where TDbContext : DbContext
    {
        protected TDbContext _context;

        public DefaultRepositoryImpl(TDbContext context)
        {
            this._context = context;
        }
    }

    public class DefaultRepositoryImpl<T, TDbContext> : DefaultRepositoryImpl<TDbContext>,  IBaseRepository<T>
        where T : class
        where TDbContext : DbContext
    {
        public DefaultRepositoryImpl(TDbContext dbContext)
            : base(dbContext)
        {
        }

        /// <summary>
        /// Gets a table
        /// </summary>
        public virtual IQueryable<T> Table => Entities;

        /// <summary>
        /// Gets a table with "no tracking" enabled (EF feature) Use it only when you load record(s) only for read-only operations
        /// </summary>
        public virtual IQueryable<T> TableNoTracking => Entities.AsNoTracking();

        private DbSet<T> _entities;

        protected virtual DbSet<T> Entities
        {
            get
            {
                if (_entities == null)
                    _entities = _context.Set<T>();

                return _entities;
            }
        }

        protected string GetFullErrorTextAndRollbackEntityChanges(DbUpdateException exception)
        {
            //rollback entity changes
            if (_context is DbContext dbContext)
            {
                var entries = dbContext.ChangeTracker.Entries()
                    .Where(e => e.State == EntityState.Added || e.State == EntityState.Modified).ToList();

                entries.ForEach(entry =>
                {
                    try
                    {
                        entry.State = EntityState.Unchanged;
                    }
                    catch (InvalidOperationException)
                    {
                        // ignored
                    }
                });
            }

            _context.SaveChanges();
            return exception.ToString();
        }

        #region 通用方法
        public T GetById(object id)
        {
            return Entities.Find(id);
        }

        /// <summary>
        /// Delete entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Delete(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Remove(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Delete entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void DeleteList(IEnumerable<T> entityList)
        {
            if (entityList == null)
                throw new ArgumentNullException(nameof(entityList));

            try
            {
                Entities.RemoveRange(entityList);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Update entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Update(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Update(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Update entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void UpdateList(IEnumerable<T> entityList)
        {
            if (entityList == null)
                throw new ArgumentNullException(nameof(entityList));

            try
            {
                Entities.UpdateRange(entityList);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Insert entity
        /// </summary>
        /// <param name="entity">Entity</param>
        public virtual void Save(T entity)
        {
            if (entity == null)
                throw new ArgumentNullException(nameof(entity));

            try
            {
                Entities.Add(entity);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        /// <summary>
        /// Insert entities
        /// </summary>
        /// <param name="entities">Entities</param>
        public virtual void SaveList(IEnumerable<T> entityList)
        {
            if (entityList == null)
                throw new ArgumentNullException(nameof(entityList));

            try
            {
                Entities.AddRange(entityList);
                _context.SaveChanges();
            }
            catch (DbUpdateException exception)
            {
                //ensure that the detailed error text is saved in the Log
                throw new Exception(GetFullErrorTextAndRollbackEntityChanges(exception), exception);
            }
        }

        public virtual T Find(Expression<Func<T, bool>> filter)
        {
            return Entities.Where(filter).FirstOrDefault();
        }

        public virtual int Count(Expression<Func<T, bool>> filter)
        {
            return Entities.Where(filter).Count();
        }

        public virtual IList<T> GetAll()
        {
            return Entities.ToList();
        }

        public virtual IList<T> GetList(Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, bool isAsc = true, IEnumerable<string> includeList = null)
        {
            IQueryable<T> query = this.Entities;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeList != null)
            {
                foreach (var item in includeList)
                {
                    query = query.Include(item);
                }
            }
            if (orderBy != null)
            {
                if (isAsc)
                {
                    query = query.OrderBy(orderBy);
                }
                else
                {
                    query = query.OrderByDescending(orderBy);
                }
            }
            return query.ToList();
        }

        public virtual IList<T> GetTopList(int pageSize, Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, bool isAsc = true, IEnumerable<Expression<Func<T, object>>> includeList = null)
        {
            IQueryable<T> query = this.Entities;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeList != null)
            {
                foreach (var item in includeList)
                {
                    query = query.Include(item);
                }
            }
            if (orderBy != null)
            {
                if (isAsc)
                {
                    query = query.OrderBy(orderBy);
                }
                else
                {
                    query = query.OrderByDescending(orderBy);
                }
            }
            return query.Take(pageSize).ToList();
        }

        public virtual IList<T> GetPageList(out int totalCount, int page, int pageSize, Expression<Func<T, bool>> filter = null, Expression<Func<T, object>> orderBy = null, bool isAsc = true, IEnumerable<string> includeList = null)
        {
            IQueryable<T> query = this.Entities;
            if (filter != null)
            {
                query = query.Where(filter);
            }
            if (includeList != null)
            {
                foreach (var item in includeList)
                {
                    query = query.Include(item);
                }
            }
            if (orderBy != null)
            {
                if (isAsc)
                {
                    query = query.OrderBy(orderBy);
                }
                else
                {
                    query = query.OrderByDescending(orderBy);
                }
            }
            totalCount = query.Count();
            return query.Skip(pageSize * (page - 1))
                        .Take(pageSize).ToList();
        }

        #endregion
    }
}
