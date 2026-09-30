namespace RepositorioRemoto.Errors;

/// <summary>
///     Tipos de error de dominio
/// </summary>
public abstract record DomainErrors {
    /// <summary>
    ///     Error cuando no se encuentra un recurso
    /// </summary>
    public sealed record NotFound(string Resource, int Id) : DomainErrors;

    /// <summary>
    ///     Error de validación de datos
    /// </summary>
    public sealed record ValidationError(string Field, string Message) : DomainErrors;

    /// <summary>
    ///     Error de comunicacion con la API
    /// </summary>
    public sealed record ApiError(int StatusCode, string Detail) : DomainErrors;
}