# DevInsightForge Web API Template

<!-- ![DevInsightForge Logo](<URL to Logo if applicable>) -->

## Overview

This template provides a foundation for building a DevInsightForge Web API project using C#.

- **Identity**: DevInsightForge.Templates.Api
- **License**: [MIT](https://opensource.org/licenses/MIT)

## Description

This template serves as a starting point for a DevInsightForge Web API project. It includes essential structures and configurations to kickstart your development process.

## Features

- ASP.NET Core Web API project
- Clean architecture pattern
- EFCore with SqlServer
- Domain Driven Design
- Repository pattern
- JWT Authentication
- Fluent Validation
- Mapster

## Prerequisites

- [.NET SDK 8.0+](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Microsoft Visual Studio 2022 - Version 17.8.0+](https://visualstudio.microsoft.com/downloads)

## Installation

To install the DevInsightForge Web API Template, use the following command:

```bash
dotnet new install DevInsightForge.Templates.Api
```

## Create Solution Using Template

To create a new solution using the DevInsightForge Web API Template, use the following command:

```bash
dotnet new devforgeapi -n YourSolutionNameHere
```

## Manage Migrations

To execute migration commands, select [YourProjectName].Infrastructure project in Package Manager Console first.

Run this to apply migrations first time.

```bash
Update-Database
```

If runs into any error even with infra as selected project, remove Migration directory from Infrastructure and execute add new migration command.

```bash
Add-Migration [Your-Migration-Name]
```
