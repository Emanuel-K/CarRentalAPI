using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models
{
    public class Car
    {
        [BsonId]
        
        public int Id { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Range(2000, 2025, ErrorMessage = "Year must be between 2000 and 2025.")]
        public int Year { get; set; }

        public bool IsAvailable { get; set; } = true;
    }
}
