using enfoco2.Data;
using enfoco2.Models;
using enfoco2.Services;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authentication.Cookies;


var builder = WebApplication.CreateBuilder(args);



// Add services to the container.
builder.Services.AddControllersWithViews().AddRazorRuntimeCompilation();



var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");


builder.Services.AddDbContext<EnfocoDb>(options => options.UseNpgsql(connectionString));

// Registrar el servicio NoticeService
builder.Services.AddScoped<NoticeService>(); // Agrega esta línea

builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
.AddCookie(options =>
{
    options.LoginPath = "/Home/Login"; // Ruta para el inicio de sesión
    options.LogoutPath = "/Home/Logout"; // Ruta para el cierre de sesión
});
//script para crear la base de datos
// 1- instalar la dependencia ef
//    dotnet tool install --global dotnet-ef

// 2- crear el first migration
//       dotnet ef migrations add firstmigration

// 3- correr el script que crea la base de datos
// dotnet ef database update firstmigration --project enfoco2.csproj

var app = builder.Build();

// Configure the HTTP request pipeline.


app.UseHttpsRedirection();


//vamos a crear la api web
app.MapPost("/noticias/", async (Notice n, EnfocoDb db) =>
{
    db.Notices.Add(n);
    await db.SaveChangesAsync();

    return Results.Created($"/noticias/{n.Id}", n);
});

app.MapGet("noticias/Detail/{id:int}", async (int id, EnfocoDb db) =>
{
    return await db.Notices.FindAsync(id)
    is Notice n ? Results.Ok(n) : Results.NotFound();
});

app.MapGet("/noticias/", async (EnfocoDb db) => await db.Notices.ToListAsync());


app.MapPut("/noticias/{id:int}", async (int id, Notice n, EnfocoDb db) => {
    if (n.Id != id) return Results.BadRequest();

    var notice = await db.Notices.FindAsync(id);

    if (notice is null) return Results.NotFound();

    notice.Title = n.Title;
    notice.Subtitle = n.Subtitle;
    notice.Issue = n.Issue;
    notice.Text = n.Text;
    notice.Img = n.Img;

    await db.SaveChangesAsync();

    return Results.Ok(notice);
});

app.MapDelete("/noticias/{id:int}", async (int id, EnfocoDb db) => {


    var notice = await db.Notices.FindAsync(id);

    if (notice is null) return Results.NotFound();



    db.Notices.Remove(notice);
    await db.SaveChangesAsync();


    return Results.NoContent();
});



app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

