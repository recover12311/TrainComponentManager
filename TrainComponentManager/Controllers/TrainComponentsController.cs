using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
    public async Task<ActionResult<IEnumerable<TrainComponent>>> GetAll([FromQuery] string? searchTerm = null)
    {
        try
        {
            IQueryable<TrainComponent> query = _repository.GetAllQueryable();

            if (!string.IsNullOrWhiteSpace(searchTerm))
            {
                string lowerSearchTerm = searchTerm.ToLower();

                query = query.Where(tc => tc.Name.ToLower().Contains(lowerSearchTerm) ||
                                          tc.UniqueNumber.ToLower().Contains(lowerSearchTerm));
            }

            var trainComponents = await query.ToListAsync();

            return Ok(trainComponents);
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
        catch (DbUpdateConcurrencyException)
        {
            if (await this._repository.FindById(id) == null)
            {
                return this.NotFound($"Component with ID {id} not found (may have been deleted).");
            }
            throw;
        }
        catch (Exception ex)
        {
            this._logger.LogError(ex, $"Error updating component with ID {id}.");
            return this.StatusCode(StatusCodes.Status500InternalServerError, "Error saving data to database.");
        }
    }

}
