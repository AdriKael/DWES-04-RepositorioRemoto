using System.Text.Json;
using RepositorioRemoto.Dto.Users;
using RepositorioRemoto.Models;
using Serilog;

namespace RepositorioRemoto.Mappers;

public static class CompanyMapper {
    private static readonly ILogger _logger = Log.ForContext(typeof(CompanyMapper));

    private static readonly JsonSerializerOptions _jsonOptions = new() {
        PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        WriteIndented = false
    };

    private static Company DefaultCompany => new(
        string.Empty,
        string.Empty,
        string.Empty
    );

    public static string ToJson(this Company? company) {
        _logger.Debug("[MAPP-MODEL-JSON] Company a JSON");
        return company is null ? string.Empty : JsonSerializer.Serialize(company, _jsonOptions);
    }

    public static Company ToCompany(this string? json) {
        _logger.Debug("[MAPP-JSON-MODEL] JSON a Company");
        if (string.IsNullOrWhiteSpace(json)) return DefaultCompany;

        try {
            var result = JsonSerializer.Deserialize<Company>(json, _jsonOptions);
            return result ?? DefaultCompany;
        }
        catch (JsonException) {
            return DefaultCompany;
        }
    }

    public static Company ToModel(this CompanyDto dto) {
        _logger.Debug("[MAPP-DTO-MODEL] CompanyDto a Model");
        return new Company(
            dto.Name,
            dto.CatchPhrase,
            dto.Bs
        );
    }

    public static CompanyDto ToDto(this Company company) {
        _logger.Debug("[MAPP-MODEL-DTO] Company a CompanyDto");

        return new CompanyDto(
            company.Name,
            company.CatchPhrase,
            company.Bs
        );
    }
}