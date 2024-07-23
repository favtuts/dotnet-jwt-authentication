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
