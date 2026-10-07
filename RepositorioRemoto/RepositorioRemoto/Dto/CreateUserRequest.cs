using RepositorioRemoto.Dto.Users;

namespace RepositorioRemoto.Dto;

/// <summary>
///     DTO para crear un nuevo usuario en la API.
///     El campo Id lo asigna el servidor.
/// </summary>
public record CreateUserRequest(
    string Name,
    string UserName,
    string Email,
    AddressDto Address,
    string Phone,
    string Website,
    CompanyDto Company
);