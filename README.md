# Motorcycle Maintenance Tracker

## Overview

Motorcycle Maintenance Tracker is an ASP.NET Core MVC web application for managing motorcycles, recording service history, and tracking maintenance tasks.

## Features

- Add, view, edit, and delete motorcycles.
- Record motorcycle services, including service type, date, mileage, cost, and notes.
- Prevent service records from using a future date.
- Create, edit, view, and delete maintenance tasks.
- Track task due dates, due mileage, and completion status.
- Validate user input on the server and in the browser.

## Technologies

- C#
- ASP.NET Core MVC
- Entity Framework Core
- SQL Server LocalDB
- Razor Views
- Bootstrap

## Prerequisites

- .NET SDK compatible with the project target framework.
- Visual Studio or another compatible .NET development environment.
- SQL Server LocalDB, or a compatible SQL Server instance.

## Setup and Running

1. Clone or download this repository.
2. Open the solution in Visual Studio, or open a terminal in the directory containing the project file.
3. Restore NuGet packages:

   ```bash
   dotnet restore
   ```

4. Configure the database connection string in `appsettings.json` or through the appropriate configuration environment variable.
5. Apply the Entity Framework Core migrations:

   ```bash
   dotnet ef database update
   ```

6. Start the application:

   ```bash
   dotnet run
   ```

7. Open the local URL displayed in the terminal.

If the `dotnet ef` command is unavailable, install the EF Core command-line tool and ensure its version is compatible with the project's Entity Framework Core packages.

## Database Configuration

The application uses Entity Framework Core with SQL Server. The default local connection string is configured in `appsettings.json`.

For a different SQL Server instance, update the connection string to match that environment. Do not commit passwords, production credentials, or other secrets to source control.

## Main Sections

- **Motorcycles:** Manage motorcycle details.
- **Service Records:** Record and review maintenance history.
- **Maintenance Tasks:** Manage upcoming and completed maintenance.

## License

No license has been specified for this project.