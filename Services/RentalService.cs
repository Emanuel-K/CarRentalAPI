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
        public async Task UpdateAsync(int id, Rental updatedRental){
            var existingRental = await GetAsync(id);
            if (existingRental == null){
                throw new InvalidOperationException("Rental not found");}
            if (updatedRental.CarId != existingRental.CarId){
                var newCar = await _cars.Find(x => x.Id == updatedRental.CarId).FirstOrDefaultAsync();
                if (newCar == null){
                    throw new InvalidOperationException("New car not found");}

                if (!newCar.IsAvailable){
                    throw new InvalidOperationException("New car is not available for rental");}
                var updateOldCar = Builders<Car>.Update.Set(c => c.IsAvailable, true);
                await _cars.UpdateOneAsync(x => x.Id == existingRental.CarId, updateOldCar);
                var updateNewCar = Builders<Car>.Update.Set(c => c.IsAvailable, false);
                await _cars.UpdateOneAsync(x => x.Id == updatedRental.CarId, updateNewCar);
            }
            updatedRental.Id = id;
            await _rentals.ReplaceOneAsync(x => x.Id == id, updatedRental);}

        public async Task DeleteAsync(int id)
        {
            var rental = await GetAsync(id);
            if (rental != null){
                var update = Builders<Car>.Update.Set(c => c.IsAvailable, true);
                await _cars.UpdateOneAsync(x => x.Id == rental.CarId, update);
                await _rentals.DeleteOneAsync(x => x.Id == id);}
        }
        public async Task<List<Rental>> SearchRentalsAsync(string searchTerm){
            searchTerm = searchTerm.ToLower();
            var rentals = await _rentals.Find(_ => true).ToListAsync();
            foreach (var rental in rentals){
                var car = await _cars.Find(x => x.Id == rental.CarId).FirstOrDefaultAsync();
                if (car != null){
                    rental.CarMake = car.Make;
                    rental.CarModel = car.Model;
                    rental.CarYear = car.Year;}
            }
            var filteredRentals = rentals.Where(r => 
                r.CarMake.ToLower().Equals(searchTerm) || 
                r.CarModel.ToLower().Equals(searchTerm)
            ).ToList();
            return filteredRentals;
        }
}}
