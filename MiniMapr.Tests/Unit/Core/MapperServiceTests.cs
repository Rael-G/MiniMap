using FluentAssertions;
using Moq;

namespace MiniMapr.Tests.Unit.Core;

public class MapperServiceTests
{
    [Fact]
    public void Add_CustomMapper_ShouldMapCorrectly()
    {
        // Arrange
        var source = new Person { Name = "Test" };
        var expected = new PersonDto { Name = "Test" };

        var mapperMock = new Mock<ITypeMapper<Person, PersonDto>>();
        mapperMock.Setup(m => m.Map(source)).Returns(expected);

        var service = new MapperService();
        service.Add(mapperMock.Object);

        // Act
        var result = service.Map<Person, PersonDto>(source);

        // Assert
        result.Should().BeSameAs(expected);
        mapperMock.Verify(m => m.Map(source), Times.Once);
    }

    [Fact]
    public void Map_WithoutRegisteredMapper_ShouldThrowInvalidOperationException()
    {
        // Arrange
        var service = new MapperService();
        var source = new Person();

        // Act
        Action act = () => service.Map<Person, PersonDto>(source);

        // Assert
        act.Should().Throw<InvalidOperationException>()
            .WithMessage($"*{typeof(Person)}*{typeof(PersonDto)}*");
    }

    [Fact]
    public void Add_GenericMapper_ShouldMapUsingDefaultMapper()
    {
        // Arrange
        var source = new Person { Name = "Auto" };
        var service = new MapperService();
        service.Add<Person, PersonDto>();

        // Act
        var result = service.Map<Person, PersonDto>(source);

        // Assert
        result.Name.Should().Be(source.Name);
    }

    [Fact]
    public void Add_WithOptions_ShouldConfigureAndMapCorrectly()
    {
        // Arrange
        var source = new Person { Name = "OptionTest" };
        var service = new MapperService();

        service.Add<Person, PersonDto>(options =>
        {
            options.IgnoredProperties.Add("Name");
        });

        // Act
        var result = service.Map<Person, PersonDto>(source);

        // Assert
        result.Name.Should().BeNull();
    }

    [Fact]
    public void Map_ToExistingInstance_ShouldUseMapperCorrectly()
    {
        // Arrange
        var source = new Person { Name = "Existing" };
        var destination = new PersonDto();

        var expected = new PersonDto { Name = "Existing" };

        var mapperMock = new Mock<ITypeMapper<Person, PersonDto>>();
        mapperMock.Setup(m => m.Map(source, destination)).Returns(expected);

        var service = new MapperService();
        service.Add(mapperMock.Object);

        // Act
        var result = service.Map(source, destination);

        // Assert
        result.Should().BeSameAs(expected);
        mapperMock.Verify(m => m.Map(source, destination), Times.Once);
    }
}


