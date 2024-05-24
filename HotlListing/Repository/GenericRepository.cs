using HotlListing.Dtos;
using HotlListing.IRespository;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Query;
using System.Linq.Expressions;
using X.PagedList;


namespace HotlListing.Repository
{
    public class GenericRepository<T> : IGenericRepository<T> where T : class
    {
        private readonly DatabaseContext _dbContext;
        private readonly DbSet<T> _dbSet;

        public GenericRepository(DatabaseContext context)
        {
            _dbContext = context;
            _dbSet = _dbContext.Set<T>();

        }
        public async Task Delete(int id)
        {
            var entity = await _dbSet.FindAsync(id);
            _dbSet.Remove(entity);

        }

        public void DeleteRange(IEnumerable<T> entities)
        {
            _dbSet.RemoveRange(entities);
        }

        public async Task<T> Get(Expression<Func<T, bool>> expression, List<string> includes = null)
        {
            IQueryable<T> query = _dbSet;
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            return await query.AsNoTracking().FirstOrDefaultAsync(predicate: expression);
        }

        public async Task<IList<T>> GetAll(Expression<Func<T, bool>> expression = null, Func<IQueryable<T>, IOrderedQueryable<T>> orderBy = null, List<string> includes = null)
        {
            IQueryable<T> query = _dbSet;
            if (expression != null)
            {
                query = query.Where(expression);
            }
            if (includes != null)
            {
                foreach (var include in includes)
                {
                    query = query.Include(include);
                }
            }
            if (orderBy != null)
            {
                query = orderBy(query);

            }

            return await query.AsNoTracking().ToListAsync();
        }

        public async Task<IPagedList<T>> GetPagedList(RequestParams requestParams, Func<IQueryable<T>, IIncludableQueryable<T, object>> includes = null)
        {
            IQueryable<T> query = _dbSet;

            if (includes != null)
            {
                query = includes(query);

            }


            return await query.AsNoTracking().ToPagedListAsync(requestParams.page, requestParams.size);
        }

        public async Task Insert(T entity)
        {
            await _dbSet.AddAsync(entity);
        }

        public async Task InsertRange(IEnumerable<T> entities)
        {
            await _dbSet.AddRangeAsync(entities);
        }

        public void Update(T entity)
        {
            _dbSet.Attach(entity);
            _dbContext.Entry(entity).State = EntityState.Modified;

        }

    }
}
