namespace TrainComponentManager.Data.Repositories;

public interface IRepository<T>
{
    /// <summary>
    /// Gets all.
    /// </summary>
    /// <returns></returns>
    Task<IEnumerable<T>> GetAll();

    /// <summary>
    /// Gets all queryable.
    /// </summary>
    /// <returns></returns>
    IQueryable<T> GetAllQueryable();

    /// <summary>
    /// Adds the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    Task<bool> Add(T entity);

    /// <summary>
    /// Deletes the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    Task<bool> Delete(int id);

    /// <summary>
    /// Updates the specified entity.
    /// </summary>
    /// <param name="entity">The entity.</param>
    /// <returns></returns>
    Task<bool> Update(T entity);

    /// <summary>
    /// Finds the by identifier.
    /// </summary>
    /// <param name="Id">The identifier.</param>
    /// <returns></returns>
    Task<T?> FindById(int id);
}