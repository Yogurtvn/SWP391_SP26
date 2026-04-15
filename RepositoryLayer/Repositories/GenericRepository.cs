using Microsoft.EntityFrameworkCore;
using RepositoryLayer.Data;
using RepositoryLayer.Interfaces;
using System.Linq.Expressions;

namespace RepositoryLayer.Repositories;

public class GenericRepository<T>(OnlineEyewearDbContext context) : IGenericRepository<T>
    where T : class
{
    protected readonly OnlineEyewearDbContext Context = context;
    protected readonly DbSet<T> DbSet = context.Set<T>();

    public async Task<T?> GetByIdAsync(object id)
    {
        return await DbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await DbSet.ToListAsync();
    }

    public async Task<IEnumerable<T>> FindAsync(
        Expression<Func<T, bool>>? filter = null,
        Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
        string includeProperties = "",
        bool tracked = true)
    {
        IQueryable<T> query = BuildQuery(filter, includeProperties, tracked);

        if (orderBy is not null)
        {
            query = orderBy(query);
        }

        return await query.ToListAsync();
    }

    public async Task<T?> GetFirstOrDefaultAsync(
        Expression<Func<T, bool>> filter,
        string includeProperties = "",
        bool tracked = true)
    {
        return await BuildQuery(filter, includeProperties, tracked).FirstOrDefaultAsync();
    }

    public async Task AddAsync(T entity)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task AddRangeAsync(IEnumerable<T> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }

    public void Update(T entity)
    {
        DbSet.Update(entity);
    }

    public void Remove(T entity)
    {
        DbSet.Remove(entity);
    }

    public void RemoveRange(IEnumerable<T> entities)
    {
        DbSet.RemoveRange(entities);
    }

    public async Task<bool> ExistsAsync(Expression<Func<T, bool>> filter)
    {
        return await DbSet.AnyAsync(filter);
    }

    public async Task<int> CountAsync(Expression<Func<T, bool>>? filter = null)
    {
        return filter is null
            ? await DbSet.CountAsync()
            : await DbSet.CountAsync(filter);
    }

    private IQueryable<T> BuildQuery(
        Expression<Func<T, bool>>? filter,
        string includeProperties,
        bool tracked)
    {
        IQueryable<T> query = DbSet;

        if (!tracked)
        {
            query = query.AsNoTracking();
        }

        if (filter is not null)
        {
            query = query.Where(filter);
        }

        foreach (var includeProperty in includeProperties.Split(',', StringSplitOptions.RemoveEmptyEntries))
        {
            query = query.Include(includeProperty.Trim());
        }

        if (!string.IsNullOrWhiteSpace(includeProperties) && Context.Database.IsRelational())
        {
            query = query.AsSplitQuery();
        }

        return query;
    }
}
