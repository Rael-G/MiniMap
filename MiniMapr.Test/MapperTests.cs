using FluentAssertions;

namespace MiniMapr.Tests;

public class MapperTests
{
    [Fact]
    public void Map_Should_Map_Properties_Correctly_When_No_Custom_Configuration()
    {
        // Arrange
        // No custom mappings or transformations defined.
        var mapper = new Mapper<Book, BookDto>();

        var book = new Book
        {
            Title = "The Fellowship of the Ring",
            Pages = 423
        };

        // Act
        var result = mapper.Map(book);

        // Assert
        result.Should().NotBeNull();
        result.Title.Should().Be(book.Title);
        result.PagesCount.Should().Be(0);
    }

     [Fact]
    public void Map_Should_Ignore_Property_When_Configured()
    {
        // Arrange
        var options = new MapperOptions();
        // Ignore the 'Password' property
        options.IgnoredProperties.Add("Password");
        var mapper = new Mapper<User, UserDto>(options);

        var user = new User
        {
            Name = "Alice",
            Email = "alice@example.com",
            Password = "secret"
        };

        // Act
        var result = mapper.Map(user);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        // Since no custom mapping for Email exists, EmailAddress remains null
        result.EmailAddress.Should().BeNull();
        result.Password.Should().BeNull(); // Password is ignored
    }

    [Fact]
    public void Map_Should_Apply_Custom_Mappings()
    {
        // Arrange
        var options = new MapperOptions();
        // Map 'Email' in the source to 'EmailAddress' in the destination
        options.CustomMappings["Email"] = "EmailAddress";
        var mapper = new Mapper<User, UserDto>(options);

        var user = new User
        {
            Name = "Alice",
            Email = "alice@example.com",
            Password = "secret"
        };

        // Act
        var result = mapper.Map(user);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.EmailAddress.Should().Be(user.Email); // Custom mapping applied
        result.Password.Should().Be(user.Password); // Unmapped, remains null
    }

    [Fact]
    public void Map_Should_Apply_Custom_Transformations()
    {
        // Arrange
        var options = new MapperOptions();
        // Prepare a mapper for AddressTest to AddressDtoTest (default mapping: property names match)
        var addressMapper = new Mapper<Address, AddressDto>();
        // Add a custom transformation for 'Address' to map nested objects
        options.AddCustomTransformation<Address, AddressDto>("Address", address => addressMapper.Map(address));
        var mapper = new Mapper<User, UserDto>(options);

        var user = new User
        {
            Name = "Alice",
            Address = new Address { Street = "123 Maple St" }
        };

        // Act
        var result = mapper.Map(user);

        // Assert
        result.Should().NotBeNull();
        result.Name.Should().Be(user.Name);
        result.Address.Should().NotBeNull();
        result.Address.Street.Should().Be(user.Address.Street);
    }

    [Fact] public void Map_Should_Throw_MappingConversionException_For_Incompatible_Types() 
    { 
        // Arrange 
        var mapper = new Mapper<Item, ItemDto>();
        // Provide a string that cannot be converted to int.
        var source = new Item { Value = "NotANumber" };

        // Act
        Action act = () => mapper.Map(source);

        // Assert
        act.Should().Throw<MappingConversionException>()
            .Where(ex => ex.Message.Contains(nameof(source.Value)) &&
                        ex.Message.Contains(nameof(Item)) &&
                        ex.Message.Contains(source.Value.GetType().Name));
    }
}