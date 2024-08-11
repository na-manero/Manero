using Manero.Lib.Context;
using Manero.Web.Helpers;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var sqlConnectionString = builder.Environment.IsDevelopment() ? builder.Configuration.GetConnectionString("SqlServer") : Environment.GetEnvironmentVariable("SqlServer_ConnectionString");

builder.Services.AddDbContext<IdentityDbContext>(o => o.UseSqlServer(sqlConnectionString));

builder.Services.AddIdentity<IdentityUser, IdentityRole>(x =>
{
    x.User.RequireUniqueEmail = true;
    x.Password.RequiredLength = 8;
}).AddEntityFrameworkStores<IdentityDbContext>();

builder.Services.AddHttpClient<IApiHelper, ApiHelper>();

var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Default}/{action=Home}/{id?}");

app.Run();
