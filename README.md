# Motorcycle Maintenance Tracker

## Overview

Motorcycle Maintenance Tracker is an ASP.NET Core MVC web application designed to help motorcycle owners manage their motorcycles, record completed services, and track upcoming maintenance tasks.

The application uses Entity Framework Core and SQL Server LocalDB to store and manage data.

## Features

- **Motorcycle management:** Create, view, edit, and delete motorcycle records.
- **Service records:** Record maintenance work, including service type, date, mileage, cost, and notes.
- **Maintenance tasks:** Create, view, edit, and delete maintenance reminders.
- **Input validation:** Validate required fields, numeric ranges, string lengths, and service dates.
- **Database relationships:** Associate service records with the corresponding motorcycle.
- **Responsive interface:** Use Bootstrap styling and Razor views.

## Technologies

- C#
- .NET 10
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server LocalDB
- Razor Views
- Bootstrap
- Data Annotations for validation

## Prerequisites

Install the following before running the application:

- .NET 10 SDK
- SQL Server Express LocalDB
- Git (optional, for cloning the repository)

The Entity Framework Core command-line tool is required to apply migrations if it is not already installed.

Install it globally using:

```powershell
dotnet tool install --global dotnet-ef
```

If it is already installed, update it when necessary instead of installing it again.

## Getting Started

### 1. Clone the repository

```powershell
git clone https://github.com/fwtisreman/MotoMaintenanceTracker.git
cd MotoMaintenanceTracker
```

Open a terminal in the directory containing `MotoMaintenanceTracker.csproj`.

### 2. Restore dependencies

```powershell
dotnet restore
```

### 3. Configure the database

The default connection string is configured in `appsettings.json` and uses SQL Server LocalDB:

```text
Server=(localdb)\MSSQLLocalDB;Database=MotoMaintenanceTrackerDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True
```

Ensure SQL Server LocalDB is installed and available on your machine. If necessary, update the connection string for your local SQL Server instance.

### 4. Apply database migrations

```powershell
dotnet ef database update
```

This creates or updates the database schema using the existing Entity Framework Core migrations.

### 5. Build the application

```powershell
dotnet build
```

### 6. Run the application

```powershell
dotnet run
```

Open the local URL displayed in the terminal.

## Database Structure

The application contains three main entity models:

- **Motorcycle:** Stores the motorcycle's brand, model, year, and mileage.
- **ServiceRecord:** Stores service type, service date, mileage, cost, and optional notes. Each service record belongs to a motorcycle.
- **MaintenanceTask:** Stores maintenance task details, optional due dates and mileage, and completion status.

Entity Framework Core manages database access and schema migrations. The application uses dependency injection to provide the database context to its controllers.

## Validation

Server-side validation uses model validation attributes and custom validation logic.

Examples include:

- Required fields for essential information.
- String length restrictions.
- Valid ranges for years, mileage, and service costs.
- Prevention of future service dates.
- Validation feedback displayed in the Razor views.

Client-side validation is supported through the ASP.NET Core validation scripts included in the application.

## Configuration and Security

The database connection string is stored in `appsettings.json` for local development.

The default configuration uses Windows authentication through SQL Server LocalDB and does not require a separate database username or password.

For other environments, configure the appropriate connection string through the standard ASP.NET Core configuration system or environment variables. Do not commit passwords, access tokens, or other secrets to the repository.


## Project Structure

The project file is located in the innermost `MotoMaintenanceTracker` directory.

```text
MotoMaintenanceTracker/                 # Repository root (example)
└── MotoMaintenanceTracker/
    └── MotoMaintenanceTracker/
        ├── Controllers/
        ├── Data/
        ├── Migrations/
        ├── Models/
        ├── Views/
        ├── wwwroot/
        ├── appsettings.json
        ├── Program.cs
        └── MotoMaintenanceTracker.csproj
```

Run the `dotnet` commands from the directory containing `MotoMaintenanceTracker.csproj`.


## License

This project was created for educational purposes as part of the SoftUni ASP.NET Fundamentals course.
