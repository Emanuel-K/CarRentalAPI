using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using System.ComponentModel.DataAnnotations;

namespace CarRentalAPI.Models
{
    public class Rental
    {
        [BsonId]
        
        public int Id { get; set; }

        
        [Required]
        public int CarId { get; set; }
        [BsonIgnore]
        public string CarMake { get; set; }  

        [BsonIgnore]
        public string CarModel { get; set; }  

        [BsonIgnore]
        public int CarYear { get; set; }
        [Required]
        public string CustomerName { get; set; }

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }
    }
}
