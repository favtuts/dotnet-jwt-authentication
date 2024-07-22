# Implement JWT In .NET Core API
* https://tuts.heomi.net/implement-jwt-in-net-core-api/

# Create new ASP.NET Core Web API application

* Using Visual Studio 2022
* Using `.NET 8.0 LTS`

# Install Nuget Packages.

```
Microsoft.EntityFrameworkCore.SqlServer
Microsoft.EntityFrameworkCore.Tools
Microsoft.IdentityModel.Tokens
System.IdentityModel.Tokens.Jwt
Microsoft.AspNetCore.Authentication.JwtBearer
```

In the root directory, you can use .NET Core CLI to fetch installed package for a given solution or project
```bash
$ dotnet list package
Project 'TokenDemo.Web' has the following package references
   [net8.0]:
   Top-level Package                                    Requested   Resolved
   > Microsoft.AspNetCore.Authentication.JwtBearer      8.0.7       8.0.7
   > Microsoft.EntityFrameworkCore.SqlServer            8.0.7       8.0.7
   > Microsoft.EntityFrameworkCore.Tools                8.0.7       8.0.7
   > Microsoft.IdentityModel.Tokens                     8.0.0       8.0.0
   > Swashbuckle.AspNetCore                             6.6.2       6.6.2
   > System.IdentityModel.Tokens.Jwt                    8.0.0       8.0.0
```

# Create DataContext

We follow the [Database First approach](https://www.entityframeworktutorial.net/efcore/create-model-for-existing-database-in-ef-core.aspx). 


We need to design database first, follow the relationship:
![db-relationship](./images/NetCore-JWT-Database-Models.png)


Then we will generate context and entity classes for an existing database in Entity Framework Core using following command:
```bash
Scaffold-DbContext "Server=.\SQLEXPRESS;Database=TokeDemoDB;Trusted_Connection=True;Persist Security Info=False;MultipleActiveResultSets=False;Encrypt=True;TrustServerCertificate=True;Connection Timeout=30;" Microsoft.EntityFrameworkCore.SqlServer -ContextDir DataContext -Context DemoTokenContext -OutputDir DataContext -Force
```