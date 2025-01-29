using CarRentalAPI.Models;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace CarRentalAPI.Services
{
    public class CarService : ICarService
    {
        private readonly IMongoCollection<Car> _cars;
        public CarService(IOptions<MongoDBSettings> mongoDBSettings){
            var mongoClient = new MongoClient(mongoDBSettings.Value.ConnectionString);
            var mongoDatabase = mongoClient.GetDatabase(mongoDBSettings.Value.DatabaseName);
            _cars = mongoDatabase.GetCollection<Car>("Cars");}
        public List<Car> Get(){
            return _cars.Find(_ => true).ToList();}
        public Car Get(int id){
            return _cars.Find(x => x.Id == id).FirstOrDefault();}
        public void Create(Car car){
            _cars.InsertOne(car);}
        public void Update(int id, Car updatedCar){
            updatedCar.Id = id;
            _cars.ReplaceOne(x => x.Id == id, updatedCar);}
        public void Delete(int id){
            _cars.DeleteOne(x => x.Id == id);}
        public List<Car> SearchCars(string searchTerm){
            var filter = Builders<Car>.Filter.Or(
                Builders<Car>.Filter.Eq(x => x.Make, searchTerm),
                Builders<Car>.Filter.Eq(x => x.Model, searchTerm)
            );
            return _cars.Find(filter).ToList();}
        public List<Car> FilterByYear(int startYear, int endYear){ 
            var yearFilter = Builders<Car>.Filter.And(
                Builders<Car>.Filter.Gte(x => x.Year, startYear),
                Builders<Car>.Filter.Lte(x => x.Year, endYear)
            );
            return _cars.Find(yearFilter).ToList();}
    }
}
