# Proyecto: Distribuidora
Proyecto realizado para la materia Laboratorio de Programación II 
Se trata de un sistema de gestión de pedidos, stock, clientes y proveedores para una distribuidora mayorista.

### Todavía no está listo para presentación, lo voy a tener listo para mesa de exámen. Pero voy a seguir trabajando en paralelo

## Tecnológias

* **Backend:** ASP.NET Core 8.0 MVC
* **Base de Datos:** SQLite (Entity Framework Core 8)
* **Frontend (MVC):** Vistas Razor con Bootstrap
* **Frontend (CRUD):** Vue.js 3 para el ABM de Productos
* **Seguridad:** ASP.NET Core Identity con Roles
* **API:** Web API con autenticación JWT

## Cómo Ejecutar el Proyecto

1.  Clonar el repositorio.
2.  Abrir una terminal en la carpeta raíz (donde está `Distribuidora.sln`).
3.  Ingresar a la carpeta del proyecto: `cd Distribuidora`
4.  Ejecutar `dotnet restore` para instalar los paquetes.
5.  Ejecutar `dotnet ef database update` para aplicar las migraciones y crear la BD (`distribuidora.db`).
6.  Ejecutar `dotnet run` para iniciar el servidor.
7.  Abrir el navegador en la URL indicada (ej. `http://localhost:5218`).
## Usuarios de Prueba

Los usuarios se crean automáticamente al primer inicio con el archivo SeedData.cs:

* **Rol Administrador:**
    * **Email:** `admin@dis.com`
    * **Clave:** `Admin123!`
* **Rol Empleado:**
    * **Email:** `empleado@dis.com`
    * **Clave:** `Empleado123!`


