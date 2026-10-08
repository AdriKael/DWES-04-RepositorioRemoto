# Repositorio Remoto --- Gestión de Usuarios

## 1. Descripción del proyecto

**Repositorio Remoto** es una aplicación de consola desarrollada con
.NET 10 que implementa la gestión de usuarios utilizando una
arquitectura por capas y distintos mecanismos de persistencia y caché
según el perfil de ejecución.

La aplicación consume la API pública **JSONPlaceholder** como fuente
remota de usuarios y mantiene una copia local. El acceso a los datos se
realiza mediante un servicio de aplicación que centraliza las
operaciones de consulta, creación, actualización, eliminación y
exportación.

El proyecto dispone de dos perfiles:

-   **dev**: SQLite + `MemoryDistributedCache`.
-   **prod**: PostgreSQL + Redis.

Además, el proyecto incorpora sincronización periódica, notificaciones
reactivas, validación, logging, exportación JSON, pruebas automatizadas
y configuración mediante Docker Compose.

> **Importante:** JSONPlaceholder es una API de demostración. Sus
> operaciones de escritura simulan la respuesta del servidor y no
> constituyen una persistencia remota permanente.

------------------------------------------------------------------------

## 2. Objetivos

Los objetivos principales del proyecto son:

-   Consumir una API REST externa mediante Refit.
-   Implementar las operaciones CRUD sobre usuarios.
-   Mantener persistencia local.
-   Incorporar una capa de caché intercambiable.
-   Separar la configuración de desarrollo y producción.
-   Aplicar inyección de dependencias.
-   Centralizar la lógica de negocio en servicios.
-   Implementar validación y manejo de errores.
-   Registrar la actividad de la aplicación mediante Serilog.
-   Exportar los usuarios a JSON.
-   Sincronizar periódicamente la información local con la API.
-   Utilizar Docker para disponer de PostgreSQL y Redis sin instalarlos
    directamente en el equipo.
-   Disponer de pruebas automatizadas sobre las principales capas.

------------------------------------------------------------------------

## 3. Tecnologías utilizadas

  Tecnología                   Uso
  ---------------------------- ------------------------------------------
  .NET 10                      Plataforma de ejecución
  C#                           Lenguaje
  Entity Framework Core 10     Persistencia
  SQLite                       Base de datos del perfil `dev`
  PostgreSQL                   Base de datos del perfil `prod`
  Npgsql                       Proveedor de EF Core para PostgreSQL
  Redis                        Caché del perfil `prod`
  `MemoryDistributedCache`     Caché del perfil `dev`
  Refit                        Cliente HTTP tipado para JSONPlaceholder
  System.Text.Json             Serialización y exportación JSON
  Serilog                      Logging
  System.Reactive              Sistema de notificaciones
  CSharpFunctionalExtensions   Gestión de resultados y errores
  NUnit                        Pruebas
  Moq                          Mocking
  FluentAssertions             Aserciones de pruebas
  Docker / Docker Compose      Contenerización de servicios

------------------------------------------------------------------------

## 4. Arquitectura

La aplicación está organizada siguiendo una separación de
responsabilidades:

``` text
                         ┌──────────────────────┐
                         │      Program.cs      │
                         │  Menú de consola     │
                         └──────────┬───────────┘
                                    │
                                    ▼
                         ┌──────────────────────┐
                         │     UserService      │
                         │    Lógica de negocio │
                         └──────┬─────┬─────┬───┘
                                │     │     │
                 ┌──────────────┘     │     └──────────────┐
                 ▼                    ▼                    ▼
          ┌────────────┐      ┌────────────┐      ┌──────────────┐
          │   Cache    │      │ Repository │      │ JSONPlaceholder│
          │Memory/Redis│      │ SQLite/PG  │      │    Refit     │
          └────────────┘      └────────────┘      └──────────────┘

                         ┌──────────────────────┐
                         │       Storage        │
                         │      users.json      │
                         └──────────────────────┘
```

La aplicación no es una Web API. Es un ejecutable de consola y el menú
invoca directamente `IUserService`.

------------------------------------------------------------------------

## 5. Estructura del proyecto

