using enfoco2.Data;
using enfoco2.Models;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");
builder.Services.AddDbContext<EnfocoDb>(options => options.UseNpgsql(connectionString));

//script para crear la base de datos
// dotnet ef database update firstmigration --project enfoco2.csproj

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();


//vamos a crear la api web
app.MapPost("/noticias/", async (Notice n, EnfocoDb db) =>
{
    db.Notices.Add(n);
    await db.SaveChangesAsync();

    return Results.Created($"/noticia/{n.Id}", n);
});

app.MapGet("/noticias/{id:int}", async (int id, EnfocoDb db) =>
{
    return await db.Notices.FindAsync(id)
    is Notice n ? Results.Ok(n) : Results.NotFound();
});



app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

