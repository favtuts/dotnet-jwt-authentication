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

# Run the Application and Test APIs with Postman

**Testing with Swagger UI**
* Start the Web API
* Can access Swagger UI: https://localhost:7104/swagger/index.html
* Execute the API: `api/employee`

**Testing with Postman**
* Enter enpoint:  https://localhost:7104/api/employee


Get all employees
```bash
curl --location 'https://localhost:7104/api/employee' \
--header 'accept: text/plain'
```

View employee details
```bash
curl --location 'https://localhost:7104/api/employee/1' \
--header 'accept: text/plain'
```

Create new Employee
```bash
curl --location 'https://localhost:7104/api/employee' \
--header 'accept: text/plain' \
--header 'Content-Type: application/json' \
--data '{
    "employeeID": 11,
    "nationalIDNumber": "998998998",
    "employeeName": "Amit Mohanty",
    "loginID": "adventure-works\\amit0",
    "jobTitle": "Chief Executive Officer",
    "birthDate": "1986-07-07",
    "maritalStatus": "M",
    "gender": "M",
    "hireDate": "2022-01-18",
    "vacationHours": 100,
    "sickLeaveHours": 80,
    "rowGuid": "b77f8c01-1390-4573-8412-ac861388b137",
    "modifiedDate": "2014-06-30T00:00:00"
}'
```

Update details of Employee
```bash
curl --location --request PUT 'https://localhost:7104/api/employee/11' \
--header 'accept: text/plain' \
--header 'Content-Type: application/json' \
--data '{
    "employeeID": 11,
    "nationalIDNumber": "998998998",
    "employeeName": "Amit Mohanty",
    "loginID": "adventure-works\\amit0",
    "jobTitle": "Chief Executive Officer",
    "birthDate": "1986-07-07",
    "maritalStatus": "M",
    "gender": "M",
    "hireDate": "2024-01-18",
    "vacationHours": 100,
    "sickLeaveHours": 200,
    "rowGuid": "b77f8c01-1390-4573-8412-ac861388b137",
    "modifiedDate": "2024-06-30T00:00:00"
}'
```

Delete an Employee
```bash
curl --location --request DELETE 'https://localhost:7104/api/employee/11' \
--header 'accept: text/plain'
```