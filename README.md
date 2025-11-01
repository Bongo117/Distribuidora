# Proyecto: Distribuidora
Proyecto realizado para la materia Laboratorio de Programación II 
Se trata de un sistema de gestión de pedidos, stock, clientes y proveedores para una distribuidora mayorista.

## Tecnológias

* **Backend:** ASP.NET Core 8.0 MVC
* **Base de Datos:** SQLite (Entity Framework Core 8)
* **Frontend (MVC):** Vistas Razor con Bootstrap
* **Frontend (CRUD):** Vue.js 3 (para el ABM de Productos)
* **Seguridad:** ASP.NET Core Identity (con Roles)
* **API:** Web API con autenticación JWT

## Usuarios de Prueba

Los usuarios se crean automáticamente al primer inicio con el archivo (`SeedData.cs`):

* **Rol Administrador:**
    * **Email:** `admin@dis.com`
    * **Clave:** `Admin123!`
* **Rol Empleado:**
    * **Email:** `empleado@dis.com`
    * **Clave:** `Empleado123!`


