using Microsoft.EntityFrameworkCore;
using TicketManagementSystemAPI.Models;
using TicketManagementSystemAPI.Repository;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

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
        try
        {
            return await _dbSet.FindAsync(id);
        }
        catch (Exception ex)
        {
            // Log the exception (you might want to use a logging framework here)
            throw new ApplicationException($"Unable to retrieve entity with ID {id}.", ex);
        }
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        try
        {
            return await _dbSet.ToListAsync();
        }
        catch (Exception ex)
        {
            // Log the exception
            throw new ApplicationException("Unable to retrieve entities.", ex);
        }
    }

    public async Task AddAsync(T entity)
    {
        try
        {
            await _dbSet.AddAsync(entity);
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateException ex)
        {
            // Log the exception
            throw new ApplicationException("Unable to add entity.", ex);
        }
    }

    public async Task UpdateAsync(T entity)
    {
        try
        {
            _dbSet.Attach(entity);
            _context.Entry(entity).State = EntityState.Modified;
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Log the exception
            throw new ApplicationException("Unable to update entity due to concurrency issues.", ex);
        }
        catch (DbUpdateException ex)
        {
            // Log the exception
            throw new ApplicationException("Unable to update entity.", ex);
        }
    }
}
