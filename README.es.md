# Cook API

[![en](https://img.shields.io/badge/lang-en-blue)](https://github.com/luz-ojeda/cook-api/blob/main/README.md)

API en .NET que sirve como una plataforma sencilla y cómoda para gestionar recetas de cocina.

## Características

- Lectura de recetas.
- Funcionalidad de búsqueda: busque recetas por nombre, dificultad e ingredientes.
- Nivel de dificultad: asigna niveles de dificultad a las recetas.

## Tabla de contenidos

- [Setup](#Setup)
- [Requisitos previos](#requisitos-previos)
- [Ejecutando sin Docker](#ejecutando-la-api-sin-docker)
- [Ejecutando con Docker](#ejecutando-la-api-con-docker)
- [Documentación API](#api-documentation)
- [Colección cartero](#colección-cartero)
- [Pruebas](#pruebas)

## Setup

### Requisitos previos

- [SDK de .NET](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/download/) al menos. 9.6

Si instala con [Docker](https://www.docker.com/get-started)
- [Docker Compose](https://docs.docker.com/compose/install/)

### Ejecutando la API _sin_ Docker

1. Clona el repositorio:

    ```bash
    git clone https://github.com/luz-ojeda/cook-api.git

2. Navegue hasta el directorio raíz del proyecto.

    ```bash
    cd cook-api/CookApi

Dentro del archivo `appsettings.Development.json`, ubique la propiedad `ConnectionStrings` y actualice `DefaultConnection` con la connection string de su propia base de datos.

    ```json
    {
      "ConnectionStrings": {
        "DefaultConnection": "Host=localhost;Username=TuUsername;Password=TuPassword;Database=TuDB;"
      },
      //...
    }
    ```
3. Aplicar migraciones

    ```bash
    dotnet restore
    dotnet ef migrations add InitialMigrate
    dotnet ef database update
    ```

4. Ejecute la API

    ```bash
    dotnet run
    ```

### Ejecutando la API _con_ Docker

1. Clona el repositorio:

    ```bash
    git clone https://github.com/luz-ojeda/cook-api.git

2. Cree una carpeta `db` en el directorio raíz con su contraseña de PostgreSQL en un archivo `password.txt` dentro. Se define un [secreto de docker](https://docs.docker.com/engine/swarm/secrets/) con nombre `db-password` en el archivo `compose.yaml`, el servicio `db` obtiene la contraseña PSQL de este secreto.

3. Compile y ejecute con Docker Compose

    ```bash
   docker-compose up --build
   ```

    Este comando creará e iniciará dos contenedores Docker. Uno para la base de datos PostgreSQL y otro para la API como se especifica en el archivo `compose.yaml`. El indicador `--build` garantiza que Docker Compose cree las imágenes necesarias.

    La API está configurada para recibir solicitudes en el puerto localhost 8080.

### Notas adicionales

- **Scripts SQL para datos iniciales**: Dentro de `SqlScripts/` puede encontrar algunos scripts para insertar recetas e ingredientes de muestra iniciales en la base de datos si lo desea. Estos se ejecutan automáticamente en la aplicación en contenedor al iniciar la base de datos.

## Documentación API

Esta API está autodocumentada mediante Swagger. Con el proyecto corriendo, puede acceder a la documentación de la API navegando a:

`http://localhost:5255/swagger` _sin_ Docker o
`http://localhost:8080/swagger` con Docker

## Colección de Postman

Puede encontrar la colección Postman para esta API [aquí](https://api.postman.com/collections/12774422-fa74b2ab-72af-4313-bcfb-dadbd3c5a617?access_key=PMAT-01HPCDDQVQR6J5A9EVP1CWH25T).

Utiliza `http://localhost:5255` de forma predeterminada, por lo que si elige ejecutar la API con Docker, actualice la url en las solicitudes.

## Pruebas

Los tests se pueden encontrar en el proyecto `CookApi.Tests`.

Los tests de de integración utilizan en una base de datos PostgreSQL que se crea en tiempo de ejecución y se elimina después de que se hayan ejecutado todas las pruebas. La connection string para esta base de datos se utiliza en el archivo `CustomWebApplicationFactory.cs` del valor `Test` de `appsettings.Development.json`.

Para ejecutarlos:

1. cd en el directorio:

    ```bash
    cd cook-api\CookApi.Tests\
2. Ejecutar:
    ```bash
    dotnet test