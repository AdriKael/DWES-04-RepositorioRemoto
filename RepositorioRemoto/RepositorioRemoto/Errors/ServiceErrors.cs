namespace RepositorioRemoto.Errors;

public abstract record ServiceErrors(string Message) : DomainErrors(Message) {
    public sealed record NotFoundError(int Id)
        : ServiceErrors($"No se ha encontrado el usuario con el id: {Id}");

    public sealed record GetAllError()
        : ServiceErrors("Error al obtener los usuarios.");

    public sealed record GetByIdError(int Id)
        : ServiceErrors($"Error al obtener el usuario con el id {Id}.");

    public sealed record CreateError()
        : ServiceErrors("Error al crear el usuario.");

    public sealed record UpdateError(int Id)
        : ServiceErrors($"Error al actualizar el usuario con el id {Id}.");

    public sealed record DeleteError(int Id)
        : ServiceErrors($"Error al eliminar el usuario con el id {Id}.");

    public sealed record ExportToJsonError(string Detail)
        : ServiceErrors($"Error al exportar los usuarios a JSON: {Detail}");
}

public static class ServiceError {
    public static DomainErrors NotFoundError(int id) {
        return new ServiceErrors.NotFoundError(id);
    }

    public static DomainErrors GetAllError() {
        return new ServiceErrors.GetAllError();
    }

    public static DomainErrors GetByIdError(int id) {
        return new ServiceErrors.GetByIdError(id);
    }

    public static DomainErrors CreateError() {
        return new ServiceErrors.CreateError();
    }

    public static DomainErrors UpdateError(int id) {
        return new ServiceErrors.UpdateError(id);
    }

    public static DomainErrors DeleteError(int id) {
        return new ServiceErrors.DeleteError(id);
    }

    public static DomainErrors ExportToJsonError(string detail) {
        return new ServiceErrors.ExportToJsonError(detail);
    }
}