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
  - [Running the API](#running-the-api)
- [Usage](#usage)
- [API Documentation](#api-documentation)
- [Testing](#testing)
- [Contributing](#contributing)
- [License](#license)

## Getting Started

### Prerequisites

- [.NET SDK](https://dotnet.microsoft.com/download)
- PostgreSQL at least. 9.6 (if setting up a local database)

### Installation

#### Local database setup (optional)

1. Install PostgreSQL: Download and install PostgreSQL from the [official PostgreSQL website](https://www.postgresql.org/download/). Ensure that the version is at least 9.6
2. Inside the `scripts` folder execute:
   1. `create_types.sql`
   2. `create_tables.sql`
   3. `insert_recipes.sql`

#### API
1. Clone the repository:

   ```bash
   git clone https://github.com/luz-ojeda/cook-api.git
   cd cook-api

### Running the API

## Usage

## API Documentation

This API is self-documented using Swagger. With the project running, you can access the API documentation by navigating to:

`http://localhost:5000/swagger`

## Testing
