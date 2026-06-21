using Application;
using Infrastructure;
using Infrastructure.Persistants.SeedData;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(); 

builder.Services.AddDatabaseContext(builder.Configuration);
builder.Services.RegisterRepositories();
builder.Services.AddJwtAuthentication(builder.Configuration);

builder.Services.AddApplication();

var app = builder.Build();

app.SeedDataExtensionUsers();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseStaticFiles();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();