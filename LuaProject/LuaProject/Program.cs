
using LuaProject.Application.RepositoryInterfaces;
using LuaProject.Application.Services;
using LuaProject.Application.ServicesInterfaces;
using LuaProject.Infrastracture.Lua;
using LuaProject.Infrastracture.Repositories;
using Scalar.AspNetCore;

var builder = WebApplication.CreateBuilder(args);
var myAllowFrontEnd = "_myAllowFrontEnd";

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddScoped<IRulesRepository, RulesRepository>();
builder.Services.AddScoped<IRulesService, RulesService>();
builder.Services.AddScoped<ILuaExecutionService, LuaExecutionService>();
builder.Services.AddScoped<IExecutionRepository, ExecutionRepository>();
builder.Services.AddScoped<IEventDispatchService, EventDispatchService>();
builder.Services.AddScoped<IExecutionService, ExecutionsService>();
builder.Services.AddScoped<IRuleVersionRepository, RuleVersionRepository>();
builder.Services.AddScoped<IRuleVersionService, RuleVersionService>();

builder.Services.AddCors(options =>
{
    options.AddPolicy(name: myAllowFrontEnd,
                      policy =>
                      {
                          policy.WithOrigins("http://localhost:4200")
                                .AllowAnyHeader()
                                .AllowAnyMethod();
                      });
});
string? connectionString = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddSqlServer<LuaProject.Infrastracture.Data.AppDbContext>(connectionString);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
    app.MapScalarApiReference();
}

app.UseHttpsRedirection();
app.UseCors(myAllowFrontEnd);

app.UseAuthorization();

app.MapControllers();

app.Run();
