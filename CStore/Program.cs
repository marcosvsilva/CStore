using CStore.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string stringConnection = builder.Configuration.GetConnectionString(
    "DefaultString");

var username = System.
    Configuration.ConfigurationManager.AppSettings["username"];

var password = System.
    Configuration.ConfigurationManager.AppSettings["password"];

stringConnection = stringConnection.Replace("{user_name}", username);
stringConnection = stringConnection.Replace("{user_password}", password);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddDbContext<CStoreContext>(options =>
{
    options.UseSqlServer(stringConnection);
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

