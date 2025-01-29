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

       
    }
}
