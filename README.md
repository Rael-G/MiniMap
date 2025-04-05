# Using MiniMap for Object Mapping in .NET

### Defining the Source and Target Classes
Suppose you have the following domain and DTO classes:

```
public class User
{
    public string Name { get; set; }
    public string Email { get; set; }
    public string Password { get; set; }
    public Address Address { get; set; }
}

public class UserDto
{
    public string Name { get; set; }
    public string EmailAddress { get; set; }
    public string Password { get; set; } = "";
    public AddressDto Address { get; set; }
}

public class Address
{
    public string Street { get; set; }
}

public class AddressDto
{
    public string Street { get; set; }
}
```

### Creating Mapping Configuration Classes
To define mappings, create classes that implement IMapperConfig. Inside the Configure method, register your mappings using the MapperService instance provided.

Example:
```
using MiniMap;

public class UserMappingConfig : IMapperConfig 
{ 
    public void Configure(MapperService mapper) 
    { 
        mapper.Add<User, UserDto>(config => 
        { 
            config.IgnoredProperties.Add("Password"); 
            config.CustomMappings["Email"] = "EmailAddress"; 
            config.AddCustomTransformation<Address, AddressDto>("Address", address =>
                mapper.Map<Address, AddressDto>(address));
        });
    }
}

public class AddressMappingConfig : IMapperConfig 
{ 
    public void Configure(MapperService mapper) 
    { 
        mapper.Add<Address, AddressDto>(); 
    } 
}
```
### Registering MiniMap in Dependency Injection
To use MiniMap in a .NET application, register the configuration classes during startup using AddMapper:

```
using MiniMap;
using Microsoft.Extensions.DependencyInjection;

var services = new ServiceCollection();

services.AddMapper(mapper => 
{ 
    mapper.Add<UserMappingConfig>()
        .Add<AddressMappingConfig>();
});

var serviceProvider = services.BuildServiceProvider();
var mapperService = serviceProvider.GetRequiredService<IMapper>();
```

Using the Mapper
Once registered, you can use the mapper as follows:

```
var user = new User
{
    Name = "Alice",
    Email = "alice@email.com",
    Password = "secret123",
    Address = new Address { Street = "123 Maple St" }
};

var userDto = mapperService.Map<User, UserDto>(user);

Console.WriteLine(userDto.Name); // Alice
Console.WriteLine(userDto.EmailAddress); // alice@email.com
Console.WriteLine(userDto.Password); // (empty)
Console.WriteLine(userDto.Address.Street); // 123 Maple St
```