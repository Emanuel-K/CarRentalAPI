using CarRentalAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CarRentalAPI.Services
{
    public class CarService : ICarService
    {
        private readonly IMongoCollection<Car> _cars;
        public CarService(IOptions<MongoDBSettings> mongoDBSettings)
        {
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _cars = mongoDatabase.GetCollection<Car>("Cars");
        }
        public async Task<List<Car>> GetAsync()
        {
            return await _cars.Find(_ => true).ToListAsync();
        }
        public async Task<Car> GetAsync(int id)
        {
            return await _cars.Find(x => x.Id == id).FirstOrDefaultAsync();
        }
        public async Task CreateAsync(Car car)
        {
            await _cars.InsertOneAsync(car);
        }
        public async Task UpdateAsync(int id, Car updatedCar)
        {
            updatedCar.Id = id;
            await _cars.ReplaceOneAsync(x => x.Id == id, updatedCar);
        }
        public async Task DeleteAsync(int id)
        {
            await _cars.DeleteOneAsync(x => x.Id == id);
        }
        public async Task<List<Car>> SearchCarsAsync(string searchTerm)
        {
            var filter = Builders<Car>.Filter.Or(
                Builders<Car>.Filter.Eq(x => x.Make, searchTerm),
                Builders<Car>.Filter.Eq(x => x.Model, searchTerm)
            );
            return await _cars.Find(filter).ToListAsync();
        }
        public async Task<List<Car>> FilterByYearAsync(int startYear, int endYear)
        {
            var yearFilter = Builders<Car>.Filter.And(
                Builders<Car>.Filter.Gte(x => x.Year, startYear),
                Builders<Car>.Filter.Lte(x => x.Year, endYear)
            );
            return await _cars.Find(yearFilter).ToListAsync();
        }
    }
}