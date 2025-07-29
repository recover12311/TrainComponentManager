namespace TrainComponentManager.Data.DTO;

public class PaginatedResult<T>
{
    /// <summary>
    /// Gets or sets the items.
    /// </summary>
    /// <value>
    /// The items.
    /// </value>
    public IEnumerable<T> Items { get; set; } = new List<T>();

    /// <summary>
    /// Gets or sets the total count.
    /// </summary>
    /// <value>
    /// The total count.
    /// </value>
    public int TotalCount { get; set; }

    /// <summary>
    /// Gets or sets the page number.
    /// </summary>
    /// <value>
    /// The page number.
    /// </value>
    public int PageNumber { get; set; }

    /// <summary>
    /// Gets or sets the size of the page.
    /// </summary>
    /// <value>
    /// The size of the page.
    /// </value>
    public int PageSize { get; set; }

    /// <summary>
    /// Gets the total pages.
    /// </summary>
    /// <value>
    /// The total pages.
    /// </value>
    public int TotalPages => (int)Math.Ceiling((double)TotalCount / PageSize);
}
