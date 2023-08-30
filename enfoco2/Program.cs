using enfoco2.Data;
using enfoco2.Models;
using enfoco2.Services;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();


var connectionString = builder.Configuration.GetConnectionString("PostgreSQLConnection");

builder.Services.AddDbContext<EnfocoDb>(options => options.UseNpgsql(connectionString));

// Registrar el servicio NoticeService
builder.Services.AddScoped<NoticeService>(); // Agrega esta línea

//script para crear la base de datos
// 1- instalar la dependencia ef
//    dotnet tool install --global dotnet-ef

// 2- crear el first migration
//       dotnet ef migrations add firstmigration

// 3- correr el script que crea la base de datos
// dotnet ef database update firstmigration --project enfoco2.csproj

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();


//vamos a crear la api web
app.MapPost("Home/noticias/", async (Notice n, EnfocoDb db) =>
{
    db.Notices.Add(n);
    await db.SaveChangesAsync();

    return Results.Created($"/noticia/{n.Id}", n);
});

app.MapGet("/detalle/{id:int}", async (int id, EnfocoDb db) =>
{
    return await db.Notices.FindAsync(id)
    is Notice n ? Results.Ok(n) : Results.NotFound();
});

app.MapGet("Home/noticias/", async (EnfocoDb db) => await db.Notices.ToListAsync());


app.MapPut("Home/noticias/{id:int}", async (int id, Notice n, EnfocoDb db) => {
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

app.MapDelete("Home/noticias/{id:int}", async (int id, EnfocoDb db) => {


    var notice = await db.Notices.FindAsync(id);

    if (notice is null) return Results.NotFound();



    db.Notices.Remove(notice);
    await db.SaveChangesAsync();


    return Results.NoContent();
});



app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");

app.Run();

