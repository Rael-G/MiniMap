using FluentAssertions;
using MiniMapr.Core.Utils;

namespace MiniMapr.Tests.Unit.Core;

public class PropertyCacheTests
{
    private readonly PropertyCache _cache = new();

    [Fact]
    public void GetCachedProperties_ShouldReturnAllPublicInstanceProperties()
    {
        // Act
        var properties = _cache.GetCachedProperties(typeof(Student));

        // Assert
        properties.Should().ContainSingle(p => p.Name == "Name");
        properties.Should().ContainSingle(p => p.Name == "Age");
        properties.Should().OnlyContain(p => p.CanRead && p.CanWrite);
    }

    [Fact]
    public void GetCachedProperties_ShouldReturnCachedResultOnSecondCall()
    {
        // Act
        var props1 = _cache.GetCachedProperties(typeof(Student));
        var props2 = _cache.GetCachedProperties(typeof(Student));

        // Assert
        props1.Should().BeSameAs(props2);
    }

    [Fact]
    public void GetGetter_ShouldReturnCompiledGetterDelegate()
    {
        // Arrange
        var instance = new Student { Name = "TestName" };
        var prop = typeof(Student).GetProperty("Name")!;

        // Act
        var getter = _cache.GetGetter(prop);
        var value = getter(instance);

        // Assert
        value.Should().Be("TestName");
    }

    [Fact]
    public void GetSetter_ShouldReturnCompiledSetterDelegate()
    {
        // Arrange
        var instance = new Student();
        var prop = typeof(Student).GetProperty("Name")!;

        // Act
        var setter = _cache.GetSetter(prop);
        setter(instance, "UpdatedName");

        // Assert
        instance.Name.Should().Be("UpdatedName");
    }

    [Fact]
    public void GetGetter_ShouldReturnSameDelegateFromCache()
    {
        // Arrange
        var prop = typeof(Student).GetProperty("Age")!;

        // Act
        var getter1 = _cache.GetGetter(prop);
        var getter2 = _cache.GetGetter(prop);

        // Assert
        getter1.Should().BeSameAs(getter2);
    }

    [Fact]
    public void GetSetter_ShouldReturnSameDelegateFromCache()
    {
        // Arrange
        var prop = typeof(Student).GetProperty("Age")!;

        // Act
        var setter1 = _cache.GetSetter(prop);
        var setter2 = _cache.GetSetter(prop);

        // Assert
        setter1.Should().BeSameAs(setter2);
    }
}

