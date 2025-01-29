using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using CarRentalAPI.Models;
using CarRentalAPI.Services;
using Microsoft.AspNetCore.Mvc;

namespace CarRentalAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CarsController : ControllerBase
    {
        private readonly ICarService _carService;
        public CarsController(ICarService carService)
        {
            _carService = carService;
        }
        [HttpGet]
        public async Task<ActionResult<List<Car>>> Get()
        {
            var cars = await _carService.GetAsync();
            return Ok(cars);
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Car>> Get(int id)
        {
            var car = await _carService.GetAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            return Ok(car);
        }
        [HttpPost]
        public async Task<ActionResult<Car>> Create([FromBody] Car car)
        {
            await _carService.CreateAsync(car);
            return CreatedAtAction(nameof(Get), new { id = car.Id }, car);
        }
        [HttpPut("{id}")]
        public async Task<IActionResult> Update(int id, [FromBody] Car updatedCar)
        {
            var existingCar = await _carService.GetAsync(id);
            if (existingCar == null)
            {
                return NotFound();
            }
            await _carService.UpdateAsync(id, updatedCar);
            return NoContent();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            var car = await _carService.GetAsync(id);
            if (car == null)
            {
                return NotFound();
            }
            await _carService.DeleteAsync(id);
            return NoContent();
        }
        [HttpGet("search/{searchTerm}")]
        public async Task<ActionResult<List<Car>>> Search(string searchTerm)
        {
            var cars = await _carService.SearchCarsAsync(searchTerm);
            return Ok(cars);
        }
        
    }
}
