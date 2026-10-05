using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Json.Serialization;
using CSharpFunctionalExtensions;
using RepositorioRemoto.Errors;
using RepositorioRemoto.Models;
using Serilog;

namespace RepositorioRemoto.Storages;

public class UserStorage : IUserStorage {
    private readonly ILogger _logger = Log.ForContext<UserStorage>();

    private readonly JsonSerializerOptions _options = new() {
        WriteIndented = true,
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull,
        Converters = { new JsonStringEnumConverter() },
        Encoder = JavaScriptEncoder.UnsafeRelaxedJsonEscaping
    };

    public UserStorage() {
        _logger.Debug("[STORAGE-INIT] Inicializando almacenamiento de usuarios en JSON");
    }

    public async Task<Result<bool, DomainErrors>> ExportarJsonAsync(IEnumerable<User> items, string path) {
        _logger.Debug("[STORAGE-EXPORT] Exportando usuarios a JSON: {Path}", path);

        try {
            InitStorage(path);

            var json = JsonSerializer.Serialize(items.ToList(), _options);

            _logger.Debug("[STORAGE-EXPORT] JSON serializado correctamente. Escribiendo archivo");
            await File.WriteAllTextAsync(path, json, new UTF8Encoding(false));

            _logger.Information("[STORAGE-EXPORT] Usuarios exportados correctamente a {Path}", path);

            return Result.Success<bool, DomainErrors>(true);
        }
        catch (Exception ex) {
            _logger.Error(ex, "[STORAGE-EXPORT] Error al exportar los usuarios a {Path}", path);
            return Result.Failure<bool, DomainErrors>(StorageError.WriteError(ex.Message));
        }
    }

    private void InitStorage(string path) {
        var directory = Path.GetDirectoryName(path);

        if (string.IsNullOrEmpty(directory)) {
            _logger.Debug("[STORAGE-INIT] La ruta no contiene un directorio que crear");
            return;
        }

        if (Directory.Exists(directory)) {
            _logger.Debug("[STORAGE-INIT] El directorio ya existe: {Directory}", directory);
            return;
        }

        _logger.Debug("[STORAGE-INIT] Creando directorio de almacenamiento: {Directory}", directory);
        Directory.CreateDirectory(directory);
    }
}