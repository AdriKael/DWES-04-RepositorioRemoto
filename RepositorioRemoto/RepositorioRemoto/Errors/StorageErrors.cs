namespace RepositorioRemoto.Errors;

public abstract record StorageErrors(string Message) : DomainErrors(Message) {
    public sealed record WriteError(string message) : StorageErrors("Error al intentar escribir el json.");
}

public static class StorageError {
    public static DomainErrors WriteError(string message) {
        return new StorageErrors.WriteError(message);
    }
}