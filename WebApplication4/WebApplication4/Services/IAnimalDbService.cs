using WebApplication4.Models;

namespace WebApplication4.Services
{
    public interface IAnimalDbService
    {
        Task<IList<Animal>> GetAllAnimals(string orderBy);
        Task<int> CreateAnimal(Animal animal);
        Task<bool> UpdateAnimal(int id, Animal animal);
        
        Task<bool> DeleteAnimal(int id);


    }
}
