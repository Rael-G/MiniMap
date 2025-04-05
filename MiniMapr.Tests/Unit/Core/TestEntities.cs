namespace MiniMapr.Tests.Unit.Core;

public class User
{
    public string? Name { get; set; }
    public string? Email { get; set; }
    public string? Password { get; set; }
    public Address? Address { get; set; }
}

public class UserDto
{
    public string? Name { get; set; }
    public string? EmailAddress { get; set; }
    public string? Password { get; set; }
    public AddressDto? Address { get; set; }
}

public class Address
{
    public string? Street { get; set; }
}

public class AddressDto
{
    public string? Street { get; set; }
}

public class Book
{
    public string? Title { get; set; }
    public int Pages { get; set; }
}

public class BookDto
{
    public string? Title { get; set; }
    public int PagesCount { get; set; }
}

public class Item
{
    public string? Value { get; set; }
}

public class ItemDto
{
    public int Value { get; set; }
}

public class Person
{
    public string? Name { get; set; }
}

public class PersonDto
{
    public string? Name { get; set; }
}
