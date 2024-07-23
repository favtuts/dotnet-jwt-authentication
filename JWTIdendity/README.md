# JWT Authentication And Authorization In .NET 8.0 With Identity Framework
* https://tuts.heomi.net/jwt-authentication-and-authorization-in-net-8-0-with-identity-framework/

# Install Libraries

Open Package Manager Console
```bash
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.EntityFrameworkCore.Tools
Install-Package Microsoft.AspNetCore.Identity.EntityFrameworkCore
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
```

# Create Auth classes

* [ApplicationDbContext.cs](./JWTIdendity.WebAPI/Auth/ApplicationDbContext.cs)
* [UserRoles.cs](./JWTIdendity.WebAPI/Auth/UserRoles.cs)
* [RegisterModel.cs](./JWTIdendity.WebAPI/Auth/RegisterModel.cs)
* [LoginModel.cs](./JWTIdendity.WebAPI/Auth/LoginModel.cs)
* [Response.cs](./JWTIdendity.WebAPI/Auth/Response.cs)


# Create API Controllers

* [AuthenticateController.cs](./JWTIdendity.WebAPI/Controllers/AuthenticateController.cs)

We have added three methods “login”, “register”, and “register-admin” inside the controller class

Register and register-admin are almost same, but the register-admin method will be used to create a user with admin role. In login method, we have returned a JWT token after successful login. 


# Update Program for Startup

In .NET 8.0, Microsoft removed the Startup class and only kept Program class. We must define all our dependency injection and other configurations inside the Program class. 
* [Program.cs](./JWTIdendity.WebAPI/Program.cs)


# Migration Entity Framework to Database

First run command to create the migration codes
```bash
PM> add-migration Initial
```

