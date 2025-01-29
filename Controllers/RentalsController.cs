using CarRentalAPI.Models;
using CarRentalAPI.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace CarRentalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RentalsController : ControllerBase
    {
        private readonly IRentalService _rentalService;
        public RentalsController(IRentalService rentalService)
        {
            _rentalService = rentalService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Rental>>> Get()
        {
            var rentals = await _rentalService.GetAsync();
            return Ok(rentals);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Rental>> Get(int id)
        {
            var rental = await _rentalService.GetAsync(id);
            if (rental == null)
            {
                return NotFound();
            }
            return Ok(rental);
        }
        [HttpPost]
        public async Task<ActionResult<Rental>> Create([FromBody] Rental rental)
        {
            try
            {
                await _rentalService.CreateAsync(rental);
                return CreatedAtAction(nameof(Get), new { id = rental.Id }, rental);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Rental updatedRental)
        {
            var existingRental = await _rentalService.GetAsync(id);
            if (existingRental == null)
            {
                return NotFound();
            }
            await _rentalService.UpdateAsync(id, updatedRental);
            return NoContent();
        }
        [HttpGet("search")]
        public async Task<ActionResult<List<Rental>>> SearchRentals([FromQuery] string searchTerm)
        {
            var rentals = await _rentalService.SearchRentalsAsync(searchTerm);
            return Ok(rentals);
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var rental = await _rentalService.GetAsync(id);
            if (rental == null)
            {
                return NotFound();
            }
            await _rentalService.DeleteAsync(id);
            return NoContent();
        }

    }
}
