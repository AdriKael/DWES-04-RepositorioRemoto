using System.Text.Json;
using RepositorioRemoto.Dto.Users;
using RepositorioRemoto.Models;
using Serilog;

namespace RepositorioRemoto.Mappers;

public static class AddressMapper {
    private static readonly ILogger _logger = Log.ForContext(typeof(AddressMapper));

    private static readonly JsonSerializerOptions _jsonOptions = new() {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    private static Address DefaultAddress => new(
        string.Empty,
        string.Empty,
        string.Empty,
        string.Empty,
        new Geo(string.Empty, string.Empty)
    );

    public static string ToJson(this Address? address) {
        _logger.Debug("[MAPP-MODEL-JSON] Address a JSON");
        return address is null ? string.Empty : JsonSerializer.Serialize(address, _jsonOptions);
    }

    public static Address ToAddress(this string? json) {
        _logger.Debug("[MAPP-JSON-MODEL] JSON a Address");
        if (string.IsNullOrWhiteSpace(json)) return DefaultAddress;

        try {
            var result = JsonSerializer.Deserialize<Address>(json, _jsonOptions);
            return result ?? DefaultAddress;
        }
        catch (JsonException) {
            return DefaultAddress;
        }
    }

    public static Address ToModel(this AddressDto dto) {
        _logger.Debug("[MAPP-DTO-MODEL] AddressDto a Model");
        return new Address(
            dto.Street,
            dto.Suite,
            dto.City,
            dto.ZipCode,
            new Geo(
                dto.Geo.Lat,
                dto.Geo.Lng
            )
        );
    }

    public static AddressDto ToDto(this Address address) {
        _logger.Debug("[MAPP-MODEL-DTO] Address a AddressDto");

        return new AddressDto(
            address.Street,
            address.Suite,
            address.City,
            address.ZipCode,
            new GeoDto(
                address.Geo.Lat,
                address.Geo.Lng
            )
        );
    }
}