using Parent_Teacher.Data;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add Razor Pages and EF Core
builder.Services.AddRazorPages();
builder.Services.AddSession();

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

var app = builder.Build();

if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}

app.UseStaticFiles();
app.UseRouting();
app.UseAuthorization();

app.UseSession();

app.MapRazorPages();
app.Run();
