using Microsoft.AspNetCore.Builder;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Seom.Application.Infrastructure;
using Seom.Application.Services;

var builder = WebApplication.CreateBuilder(args);
// *************************************************************************************************
// BUILDER CONFIGURATION
// *************************************************************************************************

// Database*****************************************************************************************
// Read the sql server connection string from appsettings.json located at
// ConnectionStrings -> Default.
builder.Services.AddDbContext<SeomContext>(opt =>
    opt.UseSqlite(builder.Configuration.GetConnectionString("Default"),
        o => o.UseQuerySplittingBehavior(QuerySplittingBehavior.SingleQuery)));

// Add services to the container.
builder.Services.AddSingleton(provider => new CalendarService(2000,2100));
builder.Services.AddRazorPages();

var app = builder.Build();
// Creating the database.
using (var scope = app.Services.CreateScope())
{
    using (var db = scope.ServiceProvider.GetRequiredService<SeomContext>())
    {
        db.CreateDatabase(isDevelopment: app.Environment.IsDevelopment());
    }
}

app.UseHttpsRedirection();
app.UseAuthentication();
app.UseAuthorization();
app.UseStaticFiles();
app.UseRouting();
app.MapRazorPages();
app.Run();