Then run command to create database and tables
```bash
PM> update-database
Build started...
Build succeeded.
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (178ms) [Parameters=[], CommandType='Text', CommandTimeout='60']
      CREATE DATABASE [JWTAuthDB];
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (32ms) [Parameters=[], CommandType='Text', CommandTimeout='60']
      IF SERVERPROPERTY('EngineEdition') <> 5
      BEGIN
          ALTER DATABASE [JWTAuthDB] SET READ_COMMITTED_SNAPSHOT ON;
      END;
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (5ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT 1
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (6ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [__EFMigrationsHistory] (
          [MigrationId] nvarchar(150) NOT NULL,
          [ProductVersion] nvarchar(32) NOT NULL,
          CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
      );
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (0ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT 1
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (11ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT OBJECT_ID(N'[__EFMigrationsHistory]');
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (6ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      SELECT [MigrationId], [ProductVersion]
      FROM [__EFMigrationsHistory]
      ORDER BY [MigrationId];
Microsoft.EntityFrameworkCore.Migrations[20402]
      Applying migration '20240723083339_Initial'.
Applying migration '20240723083339_Initial'.
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [AspNetRoles] (
          [Id] nvarchar(450) NOT NULL,
          [Name] nvarchar(256) NULL,
          [NormalizedName] nvarchar(256) NULL,
          [ConcurrencyStamp] nvarchar(max) NULL,
          CONSTRAINT [PK_AspNetRoles] PRIMARY KEY ([Id])
      );
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [AspNetUsers] (
          [Id] nvarchar(450) NOT NULL,
          [UserName] nvarchar(256) NULL,
          [NormalizedUserName] nvarchar(256) NULL,
          [Email] nvarchar(256) NULL,
          [NormalizedEmail] nvarchar(256) NULL,
          [EmailConfirmed] bit NOT NULL,
          [PasswordHash] nvarchar(max) NULL,
          [SecurityStamp] nvarchar(max) NULL,
          [ConcurrencyStamp] nvarchar(max) NULL,
          [PhoneNumber] nvarchar(max) NULL,
          [PhoneNumberConfirmed] bit NOT NULL,
          [TwoFactorEnabled] bit NOT NULL,
          [LockoutEnd] datetimeoffset NULL,
          [LockoutEnabled] bit NOT NULL,
          [AccessFailedCount] int NOT NULL,
          CONSTRAINT [PK_AspNetUsers] PRIMARY KEY ([Id])
      );
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [AspNetRoleClaims] (
          [Id] int NOT NULL IDENTITY,
          [RoleId] nvarchar(450) NOT NULL,
          [ClaimType] nvarchar(max) NULL,
          [ClaimValue] nvarchar(max) NULL,
          CONSTRAINT [PK_AspNetRoleClaims] PRIMARY KEY ([Id]),
          CONSTRAINT [FK_AspNetRoleClaims_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE
      );
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [AspNetUserClaims] (
          [Id] int NOT NULL IDENTITY,
          [UserId] nvarchar(450) NOT NULL,
          [ClaimType] nvarchar(max) NULL,
          [ClaimValue] nvarchar(max) NULL,
          CONSTRAINT [PK_AspNetUserClaims] PRIMARY KEY ([Id]),
          CONSTRAINT [FK_AspNetUserClaims_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
      );
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [AspNetUserLogins] (
          [LoginProvider] nvarchar(450) NOT NULL,
          [ProviderKey] nvarchar(450) NOT NULL,
          [ProviderDisplayName] nvarchar(max) NULL,
          [UserId] nvarchar(450) NOT NULL,
          CONSTRAINT [PK_AspNetUserLogins] PRIMARY KEY ([LoginProvider], [ProviderKey]),
          CONSTRAINT [FK_AspNetUserLogins_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
      );
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [AspNetUserRoles] (
          [UserId] nvarchar(450) NOT NULL,
          [RoleId] nvarchar(450) NOT NULL,
          CONSTRAINT [PK_AspNetUserRoles] PRIMARY KEY ([UserId], [RoleId]),
          CONSTRAINT [FK_AspNetUserRoles_AspNetRoles_RoleId] FOREIGN KEY ([RoleId]) REFERENCES [AspNetRoles] ([Id]) ON DELETE CASCADE,
          CONSTRAINT [FK_AspNetUserRoles_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
      );
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE TABLE [AspNetUserTokens] (
          [UserId] nvarchar(450) NOT NULL,
          [LoginProvider] nvarchar(450) NOT NULL,
          [Name] nvarchar(450) NOT NULL,
          [Value] nvarchar(max) NULL,
          CONSTRAINT [PK_AspNetUserTokens] PRIMARY KEY ([UserId], [LoginProvider], [Name]),
          CONSTRAINT [FK_AspNetUserTokens_AspNetUsers_UserId] FOREIGN KEY ([UserId]) REFERENCES [AspNetUsers] ([Id]) ON DELETE CASCADE
      );
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE INDEX [IX_AspNetRoleClaims_RoleId] ON [AspNetRoleClaims] ([RoleId]);
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE UNIQUE INDEX [RoleNameIndex] ON [AspNetRoles] ([NormalizedName]) WHERE [NormalizedName] IS NOT NULL;
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (2ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE INDEX [IX_AspNetUserClaims_UserId] ON [AspNetUserClaims] ([UserId]);
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE INDEX [IX_AspNetUserLogins_UserId] ON [AspNetUserLogins] ([UserId]);
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE INDEX [IX_AspNetUserRoles_RoleId] ON [AspNetUserRoles] ([RoleId]);
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE INDEX [EmailIndex] ON [AspNetUsers] ([NormalizedEmail]);
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (1ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      CREATE UNIQUE INDEX [UserNameIndex] ON [AspNetUsers] ([NormalizedUserName]) WHERE [NormalizedUserName] IS NOT NULL;
Microsoft.EntityFrameworkCore.Database.Command[20101]
      Executed DbCommand (9ms) [Parameters=[], CommandType='Text', CommandTimeout='30']
      INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
      VALUES (N'20240723083339_Initial', N'8.0.7');
Done.
```

Using Visual Studio Server Explorer to open the Database

![vs_localdb](./images/JWT-Net-Connect-LocalDB.png)

You can also using SSMS (SQL Server Mamanagement Studio) to connect to LocalDB

![esms_localdb](./images/JWT-Net-SSMS-LocalDB.png)

You can see that the tables below are created inside the database

![tables_localdb](./images/JWT-Net-SSMS-ObjectExplorer.png)


# Authorize the WeatherForecast API

Add “Authorize” attribute inside the [WeatherForecast controller](JWTIdendity.WebAPI/Controllers/WeatherForecastController.cs).

```csharp
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
```

We can run the application and try to access get method in weatherforecast controller. We have received a `401 unauthorized` error.

![401_unauthorized](./images/JWT-Net-WeatherForecast-401-Unauthorized.png)

# Register normal user

