namespace ObjectBuildR.Generator.IntegrationTests.Entities;

public class User
{
    public User()
    {
        
    }
    
    public User(string firstName, string lastName)
    {
        FirstName = firstName;
        LastName = lastName;
    }

    public string FirstName { get; set; }
    public string LastName { get; set; }
}