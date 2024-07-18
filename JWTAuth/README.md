# How to Implement JWT Authentication in Web API Using .Net 8.0, Asp.Net Core
* https://tuts.heomi.net/how-to-implement-jwt-authentication-in-web-api-using-net-8-0-asp-net-core/

# Create Database and Tables

* [DB scripts for creating tables](./DBScripts/tables.sql)

# Install Required Nuget Packages

```bash
Install-Package Microsoft.EntityFrameworkCore
Install-Package Microsoft.EntityFrameworkCore.SqlServer
Install-Package Microsoft.AspNetCore.Authentication.JwtBearer
```

# Adding the Model to the Application

* [UserInfo.cs](./JWTAuth.WebApi/Models/UserInfo.cs)
* [Employee.cs](./JWTAuth.WebApi/Models/Employee.cs)