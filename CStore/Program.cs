using CStore.Context;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

string stringConnection = builder.Configuration.GetConnectionString(
    "DefaultString");

stringConnection = stringConnection.Replace("{user_name}", "marcos");
stringConnection = stringConnection.Replace("{user_password}", "bybvaB-5nenxy-dixnuh");

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

