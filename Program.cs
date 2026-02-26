using Microsoft.EntityFrameworkCore;
using TechStoreAPI.Data;
using Swashbuckle.AspNetCore.SwaggerUI;
    
var builder = WebApplication.CreateBuilder(args);

// 🔹 Add services
builder.Services.AddControllers();

// 🔹 Swagger (versión estable para .NET 8)
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

// 🔹 DbContext
builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection")
    ));

var app = builder.Build();

// 🔹 Middleware
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();