``` text
RepositorioRemoto/
├── RepositorioRemoto/
│   ├── Api/
│   ├── Cache/
│   ├── Config/
│   ├── Dto/
│   ├── Entities/
│   ├── Enums/
│   ├── Errors/
│   ├── Infrastructure/
│   ├── Mappers/
│   ├── Models/
│   ├── Repositories/
│   ├── Service/
│   ├── Storages/
│   ├── Validators/
│   ├── Program.cs
│   ├── appsettings.dev.json
│   ├── appsettings.prod.json
│   └── Dockerfile
├── RepositorioRemoto.Test/
│   ├── Cache/
│   ├── Config/
│   ├── Infrastructure/
│   ├── Mappers/
│   ├── Repositories/
│   ├── Service/
│   ├── Storages/
│   └── Validators/
├── compose.yaml
└── .dockerignore
```

### Responsabilidad de las carpetas principales

-   **Api**: interfaz Refit para JSONPlaceholder.
-   **Cache**: abstracción e implementación del caché.
-   **Config**: configuración de perfiles y conexiones.
-   **Dto**: objetos utilizados para comunicarse con la API.
-   **Entities**: contextos de Entity Framework Core.
-   **Errors**: errores de dominio y de las distintas capas.
-   **Infrastructure**: configuración de inyección de dependencias.
-   **Mappers**: conversiones entre DTO y modelo.
-   **Models**: entidades y modelos de dominio.
-   **Repositories**: acceso a la base de datos.
-   **Service**: lógica de aplicación, sincronización y notificaciones.
-   **Storages**: almacenamiento/exportación JSON.
-   **Validators**: validación de usuarios.

------------------------------------------------------------------------

## 6. Configuración por perfiles

### Perfil `dev`

`appsettings.dev.json` utiliza SQLite y una caché en memoria:

``` text
Base de datos: SQLite
Caché: MemoryDistributedCache
```

SQLite se almacena en:

``` text
data/users.db
```

### Perfil `prod`

`appsettings.prod.json` utiliza:

``` text
Base de datos: PostgreSQL
Caché: Redis
```

La configuración local actual utiliza:

``` text
PostgreSQL → localhost:5432
Redis      → localhost:6379
```

Cuando la aplicación se ejecuta dentro de Docker, los servicios deben
resolverse mediante los nombres de Compose (`postgres` y `redis`) en
lugar de `localhost`. La forma recomendada es sobrescribir esas
conexiones mediante variables de entorno.

------------------------------------------------------------------------

## 7. Ejecución local

### Desarrollo

Desde Rider se puede ejecutar el programa pasando:

``` text
dev
```

El programa carga:

``` text
appsettings.dev.json
```

y utiliza SQLite + caché en memoria.

### Producción

Pasando:

``` text
prod
```

el programa carga:

``` text
appsettings.prod.json
```

Para ejecutar este perfil sin instalar PostgreSQL ni Redis en Windows,
ambos servicios pueden levantarse con Docker:

``` bash
docker compose up -d postgres redis
```

Después se ejecuta la aplicación desde Rider con el perfil `prod`.

------------------------------------------------------------------------

## 8. Menú de la aplicación

El programa presenta un menú de consola:

``` text
========================================
       REPOSITORIO REMOTO - USERS
========================================
1. Obtener todos los usuarios
2. Obtener usuario por ID
3. Crear usuario
4. Actualizar usuario
5. Eliminar usuario
6. Exportar usuarios a JSON
0. Salir
========================================
```

Cada opción delega la operación en `IUserService`.

------------------------------------------------------------------------

## 9. Operaciones CRUD

### 9.1 Obtener todos los usuarios

`UserService.GetAllAsync()`:

1.  Consulta el repositorio.
2.  Si existen usuarios locales, los devuelve.
3.  Si no existen, consulta JSONPlaceholder.
4.  Guarda los usuarios obtenidos en la base de datos.
5.  Devuelve los usuarios.

Flujo:

``` text
GET todos
   │
   ▼
Base de datos
   │
 ┌─┴──────────────┐
 │ Hay datos      │ No hay datos
 ▼                ▼
Devolver       JSONPlaceholder
                  │
                  ▼
              Guardar BD
                  │
                  ▼
               Devolver
```

