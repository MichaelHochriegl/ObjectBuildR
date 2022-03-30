using FluentAssertions;
using ObjectBuildR.Generator.IntegrationTests.Builders;
using ObjectBuildR.Generator.IntegrationTests.Entities;
using Xunit;

namespace ObjectBuildR.Generator.IntegrationTests;

public class UserBuilderTests
{
    [Fact]
    public void Should_Build_Simple_User()
    {
        #region Arrange

        var expectedUser = new User("Jeffrey", "Lebowski");
        var sut = UserBuilder.Simple();
        
        #endregion

        #region Act

        var result = sut.Build();

        #endregion

        #region Assert

        result.Should().BeEquivalentTo(expectedUser);

        #endregion
    }

    [Fact]
    public void Should_Build_User_WithDefaultValues()
    {
        #region Arrange

        var expectedUser = new User();
        var sut = new UserBuilder();

        #endregion

        #region Act

        var result = sut.Build();

        #endregion

        #region Assert

        result.Should().BeEquivalentTo(expectedUser);

        #endregion
    }
    
    [Theory]
    [InlineData("Walter", "Sobchak")]
    [InlineData("Donny", "Kerabatsos")]
    [InlineData("Maude", "Lebowski")]
    public void Should_Build_User_WithFirstAndLastName(string firstName, string lastName)
    {
        #region Arrange

        var expectedUser = new User(firstName, lastName);
        var sut = new UserBuilder()
            .WithFirstName(firstName)
            .WithLastName(lastName);

        #endregion

        #region Act

        var result = sut.Build();

        #endregion

        #region Assert

        result.Should().BeEquivalentTo(expectedUser);

        #endregion
    }
}