using FluentAssertions;
using RepositorioRemoto.Mappers;
using RepositorioRemoto.Models;

namespace RepositorioRemoto.Test;

[TestFixture]
public class MappersCasosIncorrectosTest {
    [Test]
    public void Address_NullToJson_RetornaCadenaVacia() {
        // Arrange
        Address? address = null;

        // Act
        var result = address.ToJson();

        // Assert
        result.Should().BeEmpty();
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase("json invalido")]
    public void StringInvalido_ToAddress_RetornaDireccionPorDefecto(string json) {
        // Arrange

        // Act
        var result = json.ToAddress();

        // Assert
        result.Should().Be(new Address());
    }

    [Test]
    public void Company_NullToJson_RetornaCadenaVacia() {
        // Arrange
        Company? company = null;

        // Act
        var result = company.ToJson();

        // Assert
        result.Should().BeEmpty();
    }

    [TestCase("")]
    [TestCase("   ")]
    [TestCase("json invalido")]
    public void StringInvalido_ToCompany_RetornaEmpresaPorDefecto(string json) {
        // Arrange

        // Act
        var result = json.ToCompany();

        // Assert
        result.Should().Be(new Company());
    }
}
