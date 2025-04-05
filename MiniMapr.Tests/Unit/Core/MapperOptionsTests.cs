using FluentAssertions;

namespace MiniMapr.Tests.Unit.Core;

public class MapperOptionsTests
{
    [Fact]
    public void AddCustomTransformation_ShouldStoreTransformation()
    {
        // Arrange
        var options = new MapperOptions();

        // Action
        options.AddCustomTransformation<int, string>("Age", age => $"Idade: {age}");

        // Assert
        options.CustomTransformations.Should().ContainKey("Age");

        var result = options.CustomTransformations["Age"](25);
        result.Should().Be($"Idade: {25}");
    }
}
