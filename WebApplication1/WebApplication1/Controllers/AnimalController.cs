using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using WebApplication1.Modules;
using WebApplication1.Modules.DTOs;

namespace WebApplication1.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AnimalController : ControllerBase
{
    private readonly IConfiguration _configuration;

    public AnimalController(IConfiguration configuration)
    {
        _configuration = configuration;
    }


    [HttpGet]
    public IActionResult GetAnimals(string orderBy = "name")
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        string query = $"SELECT * FROM Animal ORDER BY {orderBy}";
        using var command = new SqlCommand(query, connection);
        var reader = command.ExecuteReader();

        var animals = new List<Animal>();
        while (reader.Read())
        {
            animals.Add(new Animal(
                reader.GetInt32(reader.GetOrdinal("IdAnimal")),
                reader.GetString(reader.GetOrdinal("Name")),
                reader.GetString(reader.GetOrdinal("Description")),
                reader.GetString(reader.GetOrdinal("Category")),
                reader.GetString(reader.GetOrdinal("Area"))
            ));
        }

        connection.Close();
        return Ok(animals);
    }


    [HttpPost]
    public IActionResult AddAnimal([FromBody] AddAnimal addAnimal)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        string query = "INSERT INTO Animal (Name, Description) VALUES (@Name, @Description)";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Name", addAnimal.Name);
        command.Parameters.AddWithValue("@Description", addAnimal.Description ?? string.Empty);

        command.ExecuteNonQuery();

        return Created($"api/animals/{addAnimal.Name}", addAnimal);
    }
    
    
    [HttpPut("{id}")]
    public IActionResult UpdateAnimal(int id, [FromBody] AddAnimal updateAnimal)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        string query = "UPDATE Animal SET Name = @Name, Description = @Description WHERE IdAnimal = @Id";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);
        command.Parameters.AddWithValue("@Name", updateAnimal.Name);
        command.Parameters.AddWithValue("@Description", updateAnimal.Description ?? string.Empty);

        int affected = command.ExecuteNonQuery();
        if (affected == 0)
            return NotFound();

        return NoContent();
    }
    
    [HttpDelete("{id}")]
    public IActionResult DeleteAnimal(int id)
    {
        using var connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();
        string query = "DELETE FROM Animal WHERE IdAnimal = @Id";
        using var command = new SqlCommand(query, connection);
        command.Parameters.AddWithValue("@Id", id);

        int affected = command.ExecuteNonQuery();
        if (affected == 0)
            return NotFound();

        return NoContent();
    }
    
}