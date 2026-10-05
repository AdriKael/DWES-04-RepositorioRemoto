using CSharpFunctionalExtensions;
using RepositorioRemoto.Errors;
using RepositorioRemoto.Models;

namespace RepositorioRemoto.Repositories;

public interface IUserRepository {
    Task<Result<User, DomainErrors>> GetByIdAsync(int id);

    Task<List<User>> GetAllAsync();

    Task<Result<User, DomainErrors>> CreateAsync(User user);

    Task<Result<User, DomainErrors>> UpdateAsync(int id, User user);

    Task<Result<User, DomainErrors>> DeleteAsync(int id);

    Task<Result<bool, DomainErrors>> DeleteAllAsync();
}