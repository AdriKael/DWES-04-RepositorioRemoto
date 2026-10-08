using FluentAssertions;
using RepositorioRemoto.Dto;
using RepositorioRemoto.Dto.Users;
using RepositorioRemoto.Mappers;
using RepositorioRemoto.Models;

namespace RepositorioRemoto.Test.Mappers;

[TestFixture]
public class MappersCasosCorrectosTest {
    [Test]
    public void CreateUserRequest_ToModel_CopiaTodosLosCampos() {
        // Arrange
        var request = CreateRequest();

        // Act
        var result = request.ToModel();

        // Assert
        result.Id.Should().Be(0);
        result.Name.Should().Be(request.Name);
        result.Address.Should().BeEquivalentTo(request.Address);
        result.Company.Should().BeEquivalentTo(request.Company);
    }

    [Test]
    public void UpdateUserRequest_ToModel_CopiaIdYTodosLosCampos() {
        // Arrange
        var request = CreateUpdateRequest();

        // Act
        var result = request.ToModel();

        // Assert
        result.Id.Should().Be(request.Id);
        result.Should().BeEquivalentTo(new User(request.Id, request.Name, request.UserName, request.Email,
            request.Address.ToModel(), request.Phone, request.Website, request.Company.ToModel()));
    }

    [Test]
    public void User_ToCreateRequest_CopiaTodosLosCampos() {
        // Arrange
        var user = CreateUser();

        // Act
        var result = user.ToCreateRequest();

        // Assert
        result.Should().BeEquivalentTo(CreateRequest());
    }

    [Test]
    public void User_ToUpdateRequest_CopiaIdYTodosLosCampos() {
        // Arrange
        var user = CreateUser();

        // Act
        var result = user.ToUpdateRequest();

        // Assert
        result.Should().BeEquivalentTo(CreateUpdateRequest());
    }

    [Test]
    public void Address_ToJsonYToAddress_ConservaLosValores() {
        // Arrange
        var address = CreateUser().Address;

        // Act
        var json = address.ToJson();
        var result = json.ToAddress();

        // Assert
        result.Should().BeEquivalentTo(address);
        json.Should().Contain("street");
    }

    [Test]
    public void AddressDto_ToModelYToDto_ConservaLosValores() {
        // Arrange
        var dto = CreateRequest().Address;

        // Act
        var model = dto.ToModel();
        var result = model.ToDto();

        // Assert
        result.Should().BeEquivalentTo(dto);
    }

    [Test]
    public void Company_ToJsonYToCompany_ConservaLosValores() {
        // Arrange
        var company = CreateUser().Company;

        // Act
        var json = company.ToJson();
        var result = json.ToCompany();

        // Assert
        result.Should().BeEquivalentTo(company);
        json.Should().Contain("name");
    }

    [Test]
    public void CompanyDto_ToModelYToDto_ConservaLosValores() {
        // Arrange
        var dto = CreateRequest().Company;

        // Act
        var model = dto.ToModel();
        var result = model.ToDto();

        // Assert
        result.Should().BeEquivalentTo(dto);
    }

    private static User CreateUser() => new(
        7, "Name", "user", "user@test.com",
        new Address("Street", "Suite", "City", "00000", new Geo("1", "2")),
        "555", "site", new Company("Company", "phrase", "bs"));

    private static CreateUserRequest CreateRequest() => new(
        "Name", "user", "user@test.com",
        new AddressDto("Street", "Suite", "City", "00000", new GeoDto("1", "2")),
        "555", "site", new CompanyDto("Company", "phrase", "bs"));

    private static UpdateUserRequest CreateUpdateRequest() => new(
        7, "Name", "user", "user@test.com",
        new AddressDto("Street", "Suite", "City", "00000", new GeoDto("1", "2")),
        "555", "site", new CompanyDto("Company", "phrase", "bs"));
}
