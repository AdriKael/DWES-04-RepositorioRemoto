using CSharpFunctionalExtensions;
using RepositorioRemoto.Errors;
using RepositorioRemoto.Models;

namespace RepositorioRemoto.Storages;

public interface IUserStorage {
    Task<Result<bool, DomainErrors>> ExportarJsonAsync(IEnumerable<User> users, string path);
}