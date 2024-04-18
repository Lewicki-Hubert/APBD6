namespace WebApplication1.Modules;

public class Animal
{
    public int IdAnimal { get; set; }
    public String Name { get; set; }

    public String Description { get; set; }
    
    public String Category { get; set; }
    
    public String Area { get; set; }
    
    public Animal(int idAnimal, string name, string description, string category, string area)
    {
        IdAnimal = idAnimal;
        Name = name;
        Description = description;
        Category = category;
        Area = area;
    }
    
    
}