using CarRentalAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CarRentalAPI.Services{
    public class RentalService : IRentalService
    {
        private readonly IMongoCollection<Rental> _rentals;
        private readonly IMongoCollection<Car> _cars;
        public RentalService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _rentals = mongoDatabase.GetCollection<Rental>("Rentals");
            _cars = mongoDatabase.GetCollection<Car>("Cars");}
        public async Task<List<Rental>> GetAsync()
        {
            var rentals = await _rentals.Find(_ => true).ToListAsync();
            foreach (var rental in rentals){
                var car = await _cars.Find(x => x.Id == rental.CarId).FirstOrDefaultAsync();
                if (car != null){
                    rental.CarMake = car.Make;
                    rental.CarModel = car.Model;
                    rental.CarYear = car.Year;
                }}
            return rentals;
            }
    public async Task<Rental> GetAsync(int id)
        {
            var rental = await _rentals.Find(x => x.Id == id).FirstOrDefaultAsync();
            if (rental == null) return null;
            var car = await _cars.Find(x => x.Id == rental.CarId).FirstOrDefaultAsync(); 
            if (car != null){
                rental.CarMake = car.Make;
                rental.CarModel = car.Model;  
                rental.CarYear = car.Year;  }
            return rental;
        }

        public async Task CreateAsync(Rental rental)
        {
            var car = await _cars.Find(x => x.Id == rental.CarId).FirstOrDefaultAsync();
            if (car == null){
                throw new InvalidOperationException("Car not found");}
            if (!car.IsAvailable){
                throw new InvalidOperationException("Car is not available for rental");}
            if (rental.StartDate >= rental.EndDate){
                throw new InvalidOperationException("End date must be after start date");}
            var existingRental = await _rentals.Find(r => 
                r.CarId == rental.CarId && 
                r.StartDate <= rental.EndDate && 
                r.EndDate >= rental.StartDate
            ).FirstOrDefaultAsync();
            if (existingRental != null){
                throw new InvalidOperationException("Car is already booked for these dates");}
            var update = Builders<Car>.Update.Set(c => c.IsAvailable, false);
            await _cars.UpdateOneAsync(x => x.Id == rental.CarId, update);
            await _rentals.InsertOneAsync(rental);}
