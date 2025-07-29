using System.ComponentModel.DataAnnotations;

namespace TrainComponentManager.Data.DTO;
public class CreateTrainComponentDto
{
    /// <summary>
    /// Gets or sets the name.
    /// </summary>
    /// <value>
    /// The name.
    /// </value>
    [Required(ErrorMessage = "Component Name is required.")]
    [StringLength(100, ErrorMessage = "Component Name cannot exceed 100 characters.")]
    public string Name { get; set; } = "";

    /// <summary>
    /// Gets or sets the unique number.
    /// </summary>
    /// <value>
    /// The unique number.
    /// </value>
    [Required(ErrorMessage = "Unique Number is required.")]
    [StringLength(50, ErrorMessage = "Unique Number cannot exceed 50 characters.")]
    public string UniqueNumber { get; set; } = "";

    /// <summary>
    /// Gets or sets a value indicating whether this instance can assign quantity.
    /// </summary>
    /// <value>
    ///   <c>true</c> if this instance can assign quantity; otherwise, <c>false</c>.
    /// </value>
    public bool CanAssignQuantity { get; set; }

    /// <summary>
    /// Gets or sets the initial quantity.
    /// Required only if CanAssignQuantity is true and quantity must be positive.
    /// </summary>
    public int? Quantity { get; set; }
}
