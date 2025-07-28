using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrainComponentManager.Data;

namespace TrainComponentManager.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TrainComponentsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public TrainComponentsController(AppDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var result = await _context.TrainComponents.ToListAsync();

            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var item = await _context.TrainComponents.FindAsync(id);
            return item == null ? NotFound() : Ok(item);
        }

        [HttpPut("{id}/quantity")]
        public async Task<IActionResult> UpdateQuantity(int id, [FromBody] int quantity)
        {
            if (quantity <= 0)
                return BadRequest("Quantity must be a positive integer.");

            var item = await _context.TrainComponents.FindAsync(id);
            if (item == null) return NotFound();
            if (!item.CanAssignQuantity)
                return BadRequest("Quantity cannot be assigned to this component.");

            item.Quantity = quantity;
            await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
