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

# Adding Data Access Layer to the Application

* Add the [DatabaseContext.cs](./JWTAuth.WebApi/Models/DatabaseContext.cs) file to the `Models` folder
* Right-click on the `JWTAuth.WebApi` project and add two new folders as `Interface` and `Repository`
* Add an interface to the `Interface` folder, name it as [IEmployees.cs](./JWTAuth.WebApi/Interface/IEmployees.cs)
* Add a class name as [EmployeeRepository.cs](./JWTAuth.WebApi/Repository/EmployeeRepository.cs) to the `Repository` folder, which will inherit `IEmployees` interface
* Regirster `DatabaseContext` and `IEmployees` to the container in the [Program.cs](JWTAuth.WebApi/Program.cs) file

# Adding the Web API Controller to the Application

* Aut the controller class name as [EmployeeController.cs](JWTAuth.WebApi/Controllers/EmployeeController.cs)