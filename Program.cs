using System.Security.Claims;
using Microsoft.Extensions.Logging.Console;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddGlobalErrorHandling();

builder.Services.AddCors(opt =>
        opt.AddDefaultPolicy(
            builder => builder
            // .AllowAnyOrigin()
            .WithOrigins("http://localhost:4200", "http://localhost:4201")
            .AllowAnyHeader()
            .AllowAnyMethod()
            .AllowCredentials()
        )

    );

JwtSettings jwtSetttings = builder.Configuration
    .GetRequiredSection("AuthenticationSettings")
    .Get<JwtSettings>()!;   // ArgumentNullException

builder.Services.AddJwtAuthentication(jwtSetttings);
builder.Services.AddAuthorization();

// builder.Logging.ClearProviders();
// builder.Logging.AddConsole();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// data-services
#region data-services
// builder.Services.AddSingleton<ISqlConnectionFactory, SqlConnectionFactory>();
// builder.Services.AddSingleton<IProductsData, ProductsData>();
// builder.Services.AddSingleton<ICouponsData, CouponsData>();
// builder.Services.AddSingleton<IUsersData, UsersData>();
#endregion
builder.Services.AddData();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalErrorHandling();

app.UseHttpsRedirection();

app.UseCors();

app.UseAuthentication();
// logging user role
app.Use(async (context, next) => { Console.WriteLine(context.User.FindFirst(ClaimTypes.Role)?.Value); await next.Invoke(); });
app.UseAuthorization();

app.MapControllers();

app.Run();
