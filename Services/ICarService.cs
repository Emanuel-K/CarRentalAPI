using CarRentalAPI.Models;

namespace CarRentalAPI.Services
{
    public interface ICarService
    {
        List<Car> Get();
        Car Get(int id);
        void Create(Car car);
        void Update(int id, Car car);
        void Delete(int id);
        List<Car> SearchCars(string searchTerm);
        List<Car> FilterByYear(int startYear, int endYear);
    }
}
