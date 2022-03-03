// See https://aka.ms/new-console-template for more information

using Testing;

Console.WriteLine("Hello, World!");

UserBuilder builder = new();
builder
    .WithFirstName("Michael")
    .WithLastName("Hochriegl");
    
    Console.WriteLine(builder.Build().FirstName);
    Console.WriteLine(builder.Build().LastName);