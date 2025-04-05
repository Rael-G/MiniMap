using FluentAssertions;

namespace MiniMapr.Tests.Unit.DependencyInjection;

public class MapperConfiguratorTests
{
    [Fact]
    public void Add_Generic_ShouldAddConfig()
    {
        // Arrange
        var configurator = new MapperConfigurator();
        configurator.Add<DummyConfig>();

        // Action
        var mapper = configurator.Configure();
        var result = mapper.Map<Person, PersonDto>(new Person { Name = "Alice" });

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void Add_Type_ShouldAddConfig_WhenTypeIsValid()
    {
        // Arrange
        var configurator = new MapperConfigurator();
#pragma warning disable CA2263 // Prefer generic overload when type is known
        configurator.Add(typeof(DummyConfig));
#pragma warning restore CA2263 // Prefer generic overload when type is known

        // Action
        var mapper = configurator.Configure();
        var result = mapper.Map<Person, PersonDto>(new Person { Name = "Alice" });

        // Assert
        result.Should().NotBeNull();
    }

    [Fact]
    public void Add_Type_ShouldThrow_WhenTypeIsInvalid()
    {
        var configurator = new MapperConfigurator();

        var act = () => configurator.Add(typeof(InvalidConfig));
        act.Should().Throw<InvalidOperationException>()
           .WithMessage("*does not implement IMapperConfig");
    }

    [Fact]
    public void Add_Instance_ShouldAddConfigDirectly()
    {
        var config = new DummyConfig();
        var configurator = new MapperConfigurator();

        configurator.Add(config);
        var mapper = configurator.Configure();

        config.WasConfigured.Should().BeTrue();

        mapper.Map<string, string>("data").Should().Be("data");
    }
}

