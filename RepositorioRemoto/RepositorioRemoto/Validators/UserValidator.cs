using CSharpFunctionalExtensions;
using RepositorioRemoto.Errors;
using RepositorioRemoto.Models;

namespace RepositorioRemoto.Validators;

public class UserValidator : IUserValidator {
    public Result<User, DomainErrors> Validate(User user) {
        if (string.IsNullOrWhiteSpace(user.Name))
            return Result.Failure<User, DomainErrors>(UsersError.ValidationError(nameof(user.Name),
                "El nombre es obligatorio."));

        if (string.IsNullOrWhiteSpace(user.UserName))
            return Result.Failure<User, DomainErrors>(UsersError.ValidationError(nameof(user.UserName),
                "El nombre de usuario es obligatorio."));

        if (string.IsNullOrWhiteSpace(user.Email))
            return Result.Failure<User, DomainErrors>(UsersError.ValidationError(nameof(user.Email),
                "El email es obligatorio."));

        if (string.IsNullOrWhiteSpace(user.Address.Street))
            return Result.Failure<User, DomainErrors>(UsersError.ValidationError(nameof(user.Address.Street),
                "La calle es obligatoria."));

        if (string.IsNullOrWhiteSpace(user.Address.City))
            return Result.Failure<User, DomainErrors>(UsersError.ValidationError(nameof(user.Address.City),
                "La ciudad es obligatoria."));

        if (string.IsNullOrWhiteSpace(user.Phone))
            return Result.Failure<User, DomainErrors>(UsersError.ValidationError(nameof(user.Phone),
                "El teléfono es obligatorio."));

        if (string.IsNullOrWhiteSpace(user.Website))
            return Result.Failure<User, DomainErrors>(UsersError.ValidationError(nameof(user.Website),
                "El website es obligatorio."));

        if (string.IsNullOrWhiteSpace(user.Company.Name))
            return Result.Failure<User, DomainErrors>(UsersError.ValidationError(nameof(user.Company.Name),
                "El nombre de la empresa es obligatorio."));

        return Result.Success<User, DomainErrors>(user);
    }
}