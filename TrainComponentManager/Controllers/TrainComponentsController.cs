using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainComponentManager.Data.DTO;
using TrainComponentManager.Data.Models;
using TrainComponentManager.Data.Repositories;

namespace TrainComponentManager.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TrainComponentsController : ControllerBase
{
    private readonly IRepository<TrainComponent> _repository;
    private readonly ILogger<TrainComponentsController> _logger;

    public TrainComponentsController(IRepository<TrainComponent> repository, ILogger<TrainComponentsController> logger)
    {
        this._repository = repository;
        this._logger = logger;
    }

    /// <summary>
    /// Gets all.
    /// </summary>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(IEnumerable<TrainComponent>))]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<PaginatedResult<TrainComponentDto>>> GetAll(
    [FromQuery] string? searchTerm,
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        try
        {
            var query = _repository.GetAllQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string lowerSearchTerm = searchTerm.ToLower();

                query = query.Where(tc => tc.Name.ToLower().Contains(lowerSearchTerm) ||
                                          tc.UniqueNumber.ToLower().Contains(lowerSearchTerm));
            }

            var totalCount = query.Count();

            // Pagination
            var trainComponents = await query
                .OrderBy(tc => tc.Id)
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .Select(tc => new TrainComponentDto
                {
                    Id = tc.Id,
                    Name = tc.Name,
                    UniqueNumber = tc.UniqueNumber,
                    CanAssignQuantity = tc.CanAssignQuantity,
                    Quantity = tc.Quantity
                })
                .ToListAsync();

            var paginatedResult = new PaginatedResult<TrainComponentDto>
            {
                Items = trainComponents,
                TotalCount = totalCount,
                PageNumber = pageNumber,
                PageSize = pageSize
            };

            return Ok(paginatedResult);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, $"Error retrieving all components.");
            return StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database.");
        }
    }

    /// <summary>
    /// Gets the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpGet("{id}")]
    [ProducesResponseType(StatusCodes.Status200OK, Type = typeof(TrainComponent))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TrainComponent>> Get(int id)
    {
        try
        {
            var trainComponent = await this._repository.FindById(id);

            return trainComponent == null ? (ActionResult<TrainComponent>)this.NotFound($"Train component with {id} wasn't found.") : (ActionResult<TrainComponent>)this.Ok(trainComponent);
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, $"Error retrieving train component {id}.");
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Error retrieving data from the database.");
        }
    }

    /// <summary>
    /// Updates the quantity.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <param name="quantity">The quantity.</param>
    /// <returns></returns>
    [HttpPut("{id}/quantity")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> UpdateQuantity(int id, [FromBody] int quantity)
    {
        if (quantity < 1)
        {
            return BadRequest("Quantity must be a positive integer.");
        }

        if (!this.ModelState.IsValid)
        {
            return this.BadRequest(this.ModelState);
        }

        try
        {
            var item = await this._repository.FindById(id);

            if (item == null)
            {
                return NotFound($"Train component with {id} wasn't found.");
            }

            if (!item.CanAssignQuantity)
            {
                return BadRequest("Quantity cannot be assigned to this component.");
            }

            item.Quantity = quantity;

            await this._repository.Update(item);
            return NoContent();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            // Log the concurrency exception
            _logger.LogWarning(ex, "Concurrency conflict occurred for component with ID {Id} during quantity update.", id);

            // Re-check if item exists to distinguish between actual deletion and concurrent update
            if (await this._repository.FindById(id) == null)
            {
                return this.NotFound($"Component with ID {id} not found (may have been deleted due to a concurrency conflict).");
            }
            // If it still exists, then it was a concurrent modification, rethrow or handle differently.
            // For this scenario, throwing allows global exception handler to process or the client to retry.
            throw; // Re-throw if it's a genuine concurrency conflict, not a 'not found' case.
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, $"Error updating component with ID {id}.");
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Error saving data to database.");
        }
    }

    /// <summary>
    /// Creates the specified dto.
    /// </summary>
    /// <param name="dto">The dto.</param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(StatusCodes.Status201Created, Type = typeof(TrainComponent))]
    [ProducesResponseType(StatusCodes.Status400BadRequest, Type = typeof(ValidationProblemDetails))]
    [ProducesResponseType(StatusCodes.Status409Conflict)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<ActionResult<TrainComponent>> Create([FromBody] CreateTrainComponentDto dto)
    {
        if (!ModelState.IsValid)
        {
            return BadRequest(ModelState);
        }

        if (dto.CanAssignQuantity && (dto.Quantity == null || dto.Quantity < 1))
        {
            ModelState.AddModelError(nameof(dto.Quantity), "Quantity must be a positive integer when CanAssignQuantity is true.");
            return BadRequest(ModelState);
        }
        else if (!dto.CanAssignQuantity)
        {
            dto.Quantity = null;
        }

        var newComponent = new TrainComponent
        {
            Name = dto.Name,
            UniqueNumber = dto.UniqueNumber,
            CanAssignQuantity = dto.CanAssignQuantity,
            Quantity = dto.Quantity
        };

        try
        {
            await _repository.Add(newComponent);

            return CreatedAtAction(nameof(Get), new { id = newComponent.Id }, newComponent);
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error creating train component due to a database update failure.");

            if (ex.InnerException?.Message.Contains("duplicate key", StringComparison.OrdinalIgnoreCase) == true ||
                ex.InnerException?.Message.Contains("unique constraint", StringComparison.OrdinalIgnoreCase) == true)
            {
                return Conflict("A component with this Unique Number already exists.");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Error creating component in the database.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while creating a train component.");
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while creating the component.");
        }
    }

    /// <summary>
    /// Deletes the specified identifier.
    /// </summary>
    /// <param name="id">The identifier.</param>
    /// <returns></returns>
    [HttpDelete("{id}")]
    [ProducesResponseType(StatusCodes.Status204NoContent)]
    [ProducesResponseType(StatusCodes.Status404NotFound)]
    [ProducesResponseType(StatusCodes.Status500InternalServerError)]
    public async Task<IActionResult> Delete(int id)
    {
        try
        {
            bool deleted = await _repository.Delete(id);

            if (!deleted)
            {
                return NotFound($"Train component with ID {id} not found.");
            }

            return NoContent();
        }
        catch (DbUpdateException ex)
        {
            _logger.LogError(ex, "Error deleting train component {Id} due to a database update failure.", id);
            if (ex.InnerException?.Message.Contains("REFERENCE constraint", StringComparison.OrdinalIgnoreCase) == true)
            {
                return BadRequest("Cannot delete component as it is referenced by other entities.");
            }

            return StatusCode(StatusCodes.Status500InternalServerError, "Error deleting component from the database.");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "An unexpected error occurred while deleting train component {Id}.", id);
            return StatusCode(StatusCodes.Status500InternalServerError, "An unexpected error occurred while deleting the component.");
        }
    }

}
