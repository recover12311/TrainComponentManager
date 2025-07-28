using Microsoft.EntityFrameworkCore;
using TrainComponentManager.Data.Models;

namespace TrainComponentManager.Data.Repositories;

public class TrainComponentRepository : IRepository<TrainComponent>
{
    /// <summary>
    /// The context
    /// </summary>
    private readonly AppDbContext _context;

    /// <summary>
    /// Initializes a new instance of the <see cref="TrainComponentManager.Data.Repositories.TrainComponentRepository"/> class.
    /// </summary>
    /// <param name="context">The context.</param>
    public TrainComponentRepository(AppDbContext context)
    {
        _context = context;
    }

    /// <summary>
    /// Gets all.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<TrainComponent>> GetAll()
    {
        return await _context.TrainComponents.ToListAsync();
    }

    /// <summary>
    /// Gets all queryable.
    /// </summary>
    /// <returns></returns>
    public Task<IQueryable<TrainComponent>> GetAllQueryable()
    {
        return Task.FromResult(_context.TrainComponents.AsQueryable());
    }

    /// <summary>
    /// Adds the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <exception cref="System.ArgumentNullException">entity - Entity can't be null.</exception>
    public async Task Add(TrainComponent entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity can't be null.");
        }

        await _context.TrainComponents.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Deletes the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <exception cref="System.ArgumentNullException">entity - Entity can't be null.</exception>
    public async Task Delete(TrainComponent entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity can't be null.");
        }

        _context.TrainComponents.Remove(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Updates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <exception cref="System.ArgumentNullException">entity - Entity can't be null.</exception>
    public async Task Update(TrainComponent entity)
    {
        if (entity == null)
        {
            throw new ArgumentNullException(nameof(entity), "Entity can't be null.");
        }

        _context.TrainComponents.Update(entity);
        await _context.SaveChangesAsync();
    }

    /// <summary>
    /// Finds the by identifier.
    /// </summary>
    /// <param name="Id">The identifier.</param>
    /// <returns></returns>
    public async Task<TrainComponent?> FindById(int Id)
    {
        return await this._context.TrainComponents.FindAsync(Id);
    }
}