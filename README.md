# Welcome to MiniMapr

[![NuGet](https://img.shields.io/nuget/v/MiniMapr.svg)](https://www.nuget.org/packages/MiniMapr)
[![License: MIT](https://img.shields.io/badge/License-MIT-yellow.svg)](https://github.com/Rael-G/MiniMapr/blob/main/LICENSE)


**MiniMapr** is a lightweight, high-performance object-to-object mapper for .NET applications.

It was designed to offer developers a clean and minimal API to define custom mapping rules without the complexity or overhead of traditional mappers. Whether you're working on a Web API, a background service, or a desktop app, MiniMapr provides a fast and extensible mapping solution.

---

## 🚀 Features

- ✨ Convention-based property mapping
- 🧩 Custom property name mapping
- 🔒 Property ignore support
- 🔁 Support for nested and complex type transformations
- 🔧 Clean separation of mapping logic via `IMapperConfig`
- 💨 High-performance through expression trees
- ✅ Simple DI registration and usage

---

## 📦 Install MiniMapr

You can get started by installing MiniMapr via NuGet:

```bash
dotnet add package MiniMapr
```

## Getting Started

### Instalation

Install MiniMapr via NuGet:

```bash
dotnet add package MiniMapr
```

### Defining Source and Target Classes
Let's assume you have a domain model and a DTO:

```csharp
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
To define how mapping should occur, implement IMapperConfig:

Example:
```csharp
using MiniMapr;

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
### Registering MiniMapr
To enable MiniMapr in your application, register your mapping configurations:

```csharp
var services = new ServiceCollection();

services.AddMapper(mapper => 
{ 
    mapper.Add<UserMappingConfig>()
        .Add<AddressMappingConfig>();
});

var serviceProvider = services.BuildServiceProvider();
var mapperService = serviceProvider.GetRequiredService<IMapper>();
```

### Performing a Mapping
Once registered, you can use the mapper as follows:

```csharp
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

## 📚 Documentation

- [Introduction](https://rael-g.github.io/MiniMapr/docs/introduction.html)  
  Understand the philosophy, goals, and high-level overview of how MiniMapr works.

- [Getting Started](https://rael-g.github.io/MiniMapr/docs/getting-started.html)  
  Learn how to install, configure, and use MiniMapr in your own projects.

- [API Reference](https://rael-g.github.io/MiniMapr/api/MiniMapr.html)
  Dive deep into the full API, including all public types, methods, and configuration options.

---

## 🧑‍💻 Contributing

We welcome contributions of all kinds — whether you're fixing bugs, adding features, writing documentation, or suggesting improvements.

To get started:

1. Fork the repository
2. Create a feature branch
3. Submit a pull request

---

## 📃 License

This project is licensed under the **MIT License**. You are free to use, modify, and distribute it as needed.

---

> If you find MiniMapr useful, consider starring the repository ⭐ to support the project!