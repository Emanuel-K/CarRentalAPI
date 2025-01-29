using CarRentalAPI.Models;
namespace CarRentalAPI.Services{
  public interface IRentalService{
        Task<List<Rental>> GetAsync();
        Task<Rental> GetAsync(int id);
        Task CreateAsync(Rental rental);
        Task UpdateAsync(int id, Rental rental);
        Task DeleteAsync(int id);
        Task<List<Rental>> SearchRentalsAsync(string searchTerm);
  }}


