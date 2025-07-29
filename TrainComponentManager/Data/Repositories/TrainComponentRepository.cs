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
        this._context = context;
    }

    /// <summary>
    /// Gets all.
    /// </summary>
    /// <returns></returns>
    public async Task<IEnumerable<TrainComponent>> GetAll()
    {
        return await this._context.TrainComponents.ToListAsync();
    }

    /// <summary>
    /// Gets all queryable.
    /// </summary>
    /// <returns></returns>
    public IQueryable<TrainComponent> GetAllQueryable()
    {
        return this._context.TrainComponents.AsQueryable();
    }

    /// <summary>
    /// Adds the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <exception cref="System.ArgumentNullException">entity - Entity can't be null.</exception>
    public async Task<bool> Add(TrainComponent entity)
    {
        if (entity == null)
        {
            return false;
        }

        await this._context.TrainComponents.AddAsync(entity);

        await this._context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Deletes the specified entity.
    /// </summary>
    /// <param name="id"></param>
    /// <exception cref="System.ArgumentNullException">entity - Entity can't be null.</exception>
    public async Task<bool> Delete(int id)
    {
        var entity = await this._context.TrainComponents.FindAsync(id);

        if (entity == null)
        {
            return false;
        }

        this._context.TrainComponents.Remove(entity);

        await this._context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Updates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <exception cref="System.ArgumentNullException">entity - Entity can't be null.</exception>
    public async Task<bool> Update(TrainComponent entity)
    {
        if (entity == null)
        {
            return false;
        }

        this._context.TrainComponents.Update(entity);

        await this._context.SaveChangesAsync();

        return true;
    }

    /// <summary>
    /// Finds the by identifier.
    /// </summary>
    /// <param name="Id">The identifier.</param>
    /// <returns></returns>
    public async Task<TrainComponent?> FindById(int id)
    {
        return await this._context.TrainComponents.FindAsync(id);
    }
}