# 🚀 TaskFlow API - Gestión de Tareas con Azure SQL

Este proyecto es una API profesional desarrollada en **.NET 9** para la gestión 
de tareas y categorías, conectada a una base de datos en la nube.

## 🛠️ Tecnologías utilizadas
* **Lenguaje:** C#
* **Framework:** ASP.NET Core Web API
* **Base de Datos:** Azure SQL Server (Cloud)
* **ORM:** Entity Framework Core (Database First/Code First)
* **Documentación:** Scalar / Swagger

## 🏗️ Arquitectura del Proyecto
El proyecto sigue un patrón de **Inyección de Dependencias** y separación de responsabilidades:
* **Controllers:** Puerta de entrada de las peticiones HTTP.
* **Services:** Lógica de negocio (donde ocurre la magia).
* **Models/Entities:** Representación de las tablas de SQL.
* **DTOs (Data Transfer Objects):** Para enviar solo la información necesaria al cliente.

## 📁 Endpoints Principales
### Categorías
* `GET /api/categorias` - Lista todas las categorías.
* `POST /api/categorias` - Crea una nueva categoría.
* `DELETE /api/categorias/{id}` - Elimina una categoría (Borrado físico).

### Tareas
* `GET /api/mis-tareas/categoria/{id}` - Filtra tareas por categoría (usando LINQ y Lambdas).

## 🚀 Cómo ejecutarlo
1. Clona el repositorio.
2. Configura tu `Connection String` de Azure en `appsettings.json`.
3. Ejecuta `dotnet run` o pulsa F5 en Visual Studio.
