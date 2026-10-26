# Motorcycle Maintenance Tracker

## Overview

Motorcycle Maintenance Tracker is an ASP.NET Core MVC web application for motorcycle owners. It lets users register their motorcycles, record completed services, plan upcoming maintenance tasks and receive reminders when a task is about to be due.

The project was created for the SoftUni ASP.NET Fundamentals course.

## Features

- Home page: an overview with quick access to every section of the application.
- Motorcycles: create, view, edit and delete motorcycles. Deleting is a soft delete: the record stays in the database but is hidden from the application.
- Service records: create, view, edit and delete services (type, date, mileage, cost, notes). Each record belongs to a motorcycle.
- Maintenance tasks: create, view, edit and delete planned tasks with an optional due date and due mileage.
- Messages (reminders): reminders are generated automatically 7 days before, 1 day before and on the due date of a task. Messages can be dismissed or deleted.
- Validation: server-side and client-side validation on all forms.
- Responsive design: Bootstrap layout with a shared navigation bar and footer.

## Technologies

- C# and .NET 10
- ASP.NET Core MVC (Razor views)
- Entity Framework Core with SQL Server (LocalDB)
- Bootstrap 5 and Bootstrap Icons
- jQuery Validation (client-side validation)
- Git and GitHub

## Entity models

- Motorcycle: brand, model, year, mileage and a flag that marks it as deleted. One motorcycle has many service records.
- ServiceRecord: service type, date, mileage, cost and notes. Belongs to one motorcycle.
- MaintenanceTask: title, description, due date, due mileage, completion status and reminder flags. Linked to a motorcycle.
- Message: a reminder with a title, content, type and read/dismissed state. Linked to a maintenance task.

## Architecture

- MVC: models describe the data, controllers handle requests, and Razor views display the result.
- Dependency injection: ApplicationDbContext is registered in Program.cs and injected into each controller through its constructor.
- Layout, partial views and sections: every page uses the shared layout (Views/Shared/_Layout.cshtml). Forms add their validation scripts through a Scripts section and the _ValidationScriptsPartial partial view.

## Prerequisites

- .NET 10 SDK (https://dotnet.microsoft.com/download)
- SQL Server LocalDB (installed with Visual Studio, or with SQL Server Express)
- The EF Core command-line tool, installed with this command:

    dotnet tool install --global dotnet-ef

If it is already installed, skip this step.

## Setup

### 1. Clone the repository

    git clone https://github.com/fwtisreman/MotoMaintenanceTracker.git
    cd MotoMaintenanceTracker

Run the following commands from the directory containing MotoMaintenanceTracker.csproj.

### 2. Restore the packages

    dotnet restore

### 3. Check the connection string

The connection string is in appsettings.json, in the section ConnectionStrings, under the name DefaultConnection:

    Server=(localdb)\MSSQLLocalDB;Database=MotoMaintenanceTrackerDb;Trusted_Connection=True;MultipleActiveResultSets=true;TrustServerCertificate=True

It uses Windows authentication on LocalDB, so no username, password or environment variable is needed.

If you use a different SQL Server instance, change the Server part of the connection string, or override the setting with an environment variable (PowerShell):

    $env:ConnectionStrings__DefaultConnection = "Server=YOUR_SERVER;Database=MotoMaintenanceTrackerDb;Trusted_Connection=True;TrustServerCertificate=True"

### 4. Create the database

    dotnet ef database update

This applies the existing migrations and creates the database. In Visual Studio you can run Update-Database in the Package Manager Console instead.

### 5. Run the application

    dotnet run

Open the HTTPS address shown in the terminal. The database starts empty, so add a motorcycle first, then add service records and maintenance tasks for it.

Note: Bootstrap Icons are loaded from a CDN, so an internet connection is needed to see the icons on the home page.

## Validation

Server-side rules are defined with data annotations on the models (Required, StringLength, Range) and with extra checks in the controllers:

- Service dates cannot be in the future.
- Maintenance due dates cannot be in the past.
- A due mileage cannot be lower than the motorcycle's current mileage.
- A service record or task must belong to an existing motorcycle that has not been deleted.

Client-side validation uses jQuery Validation and the unobtrusive validation scripts, so most errors appear before the form is submitted. Forms also use the anti-forgery token, and controllers accept only the expected fields from each form.

## Project structure

    MotoMaintenanceTracker/
    |-- Controllers/     Home, Motorcycles, ServiceRecords, MaintenanceTasks, Messages
    |-- Data/            ApplicationDbContext
    |-- Migrations/      EF Core migrations
    |-- Models/          Entity models
    |-- Views/           Razor views, shared layout and partial views
    |-- wwwroot/         CSS, JavaScript, images and libraries
    |-- appsettings.json
    |-- Program.cs
    |-- MotoMaintenanceTracker.csproj

## Configuration and security

- No secrets are stored in the repository.
- The default configuration needs no credentials, because it uses Windows authentication on LocalDB.
- For other environments, set the connection string through an environment variable or user secrets instead of committing it.

## AI assistance

An AI assistant (Claude) was used during development for guidance, explanations, debugging, code review and help with the home page styling.