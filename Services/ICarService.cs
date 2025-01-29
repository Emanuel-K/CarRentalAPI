using CarRentalAPI.Models;

namespace CarRentalAPI.Services
{
    public interface ICarService
    {
        Task<List<Car>> GetAsync();
        Task<Car> GetAsync(int id);
        Task CreateAsync(Car car);
        Task UpdateAsync(int id, Car car);
        Task DeleteAsync(int id);
        Task<List<Car>> SearchCarsAsync(string searchTerm);
         Task<List<Car>> FilterByYearAsync(int startYear, int endYear);
    }
}