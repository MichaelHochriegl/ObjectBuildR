[![GitHub Workflow](https://github.com/MichaelHochriegl/ObjectBuildR/actions/workflows/build.yml/badge.svg)](https://github.com/MichaelHochriegl/ObjectBuildR/actions/workflows/build-and-test.yml)
[![GitHub issues](https://img.shields.io/github/issues/michaelhochriegl/objectbuildr)](https://github.com/MichaelHochriegl/ObjectBuildR/issues)
# ObjectBuildR <img src="./ObjectBuildR.png" height="30" width="30" >
> Bringing you closer to neat builders without the hassle.
>This project aims at helping you get the [Builder Pattern](https://en.wikipedia.org/wiki/Builder_pattern) going without you writing the boilerplate code.


## General Information
This project uses dotnet Source Generators to do the heavy lifting needed to get builders up and running.
With this lib you can focus on only writing the code to get the specific functionality in your builders.

## Techstack
- .Net6
- [CodeGenHelpers](https://github.com/dansiegel/CodeGenHelpers)
- [Verify.SourceGenerators](https://github.com/VerifyTests/Verify.SourceGenerators)
- *Under Construction*

## Installation
Apart from this installation technic described below you can use VS22 NuGet Manager or the Rider NuGet UI.
For an IDE-agnostic installation it is best to use the command below to install the NuGet package in your project.
```shell
dotnet add package ObjectBuildR.Generator
```

This will install the latest, stable version.
To install a specific version you can define it like so (replace {VERSION} with the version you want to install):
```shell
dotnet add package ObjectBuildR.Generator --version {VERSION}
```

Example:
```shell
dotnet add package ObjectBuildR.Generator --version 1.0.0-alpha
```

## Usage
*(If you want to see some code you can find a [sample app](https://github.com/MichaelHochriegl/ObjectBuildR/tree/master/src/ObjectBuildR.Sample) right here in this repo)*
After installing the package you can build your first 'builder' with these steps:
1. Create the dto/entity with the appropriate properties (e.g. `public class User`)
2. Create a new `partial class` with the appropriate builder name (e.g. `public partial class UserBuilder`)
3. Decorate the newly created `partial class` with the attribute `[BuildRFor(Type = typeof({MODELDTO}))]` (e.g. `[BuildRFor(Type = typeof(User))]`)
4. ObjectBuildR will now auto-generate the sourcecode for the public properties in your dto/entity and the appropriate `With{PROPERTY}` methods
5. Next, you can start to add your specific build-methods

After following the above steps you should end up with a `builder`-class similar looking to this:
```csharp
// partial builder class
[BuildRFor(Type = typeof(User))]
public partial class UserBuilder
{
    public static UserBuilder Simple()
    {
        var builder = new UserBuilder()
            .WithFirstName("Michael")
            .WithLastName("Hochriegl");

        return builder;
    }
}
// user model
public class User
{
    public string FirstName { get; set; }
    public string LastName { get; set; }
}
```

ObjectBuildR will generate the following code for your builder, that you would have normally have to write yourself:
```csharp
public partial class UserBuilder : BuildRBase<User>
{
    public Lazy<string> FirstName =
        new Lazy<string>(() => default(string));
    public Lazy<string> LastName =
        new Lazy<string>(() => default(string));

    public UserBuilder WithFirstName(Func<string> func)
    {
        FirstName = new Lazy<string>(func);
        return this;
    }

    public UserBuilder WithFirstName(string value)
    {
        return WithFirstName(() => value);
    }

    public UserBuilder WithLastName(Func<string> func)
    {
        LastName = new Lazy<string>(func);
        return this;
    }

    public UserBuilder WithLastName(string value)
    {
        return WithLastName(() => value);
    }

    public override User Build()
    {
        if (Object?.IsValueCreated != true)
        {
            Object = new(() =>
            {
                var result = new User
                {
                    FirstName = FirstName.Value,
                    LastName = LastName.Value,
                }
                ;
                return result;
            }
            );
        }
        return Object.Value;
    }
}
```

## Contribute
Pull requests are more than welcome! To contribute, it's best to create an issue first and talk about the change you want to bring into this project.
After that, simply fork the repo, pull your forked repo local to your PC, create an appropriate feature-branch (naming example: `feat/999_my-awesome-feature`, this boils down to: `{TYPEOFCHANGE}/{ISSUENUMBER}_{SHORTDESCRIPTION}`).

You can now do your coding in this branch. After you are done, push your changes to your forked repo and create a Pull Request to this repo

## Acknowledgements
- Dan Siegel for creating [CodeGenHelpers](https://github.com/dansiegel/CodeGenHelpers)
- Simon Cropp and the whole team for providing a nice base for testing [Verify.SourceGenerators](https://github.com/VerifyTests/Verify.SourceGenerators)
- Anton Wieslander for teaching me a lot
- Nick Chapsas for teaching me a lot
- Andrew Lock for the best [blogs](https://andrewlock.net/creating-a-source-generator-part-1-creating-an-incremental-source-generator/) about Source Generators
