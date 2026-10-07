namespace RepositorioRemoto.Dto.Users;

public record UserDto(
    int Id,
    string Name,
    string UserName,
    string Email,
    AddressDto Address,
    string Phone,
    string Website,
    CompanyDto Company
);