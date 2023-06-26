using System.Data.Common;
using System.Data.SqlClient;
using WebApplication4.Models;

namespace WebApplication4.Services
{
    public class AnimalDbService : IAnimalDbService
    {
        private readonly string _localConnectionString = "Data Source=(localdb)\\MSSQLLocalDB;Initial Catalog=test;Integrated Security=True;Encrypt=False";

        public async Task<IList<Animal>> GetAllAnimals(string orderBy)
        {
            List<Animal> animals = new List<Animal>();

            await using SqlConnection sqlConnection = new(_localConnectionString);
            await using SqlCommand sqlCommand = new();

            string sql;

            if (string.IsNullOrEmpty(orderBy))
            {
                sql = "SELECT * FROM Animal";
            }
            else
            {
                sql = $"SELECT * FROM Animal ORDER BY {orderBy} ASC";
            }

            sqlCommand.CommandText = sql;
            sqlCommand.Connection = sqlConnection;

            await sqlConnection.OpenAsync();

            await using SqlDataReader sqlDataReader = await sqlCommand.ExecuteReaderAsync();

            while (await sqlDataReader.ReadAsync())
            {
                Animal animal = new Animal
                {
                    IdAnimal = sqlDataReader.GetInt32(0),
                    Name = sqlDataReader.GetString(1),
                    Description = sqlDataReader.GetString(2),
                    Category = sqlDataReader.GetString(3),
                    Area = sqlDataReader.GetString(4)
                };

                animals.Add(animal);
            }

            await sqlConnection.CloseAsync();

            return animals;
            
        }

       

        public async Task<int> CreateAnimal(Animal animal)
        {
            if (animal != null)
            {
                await using SqlConnection sqlConnection = new(_localConnectionString);
                await using SqlCommand sqlCommand = new();

                string sql = "INSERT INTO Animal (Name, Description, Category, Area) VALUES (@Name, @Description, @Category, @Area)";
                sqlCommand.CommandText = sql;
                sqlCommand.Connection = sqlConnection;

                //var animals = GetAllAnimals("name");
                

                await sqlConnection.OpenAsync();

                sqlCommand.Parameters.AddWithValue("@Name", animal.Name);
                sqlCommand.Parameters.AddWithValue("@Description", animal.Description);
                sqlCommand.Parameters.AddWithValue("@Category", animal.Category);
                sqlCommand.Parameters.AddWithValue("@Area", animal.Area);

                sqlCommand.ExecuteNonQuery();

                sqlConnection.CloseAsync();

                await sqlConnection.CloseAsync();
                return 0;
            }
            else
                return 1;

        }

        public async Task<bool> UpdateAnimal(int id, Animal animal)
        {
            if (animal != null)
            {
                await using SqlConnection sqlConnection = new(_localConnectionString);
                await using SqlCommand sqlCommand = new();

                string sql = "UPDATE Animal SET Name = @Name, Description = @Description, Category = @Category, Area = @Area WHERE IdAnimal = @Id";
                sqlCommand.CommandText = sql;
                sqlCommand.Connection = sqlConnection;

                await sqlConnection.OpenAsync();


                sqlCommand.Parameters.AddWithValue("@Name", animal.Name);
                sqlCommand.Parameters.AddWithValue("@Description", animal.Description);
                sqlCommand.Parameters.AddWithValue("@Category", animal.Category);
                sqlCommand.Parameters.AddWithValue("@Area", animal.Area);
                sqlCommand.Parameters.AddWithValue("@Id", id);

                int rowsAffected = await sqlCommand.ExecuteNonQueryAsync();
                await sqlConnection.CloseAsync();
                return rowsAffected > 0;
            }
           return false;
            
        }

        public async Task<bool> DeleteAnimal(int id)
        {
            
                await using SqlConnection sqlConnection = new(_localConnectionString);
                await using SqlCommand sqlCommand = new();

                string sql = "DELETE FROM Animal WHERE IdAnimal = @Id";
                sqlCommand.CommandText = sql;
                sqlCommand.Connection = sqlConnection;

                await sqlConnection.OpenAsync();

               
                sqlCommand.Parameters.AddWithValue("@Id", id);

                int rowsAffected = await sqlCommand.ExecuteNonQueryAsync();

                return rowsAffected > 0;
           
            
        }




    }
}
