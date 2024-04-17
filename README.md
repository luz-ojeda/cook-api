# Cook API

[![es](https://img.shields.io/badge/lang-es-red)](https://github.com/luz-ojeda/cook-api/blob/main/README.es.md)

.NET-based application that serves as a platform for managing and retrieving cooking recipes.

A live version of a web app using the API can be found [here](https://cook-web-weathered-thunder-7639.fly.dev/) and the repository for the front end [here](https://github.com/luz-ojeda/cook-web).

## Features

- Recipe management: Read and delete operations for recipes (update, delete and creation work in progress).
- Search Functionality: Search for recipes by name, difficulty, only vegetarians and ingredients.
- Difficulty Level: Assign difficulty levels to recipes.

## Table of Contents

- [Getting Started](#getting-started)
- [Prerequisites](#prerequisites)
- [Running without Docker](#running-the-api-without-docker)
- [Running with Docker](#running-the-api-with-docker)
- [API Documentation](#api-documentation)
- [Postman Collection](#postman-collection)
- [Testing](#testing)

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/) at least. 9.6

If installing with Docker:
- [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

### Running the API without Docker

1. Clone the repository:

   ```bash
   git clone https://github.com/luz-ojeda/cook-api.git

2. Navigate to the project's root directory

   ```bash
   cd cook-api/CookApi

Inside the `appsettings.Development.json` file, locate the `ConnectionStrings` property and update `DefaultConnection` with your database's own connection string.

   ```json
   {
     "ConnectionStrings": {
       "DefaultConnection": "Host=localhost;Username=YourUsername;Password=YourPassword;Database=YourDatabase;"
     },
     // ...
   }
   ```
3. Apply migrations

   ```bash
   dotnet restore
   dotnet ef migrations add InitialMigrate
   dotnet ef database update
   ```

4. Run the API

   ```bash
   dotnet run
   ```

### Running the API with Docker

1. Clone the repository:

   ```bash
   git clone https://github.com/luz-ojeda/cook-api.git

2. Create a `db` folder in root with your PostgreSQL password in a `password.txt` file inside. A `db-password` [secret](https://docs.docker.com/engine/swarm/secrets/) is defined in the `compose.yaml` file, the db service will retrieve the PSQL password from this secret.

3. Build and run with Docker Compose

   ```bash
   docker-compose up --build
   ```

   This command will create and start two Docker containers. One for the PostgreSQL database and another one for the API as specified in the `compose.yaml` file. The `--build` flag ensures that Docker Compose builds the necessary images.

   The API is configured to receive requests on localhost port 8080.

### Additional Notes

- **SQL Scripts for Initial Data**: Inside `SqlScripts/` you can find some scripts to insert initial sample recipes and ingredients into the database if you desire. These are run automatically in the containerized application on the db startup.

## API Documentation

This API is self-documented using Swagger. With the project running, you can access the API documentation by navigating to:

`http://localhost:5255/swagger` without Docker or
`http://localhost:8080/swagger` with Docker

## Postman Collection

There is a Postman collection available, though it is for local testing only. URL variable can be found in the collection scope set to `http://localhost:5255`, the .NET default url when running with `dotnet run`

[<img src="https://run.pstmn.io/button.svg" alt="Run In Postman" style="width: 128px; height: 32px;">](https://app.getpostman.com/run-collection/12774422-fa74b2ab-72af-4313-bcfb-dadbd3c5a617?action=collection%2Ffork&source=rip_markdown&collection-url=entityId%3D12774422-fa74b2ab-72af-4313-bcfb-dadbd3c5a617%26entityType%3Dcollection%26workspaceId%3D7d19834a-2f61-4ab3-b03b-dfc0aeccd911)

## Testing

Tests can be found in the `CookApi.Tests` project.

Integration tests rely on a PostgreSQL database that is created on runtime and deleted after all tests have run. The connection string for this database is retrieved in the `CustomWebApplicationFactory.cs` file from `appsettings.Development.json` `Test` value.

To run them:

1. cd into directory:

   ```bash
   cd cook-api\CookApi.Tests\
2. Run:
   ```bash
   dotnet test
