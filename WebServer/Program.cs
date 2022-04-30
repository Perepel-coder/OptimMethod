using Autofac;
using AutofacDependence;
using Microsoft.EntityFrameworkCore;
using Repository;

var builder = WebApplication.CreateBuilder(args);
// Add-migration migr1
// update-database
builder.Services.AddDbContext<RepositoryContext>(options => options.UseSqlite("Data Source = DatabaseMO.db"));

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
