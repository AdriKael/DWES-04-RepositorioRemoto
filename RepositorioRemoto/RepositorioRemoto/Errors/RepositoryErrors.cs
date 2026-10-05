namespace RepositorioRemoto.Errors;

public abstract record RepositoryErrors(string Message) : DomainErrors(Message) {
    public sealed record CreationError() : RepositoryErrors("No se ha podido registar el nuevo usuario en el sistema.");

    public sealed record UpdatedError() : RepositoryErrors("No se ha podido actualizar el usuario en el sistema.");

    public sealed record DeletedError() : RepositoryErrors("No se ha podido eliminar el usuario en el sistema.");
}

public static class RepositoryError {
    public static DomainErrors CreationError() {
        return new RepositoryErrors.CreationError();
    }

    public static DomainErrors UpdatedError() {
        return new RepositoryErrors.UpdatedError();
    }

    public static DomainErrors DeletedError() {
        return new RepositoryErrors.DeletedError();
    }
}