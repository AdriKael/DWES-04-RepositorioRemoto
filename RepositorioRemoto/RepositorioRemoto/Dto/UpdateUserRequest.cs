using RepositorioRemoto.Dto.Users;

namespace RepositorioRemoto.Dto;

/// <summary>
///     DTO para actualizar un usuario existente en la API.
/// </summary>
public record UpdateUserRequest(
    int Id,
    string Name,
    string UserName,
    string Email,
    AddressDto Address,
    string Phone,
    string Website,
    CompanyDto Company
);