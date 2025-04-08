# Introduction

**MiniMapr** is a lightweight, convention-based object mapper for .NET. It helps you map between objects—typically domain models and DTOs—using a minimal configuration setup and high performance under the hood.

Instead of relying on reflection at runtime, MiniMapr leverages expression trees and compile-time optimizations to generate fast mappers that can be customized via an intuitive API.

It supports:

- Property-to-property mapping with matching names
- Custom property mappings
- Property ignoring
- Transformation logic for complex or nested types
- Dependency injection configuration
- Separation of mapping logic into clean, reusable `IMapperConfig` classes

Whether you are mapping models in a Web API, transforming entities between layers, or just avoiding repetitive code, **MiniMapr** offers a clean, extensible solution for .NET developers.
