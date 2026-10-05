namespace RepositorioRemoto.Dto.User;

public record AddressDto(
    string Street,
    string Suite,
    string City,
    string ZipCode,
    GeoDto Geo
);