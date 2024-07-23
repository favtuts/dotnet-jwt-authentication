using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using TokenDemo.Web.DataContext;
using TokenDemo.Web.Models;
using TokenDemo.Web.Services;

var builder = WebApplication.CreateBuilder(args);
var Configuration = builder.Configuration;
var services = builder.Services;

// Add services to the container.
services.AddDbContext<DemoTokenContext>(opts => opts.UseSqlServer(Configuration["ConnectionString:DefaultConnection"]));

// configure strongly typed settings objects
var appSettingsSection = Configuration.GetSection("ServiceConfiguration");
services.Configure<ServiceConfiguration>(appSettingsSection);

// configure jwt authentication
var serviceConfiguration = appSettingsSection.Get<ServiceConfiguration>();
var JwtSecretkey = Encoding.ASCII.GetBytes(serviceConfiguration.JwtSettings.Secret);
var tokenValidationParameters = new TokenValidationParameters
{
    ValidateIssuerSigningKey = true,
    IssuerSigningKey = new SymmetricSecurityKey(JwtSecretkey),
    ValidateIssuer = false,
    ValidateAudience = false,
    RequireExpirationTime = false,
    ValidateLifetime = true
};
services.AddSingleton(tokenValidationParameters);
services.AddAuthentication(x =>
{
    x.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultScheme = JwtBearerDefaults.AuthenticationScheme;
    x.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;

})
.AddJwtBearer(x =>
{
    x.RequireHttpsMetadata = false;
    x.SaveToken = true;
    x.TokenValidationParameters = tokenValidationParameters;

});


services.AddCors(options =>
{
    options.AddPolicy("CorsPolicy", 
        builder => builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader()
        );
});

// configure Application services
services.AddTransient<IIdentityService, IdentityService>();
services.AddTransient<IUserService, UserService>();

//builder.Services.AddMvc();
builder.Services.AddControllers();

// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("CorsPolicy");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();
