using System.ComponentModel.DataAnnotations;

namespace TrainComponentManager.Data.DTO
{
    public class TrainComponentDto
    {
        /// <summary>
        /// Gets or sets the identifier.
        /// </summary>
        /// <value>
        /// The identifier.
        /// </value>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        [StringLength(100, ErrorMessage = "Component Name cannot exceed 100 characters.")]
        public string Name { get; set; } = "";

        /// <summary>
        /// Gets or sets the unique number.
        /// </summary>
        /// <value>
        /// The unique number.
        /// </value>
        [Required]
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
        /// Gets or sets the quantity.
        /// </summary>
        /// <value>
        /// The quantity.
        /// </value>
        public int? Quantity { get; set; }

        /// <summary>
        /// Creates new quantity.
        /// </summary>
        /// <value>
        /// The new quantity.
        /// </value>
        // Only for UI purposes, not stored in the database
        public int? NewQuantity { get; set; }
    }
}
