<<<<<<< HEAD
using RepositorioRemoto.Dto;
=======
using RepositorioRemoto.Models;
>>>>>>> feat/cache

namespace RepositorioRemoto.Cache.Common;

public interface IUserCache {
    Task<User?> GetAsync(int id);

    Task AddAsync(User user);

    Task RemoveAsync(int id);

    Task ClearAsync();
}