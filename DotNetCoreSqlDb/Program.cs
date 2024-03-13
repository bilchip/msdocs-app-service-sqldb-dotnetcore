using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using DotNetCoreSqlDb.Data;
var builder = WebApplication.CreateBuilder(args);

// Add database context and cache
// bilchip@gmail.com: link: https://learn.microsoft.com/en-us/azure/app-service/tutorial-dotnetcore-sqldb-app
// commit 01
// commit 02
// prepare for database
// add user and assign role
// SQL> CREATE USER db01_user WITH PASSWORD = 'PP01-Gjkmpjdfntkm-85'
// SQL> EXEC sp_addrolemember 'db_owner', 'db01_user';


builder.Services.AddDbContext<MyDatabaseContext>(options =>
  //options.UseSqlServer(builder.Configuration.GetConnectionString("MyDbConnection")));
  options.UseSqlServer(builder.Configuration.GetConnectionString("AZURE_SQL_CONNECTIONSTRING")));
builder.Services.AddDistributedMemoryCache();

// Add services to the container.
builder.Services.AddControllersWithViews();

// Add App Service logging
builder.Logging.AddAzureWebAppDiagnostics();
builder.Services.AddApplicationInsightsTelemetry(builder.Configuration["APPLICATIONINSIGHTS_CONNECTION_STRING"]);

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    // app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Todos}/{action=Index}/{id?}");

app.Run();
