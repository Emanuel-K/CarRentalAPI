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


    }
}
