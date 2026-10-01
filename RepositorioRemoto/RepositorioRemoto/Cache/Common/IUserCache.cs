using RepositorioRemoto.Dto;

namespace RepositorioRemoto.Cache.Common;

public interface IUserCache {
    Task<User?> GetAsync(int id);

    Task AddAsync(User user);

    Task RemoveAsync(int id);

    Task ClearAsync();
}