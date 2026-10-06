using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using RepositorioRemoto.Api;
using RepositorioRemoto.Cache.Common;
using RepositorioRemoto.Models;
using RepositorioRemoto.Repositories;
using RepositorioRemoto.Service.Synchro;

namespace RepositorioRemoto.Test;

[TestFixture]
public class SynchroServiceCasosIncorrectosTest {
    [Test]
    public async Task SynchronizeOnceAsync_FallaLaCache_PropagaExcepcion() {
        // Arrange
        var cache = new Mock<IUserCache>();
        cache.Setup(x => x.ClearAsync()).ThrowsAsync(new InvalidOperationException("cache"));
        using var provider = CreateProvider(new Mock<IUserRepository>(), new Mock<IJsonPlaceholderApi>());
        var service = new SynchroService(provider, cache.Object);

        // Act
        var action = () => service.SynchronizeOnceAsync();

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage("cache");
    }

    [Test]
    public async Task SynchronizeOnceAsync_FallaLaApi_PropagaExcepcion() {
        // Arrange
        var cache = new Mock<IUserCache>();
        var repository = new Mock<IUserRepository>();
        var api = new Mock<IJsonPlaceholderApi>();
        api.Setup(x => x.GetUsuariosAsync()).ThrowsAsync(new InvalidOperationException("api"));
        using var provider = CreateProvider(repository, api);
        var service = new SynchroService(provider, cache.Object);

        // Act
        var action = () => service.SynchronizeOnceAsync();

        // Assert
        await action.Should().ThrowAsync<InvalidOperationException>().WithMessage("api");
    }

    private static ServiceProvider CreateProvider(Mock<IUserRepository> repository, Mock<IJsonPlaceholderApi> api) =>
        new ServiceCollection()
            .AddSingleton(repository.Object)
            .AddSingleton(api.Object)
            .BuildServiceProvider();
}
