namespace RepositorioRemoto.Models;

public record User(
    int Id,
    string Name,
    string UserName,
    string Email,
    Address Address,
    string Phone,
    string Website,
    Company Company
) {
    public User() : this(0, "", "", "", new Address(), "", "", new Company()) {
    }
}