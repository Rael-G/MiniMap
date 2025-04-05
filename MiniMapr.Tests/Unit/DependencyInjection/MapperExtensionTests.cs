using FluentAssertions;
using Microsoft.Extensions.DependencyInjection;
using Moq;

namespace MiniMapr.Tests.Unit.DependencyInjection;

public class MapperExtensionsTests
{
    [Fact]
    public void AddMapper_ShouldRegisterIMapper()
    {
        // Arrange
        var services = new ServiceCollection();

        services.AddMapper(cfg => cfg.Add<DummyConfig>());

        // Act
        var provider = services.BuildServiceProvider();
        var mapper = provider.GetRequiredService<IMapper>();

        // Assert
        mapper.Should().NotBeNull();
        mapper.Map<Person, PersonDto>(new Person { Name = "Alice" }).Name.Should().Be("Alice");
    }

    [Fact]
    public void AddMapper_ShouldApplyAllProvidedConfigs()
    {
        // Arrange
        var configMock = new Mock<IMapperConfig>();
        var services = new ServiceCollection();

        services.AddMapper(cfg => cfg.Add(configMock.Object));

        var provider = services.BuildServiceProvider();
        var mapper = provider.GetRequiredService<IMapper>();

        // Assert
        configMock.Verify(c => c.Configure(It.IsAny<MapperService>()), Times.Once);
    }
}