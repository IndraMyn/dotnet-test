

using Microsoft.EntityFrameworkCore;
using test.Models;

var builder = WebApplication.CreateBuilder(args);

var connectionString = builder.Configuration.GetConnectionString("DefaultConnection");

builder.Services.AddDbContext<BaseDbContext>(options =>
    options.UseSqlServer(connectionString), ServiceLifetime.Scoped);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var dbContext = scope.ServiceProvider.GetRequiredService<BaseDbContext>();
    if (await dbContext.Database.CanConnectAsync())
    {
        Console.WriteLine("Koneksi ke database berhasil.");
    }
    else
    {
        Console.WriteLine("Koneksi ke database gagal.");
    }
}


// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseRouting();

app.UseAuthorization();

app.MapStaticAssets();
app.MapRazorPages()
   .WithStaticAssets();

app.Run();
