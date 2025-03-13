# MI API FINANZAUTO
ApiFinanzauto es una API RESTful diseñada para la gestión de un sistema educativo, permitiendo administrar estudiantes, profesores, cursos y calificaciones. Implementa autenticación y autorización basadas en JWT (JSON Web Tokens) con roles de usuario, y está construida con .NET 8.0, Entity Framework Core y SQL Server.

## Características
### Autenticación y Autorización: 
Soporte para registro e inicio de sesión de usuarios con tokens JWT. Roles predefinidos: User, Admin, y Professor.
### Gestión de Entidades:
Estudiantes: Crear, leer, actualizar y eliminar (CRUD) estudiantes.
Profesores: CRUD para profesores.
Cursos: Gestión de cursos.
Calificaciones: Asignación y consulta de calificaciones.
### Filtros Personalizados: 
Manejo centralizado de excepciones mediante filtros de acción.
### Documentación con Swagger: 
Interfaz interactiva para probar los endpoints.

# Tecnologías Utilizadas
.NET 8.0: Framework principal para el desarrollo de la API.<br>
Entity Framework Core 8.0: ORM para la interacción con la base de datos.<br>
SQL Server LocalDB: Base de datos para desarrollo (configurable para otros proveedores de SQL Server).<br>
ASP.NET Core Identity: Gestión de usuarios y roles.<br>
JWT (JSON Web Tokens): Autenticación basada en tokens.<br>
Swagger/OpenAPI: Documentación interactiva de la API.<br>
Filters: Filtros personalizados para manejo de excepciones y logging.<br>

# Instalación y Configuración
1. Clonar el Repositorio
Clona el repositorio desde GitHub : git clone https://github.com/jvianad/ApiFinanzauto
2. Restaurar Dependencias
Restaura los paquetes NuGet necesarios: dotnet restore
3. Configurar la Base de Datos
El proyecto utiliza SQL Server LocalDB por defecto. Configura la cadena de conexión en appsettings.json si usas otro servidor SQL Server.
4. Aplicar Migraciones
Crea la base de datos y aplica las migraciones: dotnet ef database update --project ApiFinanzauto
5. Ejecutar la Aplicación
Inicia la API: Al momento de iniciar se abre directamente el swagger

# Uso de la API
1. La API utiliza autenticación basada en JWT. Para acceder a los endpoints protegidos, primero debes autenticarte, en registrar un usuario, todos seran de tipo user.
2. Inicias sesion en Auth login con los datos que te registraste
3. Se te genera un token, eso lo pones en el boton "Authorize" de swagger(arriba a la derecha): Ingresa el token en el formato Bearer <token> (por ejemplo, Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...).
4. Ya puedes acceder a los endpoints, estan configurados para que todos puedan revisarlos cada uno, independientemente del rol

# Contacto
## Autor: Jean Carlo Viana De Mares
## Correo: jeanvianademares@gmail.com
## GitHub: jvianad

