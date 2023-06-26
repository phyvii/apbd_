using Microsoft.AspNetCore.Mvc;
using WebApplication4.Models;
using WebApplication4.Services;

namespace WebApplication4.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    
    public class AnimalController : ControllerBase
    {
        private readonly IAnimalDbService _animalDbService;

        public AnimalController(IAnimalDbService animalDbService)
        {
            _animalDbService = animalDbService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAnimals([FromQuery]string orderBy = "name")
        {
            try
            {
                IList<Animal> animals = await _animalDbService.GetAllAnimals(orderBy);
                return Ok(animals);
            }catch (Exception ex)
            {
                return StatusCode(500,ex);
            }
        }

        [HttpPost]
        public async Task<IActionResult> CreateAnimal([FromBody] Animal animal)
        {
            try
            {
                int result  = await _animalDbService.CreateAnimal(animal);
                if (result == 0)
                {
                    return Ok($"animal was added");

                }
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Internal server error: {ex.Message}");
            }
            return BadRequest();
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateAnimal(int id, [FromBody] Animal animal)
        {
            try
            {
                bool updated = await _animalDbService.UpdateAnimal(id, animal);

                if (updated)
                {
                    return Ok("Animal has been updated");
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500,ex.Message);
            }
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteAnimal(int id)
        {
            try
            {
                bool deleted = await _animalDbService.DeleteAnimal(id);

                if (deleted)
                {
                    return Ok("animal deleted");
                }

                return NotFound();
            }
            catch (Exception ex)
            {
                return StatusCode(500, ex.Message);
            }
        }
    }
}
