using Moq;
using RepositorioRemoto.Api;
using RepositorioRemoto.Cache.Common;
using RepositorioRemoto.Dto;
using RepositorioRemoto.Dto.User;
using RepositorioRemoto.Models;
using RepositorioRemoto.Repositories;
using RepositorioRemoto.Service.Notifications;
using RepositorioRemoto.Service.Users;
using RepositorioRemoto.Storages;
using RepositorioRemoto.Validators;

namespace RepositorioRemoto.Test;

public abstract class UserServiceTestBase {
    protected Mock<IJsonPlaceholderApi> Api = null!;
    protected Mock<IUserCache> Cache = null!;
    protected Mock<INotificationService> Notifications = null!;
    protected Mock<IUserRepository> Repository = null!;
    protected Mock<IUserStorage> Storage = null!;
    protected Mock<IUserValidator> Validator = null!;
    protected UserService Service = null!;

    [SetUp]
    public void SetUp() {
        Api = new Mock<IJsonPlaceholderApi>();
        Cache = new Mock<IUserCache>();
        Notifications = new Mock<INotificationService>();
        Repository = new Mock<IUserRepository>();
        Storage = new Mock<IUserStorage>();
        Validator = new Mock<IUserValidator>();
        Service = new UserService(Validator.Object, Repository.Object, Cache.Object,
            Storage.Object, Notifications.Object, Api.Object);
    }

    protected static User CreateUser(int id) => new(id, $"Name{id}", $"user{id}", $"user{id}@test.com",
        new Address("Street", "Suite", "City", "00000", new Geo("0", "0")), "555", "site", new Company("Company", "phrase", "bs"));

    protected static CreateUserRequest CreateRequest() => new("Name", "user", "user@test.com",
        new AddressDto("Street", "Suite", "City", "00000", new GeoDto("0", "0")), "555", "site", new CompanyDto("Company", "phrase", "bs"));

    protected static UpdateUserRequest CreateUpdateRequest(int id) => new(id, "Name", "user", "user@test.com",
        new AddressDto("Street", "Suite", "City", "00000", new GeoDto("0", "0")), "555", "site", new CompanyDto("Company", "phrase", "bs"));
}
