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

# Adding the Token to the Application

* Aut the controller class name as [TokenController.cs](JWTAuth.WebApi/Controllers/TokenController.cs)

# Test APIs are secured by the JWT with Postman

First get token:
```bash
curl --location 'https://localhost:7104/api/token' \
--header 'accept: text/plain' \
--header 'Content-Type: application/json' \
--data-raw '{
    "email": "admin@abc.com",
    "password": "$admin@2022"
}'
```

Then append `Authorization` header with Bearer token:
```bash
curl --location 'https://localhost:7104/api/employee' \
--header 'accept: text/plain' \
--header 'Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJKV1RTZXJ2aWNlQWNjZXNzVG9rZW4iLCJqdGkiOiI3YTEwNjk3MS04MTExLTQxZGItYTU4Zi03YTYzMDhlYzc1NTkiLCJpYXQiOiIxNzIxMjk5OTYzIiwiVXNlcklkIjoiMSIsIkRpc3BsYXlOYW1lIjoiQW1pdCBNb2hhbnR5IiwiVXNlck5hbWUiOiJBZG1pbiIsIkVtYWlsIjoiYWRtaW5AYWJjLmNvbSIsImV4cCI6MTcyMTMwMDU2MywiaXNzIjoiSldUQXV0aGVudGljYXRpb25TZXJ2ZXIiLCJhdWQiOiJKV1RTZXJ2aWNlUG9zdG1hbkNsaWVudCJ9.1_2iw1mFSztyVJNQFZXe8Cz0khiSN2eGDRB-T8iP6-Q'


[
    {
        "employeeID": 1,
        "nationalIDNumber": "295847284",
        "employeeName": "Michael Westover",
        "loginID": "adventure-works\\ken0",
        "jobTitle": "Vice President of Sales",
        "birthDate": "1969-01-29T00:00:00",
        "maritalStatus": "S",
        "gender": "M",
        "hireDate": "2009-01-14T00:00:00",
        "vacationHours": 99,
        "sickLeaveHours": 69,
        "rowGuid": "f01251e5-96a3-448d-981e-0f99d789110d",
        "modifiedDate": "2014-06-30T00:00:00"
    },
    {
        "employeeID": 2,
        "nationalIDNumber": "245797967",
        "employeeName": "Raeann Santos",
        "loginID": "adventure-works\\terri0",
        "jobTitle": "Vice President of Engineering",
        "birthDate": "1971-08-01T00:00:00",
        "maritalStatus": "S",
        "gender": "F",
        "hireDate": "2008-01-31T00:00:00",
        "vacationHours": 1,
        "sickLeaveHours": 20,
        "rowGuid": "45e8f437-670d-4409-93cb-f9424a40d6ee",
        "modifiedDate": "2014-06-30T00:00:00"
    },
    {
        "employeeID": 3,
        "nationalIDNumber": "509647174",
        "employeeName": "Pamela Wambsgans",
        "loginID": "adventure-works\\roberto0",
        "jobTitle": "Engineering Manager",
        "birthDate": "1974-11-12T00:00:00",
        "maritalStatus": "M",
        "gender": "M",
        "hireDate": "2007-11-11T00:00:00",
        "vacationHours": 2,
        "sickLeaveHours": 21,
        "rowGuid": "9bbbfb2c-efbb-4217-9ab7-f97689328841",
        "modifiedDate": "2014-06-30T00:00:00"
    }
]
```


# Fix: ASP.NET core JWT authentication always throwing 401 unauthorized

* Issue 1: The "iat (issued at)" is not correct.(https://github.com/dotnet/aspnetcore/issues/52286)

Certainly! When upgrading from .NET 6 to .NET 8, I encountered an issue related to setting the "issued at" (iat) claim. Initially, I was using `DateTime.UtcNow.ToString()`, but I switched to `DateTimeOffset.Now.ToUnixTimeSeconds().ToString()` to address the issue. 🚀🕰️

* Issue 2: Need to add  JwtBearerOptions.UseSecurityTokenValidators to true (https://stackoverflow.com/questions/77515249/custom-token-validator-not-working-in-net-8)

According to Microsoft docs in here the easiest way to fix this is to set
```
UseSecurityTokenValidators = true;
```

* Need to append the Log Request Headers Middleware for debuging (https://ardalis.com/log-request-headers-middleware/)