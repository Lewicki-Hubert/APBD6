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
    public IActionResult GetAnimals()
    {
        SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        // Definiujemy command
        SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "SELECT * FROM ANIMAL";

        //wykonanie zapytania
        var reader = command.ExecuteReader();

        List<Animal> animals = new List<Animal>();

        int idAnimalOrdinal = reader.GetOrdinal("IdAnimal");
        int nameOrdinal = reader.GetOrdinal("Name");

        reader.Read();
        while (reader.Read())
        {
            animals.Add(new Animal()
            {
                IdAnimal = reader.GetInt32(idAnimalOrdinal),
                Name = reader.GetString(nameOrdinal)
            });
        }

        return Ok();
    }


    [HttpPost]
    public IActionResult AddAnimals(AddAnimal addAnimal)
    {
        SqlConnection connection = new SqlConnection(_configuration.GetConnectionString("Default"));
        connection.Open();

        // Definiujemy command
        using SqlCommand command = new SqlCommand();
        command.Connection = connection;
        command.CommandText = "INSERT INTO ANIMAL VALUES (@animalName, '','','' )";
        command.Parameters.AddWithValue("@animalName", addAnimal.Name);
        
        // Wykonanie zamykanie
        command.ExecuteNonQuery();
        
        // _repository
        
        return Created("", null);
    }
}