using SistemaSolicitudesLaDat.Repository.Infrastructure;
using SistemaSolicitudesLaDat.Repository.Login;
using SistemaSolicitudesLaDat.Repository.Usuarios;
using SistemaSolicitudesLaDat.Service.Abstract;
using SistemaSolicitudesLaDat.Service.Encriptado;
using SistemaSolicitudesLaDat.Service.Login;
using SistemaSolicitudesLaDat.Service.Seguridad;
using SistemaSolicitudesLaDat.Service.Usuarios;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddSingleton<IDbConnectionFactory, DbConnectionFactory>();

// Repositorios
builder.Services.AddScoped<UsuarioRepository>();
builder.Services.AddScoped<LoginRepository>(); 

// Servicios
builder.Services.AddScoped<IUsuarioService, UsuarioService>();
builder.Services.AddScoped<IEncriptadoService, EncriptadoService>();
builder.Services.AddScoped<ISeguridadService, SeguridadService>();
builder.Services.AddScoped<LoginService>();
builder.Services.AddScoped<EncriptadoService>();

builder.Services.AddAuthentication("Cookies")
    .AddCookie("Cookies", options =>
    {
        options.LoginPath = "/Index"; // Página donde redirige si no hay sesión
        //options.AccessDeniedPath = "/AccessDenied"; // Opcional
        options.SlidingExpiration = true;
    });

// Para restringir la autenticación y autorización de usuarios
builder.Services.AddAuthorization();

builder.Services.AddRazorPages(options =>
{
    // Para proteger toda la carpeta /Usuarios
    options.Conventions.AuthorizeFolder("/Usuarios");

    // Para permitir acceso anónimo al login
    options.Conventions.AllowAnonymousToPage("/Login");
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication(); // Añadir autenticación antes de autorización
app.UseAuthorization();

app.MapRazorPages();

app.Run();
