# Cook API

.NET-based application that serves as a simple and convenient platform for managing and retrieving cooking recipes.

## Features

- Recipe Management: Create, Read, Update, and Delete (CRUD) operations for recipes.
- Search Functionality: Search for recipes by name, difficulty, tags, and ingredients.
- Substitution Suggestions: Receive recommendations for ingredient substitutions.
- Difficulty Level: Assign difficulty levels to recipes.
- Tags and Categories: Categorize recipes with tags for easy organization.
- Ingredients Scaling: Easily convert recipes to different serving sizes.
- Copy-Paste Functionality: Seamlessly copy and paste ingredients and instructions.

## Table of Contents

- [Getting Started](#getting-started)
  - [Prerequisites](#prerequisites)
  - [Installation](#installation)
- [Usage](#usage)
- [API Documentation](#api-documentation)
- [Testing](#testing)

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/) at least. 9.6

### Installation

1. Clone the repository:

   ```bash
   git clone https://github.com/luz-ojeda/cook-api.git

2. Navigate to the project's root directory

   ```bash
   cd cook-api/CookApi

Inside the `appsettings.Development.json` file, locate the `ConnectionStrings` property and update `CookDB` with your database credentials (or use the default string making sure a database named `cook` exists).

   ```json
   {
     "ConnectionStrings": {
       "CookDB": "Host=localhost;Username=YourUsername;Password=YourPassword;Database=YourDatabase;"
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

### Additional Notes

- **SQL Scripts for Initial Data**: Inside `scripts/` you can find some scripts ((`insert_recipes.sql`, `insert_bbcfood_recipes_spanish.sql` and `insert_ingredients.sql`)) to insert initial sample recipes and ingredients into the database.

## Usage

## API Documentation

This API is self-documented using Swagger. With the project running, you can access the API documentation by navigating to:

`http://localhost:5000/swagger`

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