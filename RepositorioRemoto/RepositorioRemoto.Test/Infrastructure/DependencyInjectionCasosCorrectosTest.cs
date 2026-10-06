using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using RepositorioRemoto.Cache.Common;
using RepositorioRemoto.Config;
using RepositorioRemoto.Infrastructure;
using RepositorioRemoto.Service.Notifications;
using RepositorioRemoto.Service.Synchro;
using RepositorioRemoto.Service.Users;
using RepositorioRemoto.Storages;
using RepositorioRemoto.Validators;

namespace RepositorioRemoto.Test;

[TestFixture]
public class DependencyInjectionCasosCorrectosTest {
    [Test]
    public void BuildServiceProvider_PerfilDev_ResuelveServiciosPrincipales() {
        // Arrange
        AppConfig.Configure("dev");

        // Act
        using var provider = (ServiceProvider)InyectorDependencias.BuildServiceProvider();
        using var scope = provider.CreateScope();
        var validator = scope.ServiceProvider.GetService<IUserValidator>();
        var storage = scope.ServiceProvider.GetService<IUserStorage>();
        var cache = scope.ServiceProvider.GetService<IUserCache>();
        var notifications = scope.ServiceProvider.GetService<INotificationService>();
        var userService = scope.ServiceProvider.GetService<IUserService>();
        var synchro = scope.ServiceProvider.GetService<SynchroService>();

        // Assert
        validator.Should().NotBeNull();
        storage.Should().NotBeNull();
        cache.Should().NotBeNull();
        notifications.Should().NotBeNull();
        userService.Should().NotBeNull();
        synchro.Should().NotBeNull();
    }

    [Test]
    public void BuildServiceProvider_DosScopes_ComparteSingletonYSeparaScoped() {
        // Arrange
        AppConfig.Configure("dev");
        using var provider = (ServiceProvider)InyectorDependencias.BuildServiceProvider();
        using var firstScope = provider.CreateScope();
        using var secondScope = provider.CreateScope();

        // Act
        var firstNotifications = firstScope.ServiceProvider.GetRequiredService<INotificationService>();
        var secondNotifications = secondScope.ServiceProvider.GetRequiredService<INotificationService>();
        var firstService = firstScope.ServiceProvider.GetRequiredService<IUserService>();
        var secondService = secondScope.ServiceProvider.GetRequiredService<IUserService>();

        // Assert
        firstNotifications.Should().BeSameAs(secondNotifications);
        firstService.Should().NotBeSameAs(secondService);
    }
}
