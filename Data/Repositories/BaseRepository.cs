using Data.Contexts;
using Data.Interfaces;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Data.Repositories;

public abstract class BaseRepository<TEntity>(OrderContext context) : IBaseRepository<TEntity> where TEntity : class
{
    private readonly OrderContext _context = context;
    private readonly DbSet<TEntity> _dbSet = context.Set<TEntity>();

    public virtual async Task<TEntity> CreateAsync(TEntity entity)
    {
        if (entity == null)
            return null!;

        try
        {
            await _dbSet.AddAsync(entity);
            return entity;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error creating {nameof(TEntity)} :: {ex.Message}");
            return null!;
        }
    }

    public virtual async Task<IEnumerable<TEntity>> GetAsync()
    {
        try
        {
            var entityList = await _dbSet.ToListAsync();
            return entityList;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error finding entities :: {ex.Message}");
            return null!;
        }
    }

    public virtual async Task<TEntity> GetAsync(Expression<Func<TEntity, bool>> expression)
    {
        if (expression == null)
            return null!;
        try
        {
            var entity = await _dbSet.FirstOrDefaultAsync(expression);

            if (entity == null)
                return null!;

            return entity;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error finding entity :: {ex.Message}");
            return null!;
        }
    }

    public virtual async Task<TEntity> UpdateAsync(Expression<Func<TEntity, bool>> expression, TEntity updatedEntity)
    {
        //Got help here from Claude AI with excluding the primary key property from updating. 
        if (updatedEntity == null)
            return null!;
        try
        {
            var existingEntity = await _dbSet.FirstOrDefaultAsync(expression) ?? null!;
            if (existingEntity == null)
                return null!;

            var propertyValues = _context.Entry(updatedEntity).CurrentValues;
            var existingEntry = _context.Entry(existingEntity);

            var keyName = existingEntry.Metadata?.FindPrimaryKey()?.Properties
                .Select(p => p.Name).SingleOrDefault();

            foreach (var property in propertyValues.Properties)
            {
                if (property.Name != keyName)
                {
                    var value = propertyValues[property.Name];
                    existingEntry.Property(property.Name).CurrentValue = value;
                }
            }

            return existingEntity;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error updating {nameof(TEntity)} :: {ex.Message}");
            return null!;
        }
    }

    public virtual async Task<bool> DeleteAsync(Expression<Func<TEntity, bool>> expression)
    {
        if (expression == null)
            return false;

        try
        {
            var existingEntity = await GetAsync(expression);
            if (existingEntity == null)
                return false;

            _dbSet.Remove(existingEntity);
            return true;
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error deleting {nameof(TEntity)} :: {ex.Message}");
            return false;
        }
    }

    public virtual async Task<bool> AlreadyExistsAsync(Expression<Func<TEntity, bool>> expression)
    {
        try
        {
            return await _dbSet.AnyAsync(expression);
        }
        catch (Exception ex)
        {
            Debug.WriteLine($"Error :: {ex.Message}");
            return false;
        }
    }

    public virtual async Task<int> SaveAsync()
    {
        return await _context.SaveChangesAsync();
    }
}
