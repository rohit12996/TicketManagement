using Microsoft.EntityFrameworkCore;
using TicketManagementSystemAPI.Models;
using TicketManagementSystemAPI.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    protected readonly RohitContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(RohitContext context)
    {
        _context = context;
        _dbSet = _context.Set<T>();
    }

    public async Task<T> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Attach(entity);
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}
