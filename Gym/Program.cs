using Gym.Servicios;
using Microsoft.AspNetCore.Authentication.Cookies;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllersWithViews();
builder.Services.AddTransient<IRepositorioUsuarios, RepositorioUsuarios>();
builder.Services.AddTransient<IRepositorioMensualidades, RepositorioMensualidades>();
builder.Services.AddTransient<IRepositorioPagos, RepositorioPagos>();
builder.Services.AddTransient<IRepositorioDashboard, RepositrorioDashboard>();
builder.Services.AddTransient<IRepositorioHome, RepositorioHome>();
builder.Services.AddTransient<IRepositorioEntrenadores, RepositorioEntrenadores>();
builder.Services.AddTransient<IRepositorioLogin, RepositorioLogin>();
builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
    .AddCookie(options =>
    {
        options.LoginPath = "/Auth/Login";   // Página de login
        options.LogoutPath = "/Auth/Logout"; // Página de logout
        options.AccessDeniedPath = "/Home/AccesoDenegado"; // Página si no tiene permisos
    });

builder.Services.AddAuthorization();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Auth}/{action=Login}/{id?}");

app.Run();

app.UseAuthentication(); // ✅ Asegurar que ASP.NET maneje la autenticación
app.UseAuthorization();