### 9.2 Obtener usuario por ID

`UserService.GetByIdAsync(id)` sigue el orden:

``` text
Caché
  │
  ├── encontrado → devolver
  │
  └── no encontrado
          │
          ▼
      Base de datos
          │
          ├── encontrado → añadir a caché → devolver
          │
          └── no encontrado
                  │
                  ▼
             JSONPlaceholder
                  │
                  ├── no existe → error NotFound
                  │
                  └── existe
                        │
                        ▼
                     Guardar BD
                        │
                        ▼
                   Añadir caché
                        │
                        ▼
                     Devolver
```

### 9.3 Crear usuario

El menú construye un `User`, que se transforma a `CreateUserRequest`.

El servicio:

1.  Valida los datos.
2.  Envía la petición POST a JSONPlaceholder.
3.  Obtiene el ID generado por la API.
4.  Asigna el ID al modelo local.
5.  Guarda el usuario en la base de datos.
6.  Emite una notificación.

### 9.4 Actualizar usuario

El servicio:

1.  Comprueba que el usuario existe localmente.
2.  Envía PUT a JSONPlaceholder.
3.  Actualiza la base de datos.
4.  Invalida la entrada correspondiente de la caché.
5.  Emite una notificación.

### 9.5 Eliminar usuario

El servicio:

1.  Comprueba la existencia.
2.  Envía DELETE a JSONPlaceholder.
3.  Elimina el usuario local.
4.  Elimina su entrada de caché.
5.  Emite una notificación.

------------------------------------------------------------------------

## 10. Caché

La aplicación utiliza la abstracción `IUserCache`.

Esto permite cambiar la implementación sin modificar `UserService`:

``` text
                IUserCache
                    │
          ┌─────────┴─────────┐
          │                   │
        dev                  prod
          │                   │
 MemoryDistributedCache      Redis
```

La caché guarda una colección serializada de usuarios bajo una clave
común y aplica una expiración absoluta configurada en `appsettings`.

------------------------------------------------------------------------

## 11. Persistencia

También se abstrae el acceso a datos mediante `IAppDbContext` e
`IUserRepository`.

``` text
                 IAppDbContext
                      │
             ┌────────┴────────┐
             │                 │
           dev                prod
             │                 │
          SQLite           PostgreSQL
```

Existen dos contextos EF Core porque las dos bases de datos tienen
necesidades de configuración diferentes.

En PostgreSQL, `Address` y `Company` se almacenan mediante columnas
`jsonb` utilizando conversiones de `System.Text.Json`.

En SQLite se utilizan objetos propiedad de EF Core con almacenamiento
JSON.

------------------------------------------------------------------------

## 12. Consumo de la API REST

La API externa se encapsula mediante Refit:

``` csharp
[Get("/users")]
Task<List<User>> GetUsuariosAsync();

[Get("/users/{id}")]
Task<User?> GetUsuarioByIdAsync(int id);

[Post("/users")]
Task<User> CreateUsuarioAsync([Body] CreateUserRequest request);

[Put("/users/{id}")]
Task<User> UpdateUsuarioAsync(int id, [Body] UpdateUserRequest request);

[Delete("/users/{id}")]
Task DeleteUsuarioAsync(int id);
```

Esto evita construir manualmente las peticiones HTTP y centraliza el
contrato de la API.

------------------------------------------------------------------------

## 13. DTO y mappers

Los DTO separan el modelo interno de los objetos que se envían a la API.

Se utilizan:

-   `CreateUserRequest`
-   `UpdateUserRequest`
-   `AddressDto`
-   `CompanyDto`
-   `GeoDto`

Los mappers permiten realizar conversiones en ambos sentidos:

``` text
DTO ───────► Model
Model ─────► DTO
```

Esto evita acoplar directamente el dominio a la representación de la
API.

------------------------------------------------------------------------

## 14. Validación y errores

`UserValidator` comprueba campos obligatorios como:

-   Nombre.
-   Username.
-   Email.
-   Calle.
-   Ciudad.
-   Teléfono.
-   Website.
-   Empresa.

Los errores se agrupan por dominio mediante tipos como:

``` text
DomainErrors
├── ApiErrors
├── RepositoryErrors
├── ServiceErrors
├── StorageErrors
└── UserErrors
```

El proyecto utiliza `Result<T, DomainErrors>` para devolver resultados
exitosos o errores sin depender exclusivamente de excepciones para el
flujo normal de la aplicación.

------------------------------------------------------------------------

## 15. Notificaciones

Las operaciones de creación, actualización y eliminación generan objetos
`Notification`.

`NotificationService` utiliza `Subject<Notification>` de
System.Reactive:

``` text
UserService
     │
     ▼
NotificationService
     │
     ▼
IObservable<Notification>
     │
     ▼
Program
```

El programa se suscribe al observable y muestra las notificaciones en
consola.

------------------------------------------------------------------------

## 16. Sincronización automática

`SynchroService` utiliza `PeriodicTimer`.

El intervalo actual está configurado en:

``` text
60 segundos
```

En cada sincronización:

1.  Se limpia la caché.
2.  Se eliminan los usuarios locales.
3.  Se consulta JSONPlaceholder.
4.  Se guardan de nuevo los usuarios.

``` text
Cada 60 segundos
       │
       ▼
 Limpiar caché
       │
       ▼
 Eliminar BD
       │
       ▼
 Consultar API
       │
       ▼
 Guardar usuarios
```

El servicio se ejecuta de forma concurrente con el menú principal.

------------------------------------------------------------------------

## 17. Exportación JSON

La opción de exportación utiliza `UserStorage` y `System.Text.Json`.

El proceso es:

``` text
Base de datos
     │
     ▼
UserStorage
     │
     ▼
System.Text.Json
     │
     ▼
data/users.json
```

El JSON se genera con formato indentado y nombres de propiedades en
camelCase.

------------------------------------------------------------------------

## 18. Logging

Se utiliza Serilog con salida a consola.

Los logs están agrupados mediante prefijos que permiten identificar la
capa:

``` text
[DI]
[REPO-*]
[CACHE-*]
[SERVICE-*]
[SYNC-*]
[STORAGE-*]
[MAPP-*]
[NOTIF-*]
```

Esto facilita localizar el origen de una operación o de un error durante
la ejecución.

------------------------------------------------------------------------

## 19. Inyección de dependencias

`InyectorDependencias` registra las implementaciones en función del
perfil.

Para `dev`:

``` text
IUserCache      → UserCache + MemoryDistributedCache
IAppDbContext   → AppDbContextSqlite
```

Para `prod`:

``` text
IUserCache      → UserCache + Redis
IAppDbContext   → AppDbContextPostgre
```

El resto de servicios mantiene las mismas abstracciones.

Esto permite cambiar infraestructura sin modificar la lógica de negocio.

------------------------------------------------------------------------

## 20. Docker

El proyecto dispone de un `Dockerfile` para la aplicación y un
`compose.yaml`.

El Compose actual incluye:

``` text
repositorioremoto
postgres
redis
repositorioremoto.test
```

PostgreSQL utiliza un volumen persistente:

``` text
postgres_data
```

Redis utiliza:

``` text
redis_data
```

Ambos servicios disponen de healthchecks.

### Arquitectura en Docker

``` text
                 Docker Compose
                       │
       ┌───────────────┼────────────────┐
       │               │                │
       ▼               ▼                ▼
 repositorioremoto   postgres         redis
       │               │                │
       └───────────────┴────────────────┘
                    red Docker
```

Dentro de Docker, la aplicación debe utilizar:

``` text
postgres:5432
redis:6379
```

en lugar de `localhost`.

La configuración recomendada es mantener los valores locales en
`appsettings.prod.json` y sobrescribir las conexiones mediante variables
de entorno en Compose.

Ejemplo:

``` yaml
environment:
  PROFILE: prod
  Repository__PostgreSqlConnection: "Host=postgres;Port=5432;Database=users;Username=postgres;Password=postgres"
  Cache__RedisConnection: "redis:6379"
```

Para que estas variables sean leídas por la aplicación,
`AppConfig.Configure()` debe incorporar:

``` csharp
.AddEnvironmentVariables()
```

