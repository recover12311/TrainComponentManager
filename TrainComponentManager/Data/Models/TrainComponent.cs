using System.ComponentModel.DataAnnotations;

namespace TrainComponentManager.Data.Models
{
    public class TrainComponent
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
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the unique number.
        /// </summary>
        /// <value>
        /// The unique number.
        /// </value>
        [Required]
        public string UniqueNumber { get; set; } = string.Empty;

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
    }
}
