using Refit;
using RepositorioRemoto.Dto;
using RepositorioRemoto.Models;

namespace RepositorioRemoto.Api;

/// <summary>
///     Interfaz que define los endpoints de JSONPlaceholder.
///     Refit genera la implementación en tiempo de compilación
///     a partir de los atributos [Get], [Post], [Put], [Delete].
/// </summary>
[Headers("Content-Type: application/json")]
public interface IJsonPlaceholderApi {
    /// <summary>
    ///     GET /users - Obtiene todos los usuarios.
    /// </summary>
    [Get("/users")]
    Task<List<User>> GetUsuariosAsync();

    /// <summary>
    ///     GET /users/{id} - Obtiene un usuario por su ID.
    /// </summary>
    [Get("/users/{id}")]
    Task<User?> GetUsuarioByIdAsync(int id);

    /// <summary>
    ///     POST /users - Crea un nuevo usuario.
    ///     El servidor asigna el Id automáticamente.
    /// </summary>
    [Post("/users")]
    Task<User> CreateUsuarioAsync([Body] CreateUserRequest request);

    /// <summary>
    ///     PUT /users/{id} - Actualiza completamente un usuario.
    /// </summary>
    [Put("/users/{id}")]
    Task<User> UpdateUsuarioAsync(int id, [Body] UpdateUserRequest request);

    /// <summary>
    ///     DELETE /users/{id} - Elimina un usuario.
    /// </summary>
    [Delete("/users/{id}")]
    Task DeleteUsuarioAsync(int id);
}