y `Program.cs` debe permitir obtener el perfil desde `PROFILE`.

------------------------------------------------------------------------

## 21. Tests

Todos los comandos se ejecutan desde la carpeta de la solución (RepositorioRemoto/,
la que contiene RepositorioRemoto.slnx).

```bash
# Suite completa (unitarios + integración con Testcontainers)
dotnet test RepositorioRemoto.Test/RepositorioRemoto.Test.csproj

# Solo integración (PostgreSQL + Redis, requieren Docker en marcha)
dotnet test RepositorioRemoto.Test/RepositorioRemoto.Test.csproj --filter "FullyQualifiedName~Integration"
```

### Cobertura

La cobertura excluye Program.cs (punto de entrada consola) mediante
RepositorioRemoto.Test/coverlet.runsettings.

```bash
dotnet test RepositorioRemoto.Test/RepositorioRemoto.Test.csproj --settings RepositorioRemoto.Test/coverlet.runsettings --collect:"XPlat Code Coverage" --results-directory ./TestResults
```

### Informe HTML

Requiere la herramienta reportgenerator (una sola vez):

```bash
dotnet tool install -g dotnet-reportgenerator-globaltool
```

Generar el informe a partir del último XML de cobertura:

```bash
reportgenerator -reports:TestResults/*/coverage.cobertura.xml -targetdir:CoverageReport -reporttypes:Html
```

Abrir CoverageReport/index.html en el navegador.

------------------------------------------------------------------------

## 22. Decisiones de diseño

### Separar API, servicio y repositorio

Se evita que el programa de consola conozca detalles de HTTP o de Entity
Framework. `Program` se ocupa de la interacción con el usuario y
`UserService` de la lógica.

### Usar interfaces

Interfaces como `IUserService`, `IUserRepository`, `IUserCache` e
`IUserStorage` permiten sustituir implementaciones y facilitan las
pruebas mediante mocks.

### Dos DbContext

SQLite y PostgreSQL no se configuran exactamente igual, por lo que se
utilizan dos contextos independientes y una abstracción común.

### Caché distribuida

La interfaz `IDistributedCache` permite usar memoria en desarrollo y
Redis en producción sin cambiar `UserCache`.

### DTO

Los DTO evitan que los modelos internos dependan directamente del
contrato de entrada de la API.

### Result

`CSharpFunctionalExtensions` permite representar operaciones con
éxito/error de forma explícita y mantener un flujo de negocio más
controlado.

### Docker

Docker permite disponer de PostgreSQL y Redis sin instalar ambos
servicios en el sistema operativo.

------------------------------------------------------------------------

## 23. Posibles mejoras

Antes de considerar el proyecto completamente cerrado, se pueden
realizar algunos ajustes:

1.  Añadir las variables `PROFILE`, `Repository__PostgreSqlConnection` y
    `Cache__RedisConnection` al `compose.yaml`.
2.  Añadir `.AddEnvironmentVariables()` a `AppConfig`.
3.  Hacer que `Program.cs` pueda obtener el perfil desde `PROFILE`.
4.  Añadir el usuario creado a la caché después de un POST.
5.  Esperar (`await`) a `cache.RemoveAsync(id)` en las operaciones de
    actualización y eliminación.
6.  Hacer que la exportación reutilice la lógica de `GetAllAsync()` para
    respetar exactamente el comportamiento solicitado.
7.  Devolver o mostrar correctamente la ruta del fichero exportado.
8.  Revisar el comportamiento de la sincronización cuando existen
    operaciones del menú ejecutándose simultáneamente.
9.  Añadir pruebas específicas para el menú y para la integración real
    con Redis/PostgreSQL.

------------------------------------------------------------------------

## 24. Conclusión

El proyecto implementa una aplicación de gestión de usuarios que combina
una API REST externa, persistencia local, caché, validación,
notificaciones, sincronización y exportación.

La separación mediante interfaces y servicios permite cambiar la
infraestructura según el entorno:

``` text
DEV
SQLite + MemoryCache

PROD
PostgreSQL + Redis
```

Docker completa la solución proporcionando los servicios de
infraestructura de producción de forma reproducible y sin necesidad de
instalarlos directamente en el equipo.
