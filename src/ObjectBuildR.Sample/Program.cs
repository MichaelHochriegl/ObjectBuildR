// See https://aka.ms/new-console-template for more information

using Testing;
using MovieBuilder = ObjectBuildR.Sample.Builders.MovieBuilder;

Console.WriteLine("Hello, World!");

UserBuilder builder = new();

builder
    .WithFirstName("Michael")
    .WithLastName("Hochriegl");
    
    Console.WriteLine(builder.Build().FirstName);
    Console.WriteLine(builder.Build().LastName);
    
    MovieBuilder movieBuilder = MovieBuilder.Simple();

var movie = movieBuilder.Build();
Console.WriteLine(movie);
    // Console.WriteLine(movie.Name);
    // Console.WriteLine(movie.ReleaseDate);
    // Console.WriteLine(movie.VideoStream);
    // Console.WriteLine(movie.AudioStreams);