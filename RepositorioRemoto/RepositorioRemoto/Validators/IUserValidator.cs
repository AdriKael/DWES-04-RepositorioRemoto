using CSharpFunctionalExtensions;
using RepositorioRemoto.Errors;
using RepositorioRemoto.Models;

namespace RepositorioRemoto.Validators;

public interface IUserValidator {
    Result<User, DomainErrors> Validate(User user);
}