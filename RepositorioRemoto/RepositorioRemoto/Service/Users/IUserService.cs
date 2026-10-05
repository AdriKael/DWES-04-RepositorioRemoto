using CSharpFunctionalExtensions;
using RepositorioRemoto.Dto;
using RepositorioRemoto.Errors;
using RepositorioRemoto.Models;

namespace RepositorioRemoto.Service.Users;

public interface IUserService {
    Task<Result<IEnumerable<User>, DomainErrors>> GetAllAsync();
    Task<Result<User, DomainErrors>> GetByIdAsync(int id);
    Task<Result<User, DomainErrors>> CreateAsync(CreateUserRequest request);
    Task<Result<User, DomainErrors>> UpdateAsync(int id, UpdateUserRequest request);
    Task<Result<User, DomainErrors>> DeleteAsync(int id);
    Task<Result<bool, DomainErrors>> ExportToJsonAsync();
}