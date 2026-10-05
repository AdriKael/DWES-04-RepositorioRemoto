using RepositorioRemoto.Dto;
using RepositorioRemoto.Models;
using Serilog;

namespace RepositorioRemoto.Mappers;

public static class UsuarioMapper {
    private static readonly ILogger _logger = Log.ForContext(typeof(UsuarioMapper));

    public static User ToModel(this UpdateUserRequest dto) {
        _logger.Debug("[MAPP-UPDATE-MODEL] UpdateUserRequest a Model");
        return new User {
            Id = dto.Id,
            Name = dto.Name,
            UserName = dto.UserName,
            Email = dto.Email,
            Address = dto.Address.ToModel(),
            Phone = dto.Phone,
            Website = dto.Website,
            Company = dto.Company.ToModel()
        };
    }

    public static User ToModel(this CreateUserRequest dto) {
        _logger.Debug("[MAPP-CREATE-MODEL] CreateUserRequest a Model");
        return new User {
            Id = 0,
            Name = dto.Name,
            UserName = dto.UserName,
            Email = dto.Email,
            Address = dto.Address.ToModel(),
            Phone = dto.Phone,
            Website = dto.Website,
            Company = dto.Company.ToModel()
        };
    }

    public static CreateUserRequest ToCreateRequest(this User user) {
        _logger.Debug("[MAPP-MODEL-CREATE-DTO] User a CreateUserRequest");

        return new CreateUserRequest(
            user.Name,
            user.UserName,
            user.Email,
            user.Address.ToDto(),
            user.Phone,
            user.Website,
            user.Company.ToDto()
        );
    }

    public static UpdateUserRequest ToUpdateRequest(this User user) {
        _logger.Debug("[MAPP-MODEL-UPDATE-DTO] User a UpdateUserRequest");

        return new UpdateUserRequest(
            user.Id,
            user.Name,
            user.UserName,
            user.Email,
            user.Address.ToDto(),
            user.Phone,
            user.Website,
            user.Company.ToDto()
        );
    }
}