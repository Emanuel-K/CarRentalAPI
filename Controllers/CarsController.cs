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

        
    }
}