```bash
curl --location 'https://localhost:7214/api/Authenticate/register' \
--header 'Content-Type: application/json' \
--data-raw '{
    "username": "favtuts",
    "email": "favtuts@gmail.com",
    "password": "Password@1234"
}'
```

![register_user](./images/JWT-Net-Register-User-Postman.png)

# Get JWT token

Login with registered credentials
```bash
curl --location 'https://localhost:7214/api/Authenticate/login' \
--header 'Content-Type: application/json' \
--data-raw '{
    "username": "favtuts",    
    "password": "Password@1234"
}'
```

![login_credentials](./images/JWT-Net-Login-Credentials-Postman.png)

We have the Access Token which is decoded on tool: https://jwt.io/
![decode_jwt_token](./images/JWT-Net-AccessToken-Decode.png)

# Bearer token inside the authorization

```bash
curl --location 'https://localhost:7214/api/WeatherForecast' \
--header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZmF2dHV0cyIsImp0aSI6ImI1M2QyNjgxLTJkOWEtNDI2Mi05ZGYwLTNjNWQxY2ZlZGY0ZSIsImV4cCI6MTcyMTc1OTcxNiwiaXNzIjoiaHR0cDovL2xvY2FsaG9zdDo1MDAwIiwiYXVkIjoiaHR0cDovL2xvY2FsaG9zdDo0MjAwIn0.Julc1zOq7wrQCQuE-fwLlYVavj_xDtm35JJNvdblEhw'
```

![bearer_authorization](./images/JWT-Net-WeatherForecast-Bearer-Authorization.png)

# Role-based Authorization

We change the Authorize annotation
```csharp
    [Authorize(Roles = UserRoles.Admin)]
    [ApiController]
    [Route("api/[controller]")]
    public class WeatherForecastController : ControllerBase
```

We can try to access the weatherforecast controller with same token again in Postman tool. 
We have received a 403 forbidden error instead of 401 now. 

![bearer_authorization](./images/JWT-Net-WeatherForecast-403-Forbidden.png)

Even though we are passing a valid token we don’t have sufficient privilege to access the controller. To access this controller, the user must have an admin role permission. Current user is a normal user and does not have any admin role permission. 


# Register normal user

```bash
curl --location 'https://localhost:7214/api/Authenticate/register-admin' \
--header 'Content-Type: application/json' \
--data-raw '{
    "username": "favtuts-admin",
    "email": "favtuts-admin@gmail.com",
    "password": "Password@1234"
}'
```

![register_user_admin](./images/JWT-Net-Register-Admin-Postman.png)


# Get JWT token with Admin user

```bash
curl --location 'https://localhost:7214/api/Authenticate/login' \
--header 'Content-Type: application/json' \
--data-raw '{
    "username": "favtuts-admin",    
    "password": "Password@1234"
}'
```

![login_admin](./images/JWT-Net-Login-Admin-Postman.png)

If you decode the token, you can see that the roles are added to the token.
![decode_jwt_admin_role](./images/JWT-Net-AccessToken-Decode-Admin-Role.png)


# Role-based Authorization

We can use this new token instead of the old token to access the weatherforecast controller. 

```bash
curl --location 'https://localhost:7214/api/WeatherForecast' \
--header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJodHRwOi8vc2NoZW1hcy54bWxzb2FwLm9yZy93cy8yMDA1LzA1L2lkZW50aXR5L2NsYWltcy9uYW1lIjoiZmF2dHV0cy1hZG1pbiIsImp0aSI6IjNmZWE0NjRiLWQ5MGYtNDQ1My1iNjY5LWY0YjRlNTJjYTk1MiIsImh0dHA6Ly9zY2hlbWFzLm1pY3Jvc29mdC5jb20vd3MvMjAwOC8wNi9pZGVudGl0eS9jbGFpbXMvcm9sZSI6WyJVc2VyIiwiQWRtaW4iXSwiZXhwIjoxNzIxNzYxNDAwLCJpc3MiOiJodHRwOi8vbG9jYWxob3N0OjUwMDAiLCJhdWQiOiJodHRwOi8vbG9jYWxob3N0OjQyMDAifQ.u_FcvDF-nJIqvJKiGcKLu1ukvFnlloD2nDSn1-ZloyU'
```

![role_authorization](./images/JWT-Net-WeatherForecast-Role-Based-Authorization.png)

Now we have successfully fetched the data from weatherforecast controller.