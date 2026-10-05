namespace RepositorioRemoto.Errors;

public abstract record UserErrors(string Message) : DomainErrors(Message) {
    public sealed record NotFoundError(int Id)
        : UserErrors($"No se ha encontrado la entidad con el id: {Id}");

    public sealed record ValidationError(string Field, string Message)
        : UserErrors(Message);
}

public static class UsersError {
    public static DomainErrors NotFoundError(int Id) {
        return new UserErrors.NotFoundError(Id);
    }

    public static DomainErrors ValidationError(string Field, string Message) {
        return new UserErrors.ValidationError(Field, Message);
    }
}