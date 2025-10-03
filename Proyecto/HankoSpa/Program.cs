using Microsoft.EntityFrameworkCore;
using HankoSpa.Data;
using HankoSpa.Helpers;
using HankoSpa.Repository;
using HankoSpa.Services;
using HankoSpa.Services.Interfaces;
using HankoSpa.Models;
using Microsoft.AspNetCore.Identity;
using HankoSpa.Data.Seeder;
using AspNetCoreHero.ToastNotification;
using AspNetCoreHero.ToastNotification.Extensions;
using AspNetCoreHero.ToastNotification.Notyf;
using HankoSpa.C4; // 👈 NUEVO: Importamos la carpeta donde está C4Generator.
using System;
// NOTA: Se han eliminado 'using Structurizr;' y 'using Structurizr.Api;'

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddDbContext<AppDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

builder.Services.AddHttpContextAccessor();

builder.Services.AddTransient<SeedDb>();
builder.Services.AddScoped<IUserClaimsPrincipalFactory<User>, CustomUserClaimsPrincipalFactory>();
builder.Services.AddControllersWithViews();
builder.Services.AddScoped<ICitaRepository, CitaRepository>();
builder.Services.AddScoped<ICitaServices, CitasService>();
builder.Services.AddScoped<IServicioRepository, ServicioRepository>();
builder.Services.AddScoped<IServicioServices, ServicioService>();
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IUserService, UserService>();
builder.Services.AddScoped<ICustomRolRepository, CustomRolRepository>();
builder.Services.AddScoped<ICustomRolService, CustomRolService>();
builder.Services.AddAutoMapper(typeof(AutoMapperProfile));
builder.Services.AddScoped<ICombosHelper, CombosHelper>();

// Registro del repositorio PermissionRepository
builder.Services.AddScoped<IPermissionRepository, PermissionRepository>();

// Registro del servicio PermissionService
builder.Services.AddScoped<IPermissionService, PermissionService>();

// Registro del servicio Notyf
builder.Services.AddNotyf(config =>
{
    config.DurationInSeconds = 5;
    config.IsDismissable = true;
    config.Position = NotyfPosition.TopRight;
});

builder.Services.AddIdentity<User, IdentityRole>(x =>
{
    x.User.RequireUniqueEmail = true;
    x.Password.RequireDigit = false;
    x.Password.RequiredUniqueChars = 0;
    x.Password.RequireLowercase = false;
    x.Password.RequireNonAlphanumeric = false;
    x.Password.RequireUppercase = false;
})
    .AddEntityFrameworkStores<AppDbContext>()
    .AddDefaultTokenProviders();

// Configuración de la ruta de acceso denegado
builder.Services.ConfigureApplicationCookie(options =>
{
    options.AccessDeniedPath = "/Account/AccessDenied";
});

// Políticas de autorización personalizadas
builder.Services.AddAuthorization(options =>
{
    // Políticas para Servicios, Citas y Usuarios (sin cambios)
    options.AddPolicy("Servicios_Create", policy => policy.RequireClaim("CustomRolId", "1", "4", "5"));
    options.AddPolicy("Servicios_Read", policy => policy.RequireClaim("CustomRolId", "1", "2", "3", "4", "5"));
    options.AddPolicy("Servicios_Update", policy => policy.RequireClaim("CustomRolId", "1", "4", "5"));
    options.AddPolicy("Servicios_Delete", policy => policy.RequireClaim("CustomRolId", "1", "4", "5"));

    options.AddPolicy("Citas_Create", policy => policy.RequireClaim("CustomRolId", "1", "3", "4", "5"));
    options.AddPolicy("Citas_Read", policy => policy.RequireClaim("CustomRolId", "1", "2", "3", "4", "5"));
    options.AddPolicy("Citas_Update", policy => policy.RequireClaim("CustomRolId", "1", "3", "4", "5"));
    options.AddPolicy("Citas_Delete", policy => policy.RequireClaim("CustomRolId", "1", "4", "5"));

    options.AddPolicy("Usuarios_Create", policy => policy.RequireClaim("CustomRolId", "1", "4", "5"));
    options.AddPolicy("Usuarios_Read", policy => policy.RequireClaim("CustomRolId", "1", "2", "4", "5"));
    options.AddPolicy("Usuarios_Update", policy => policy.RequireClaim("CustomRolId", "1", "4", "5"));
    options.AddPolicy("Usuarios_Delete", policy => policy.RequireClaim("CustomRolId", "1", "4", "5"));

    // Política unificada para todas las acciones de Usuario
    options.AddPolicy("UsuarioCRUD", policy => policy.RequireClaim("CustomRolId", "1", "4", "5"));

    // Política unificada para todas las acciones de Rol
    options.AddPolicy("RolCRUD", policy => policy.RequireClaim("CustomRolId", "1", "4", "5"));
    options.AddPolicy("Rol_Read", policy => policy.RequireClaim("CustomRolId", "1", "4", "5"));

    // Política agregada para Permiso_Read
    options.AddPolicy("Permiso_Read", policy => policy.RequireClaim("CustomRolId", "1", "4", "5"));

    // Política agregada para PermisoCRUD (crear, editar, ver, eliminar permisos)
    options.AddPolicy("PermisoCRUD", policy => policy.RequireClaim("CustomRolId", "1", "4", "5"));
});

var app = builder.Build();

// Primero migrar y crear roles/permisos
using (var scope = app.Services.CreateScope())
{
    var db = scope.ServiceProvider.GetRequiredService<AppDbContext>();
    db.Database.Migrate();
    await AppDbContextSeed.SeedAsync(db);
}

// Lógica de siembra de datos asíncrona
async Task SeedDataAsync(WebApplication app)
{
    var scopedFactory = app.Services.GetService<IServiceScopeFactory>();
    using var scope = scopedFactory!.CreateScope();
    var service = scope.ServiceProvider.GetService<SeedDb>();
    await service!.SeedAsync();
}
await SeedDataAsync(app);

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Home/Error");
    app.UseHsts();
}

app.UseHttpsRedirection();
app.UseStaticFiles();
app.UseRouting();
app.UseAuthentication();
app.UseAuthorization();

// Middleware de Notyf
app.UseNotyf();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");


// ========== LLAMADA A LA GENERACIÓN DE DIAGRAMAS C4 ==========
// Toda la lógica de Structurizr se ha movido al archivo C4Generator.cs
// Línea 146
HankoSpa.C4.C4Generator.GenerateAndUpload();
// ============================================================

app.Run();