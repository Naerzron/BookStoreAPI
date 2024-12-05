using System.Text;
using BookStore.API.Data;
using BookStore.API.Identity;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(builder.Configuration.GetConnectionString("DefaultConnection")));

// Configura Identity
builder.Services
    .AddIdentity<MyUser, IdentityRole>(options =>
    {
        options.Password.RequireDigit = true;
        options.Password.RequireLowercase = true;
        options.Password.RequireNonAlphanumeric = false;
        options.Password.RequiredLength = 8;
        options.SignIn.RequireConfirmedEmail = false;
        options.Lockout.AllowedForNewUsers = true;
        options.Lockout.MaxFailedAccessAttempts = 5;
    })
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddCors(options =>
{
    options.AddPolicy("AutorizeReactApp",
        policyBuilder =>
        {
            policyBuilder.AllowAnyOrigin()
                         .AllowAnyHeader()
                         .AllowAnyMethod();
        });
});

builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Secret"] ?? ""))
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            if (context.Request.Cookies.ContainsKey("jwt"))
            {
                context.Token = context.Request.Cookies["jwt"];
            }
            return Task.CompletedTask;
        }
    };
});

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

// Agrega autenticación y autorización en el pipeline
app.UseCors("AutorizeReactApp");
app.UseAuthentication(); // <--- Asegúrate de que UseAuthentication esté antes de UseAuthorization
app.UseAuthorization();
app.MapControllers();

// Llamada para crear roles al iniciar
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    await SeedAdminUserAndRoles(services);
}

app.Run();

// Método para inicializar roles y usuario administrador
async Task SeedAdminUserAndRoles(IServiceProvider serviceProvider)
{
    var roleManager = serviceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = serviceProvider.GetRequiredService<UserManager<MyUser>>();

    string[] roleNames = { "Administrador", "Usuario" };
    foreach (var roleName in roleNames)
    {
        if (!await roleManager.RoleExistsAsync(roleName))
        {
            await roleManager.CreateAsync(new IdentityRole(roleName));
        }
    }

    // Crear usuario administrador predeterminado si no existe
    var adminEmail = builder.Configuration["AdminEmail"] ?? "ajlopezcobo@outlook.es";
    var adminPassword = builder.Configuration["AdminPassword"] ?? "Abbyl40l!"; // Ajusta la contraseña según sea necesario
    var adminUser = await userManager.FindByEmailAsync(adminEmail);
    if (adminUser == null)
    {
        var newAdminUser = new MyUser
        {
            UserName = "AdminUser",
            Email = adminEmail,
            EmailConfirmed = true,
            Address = "Una dirección falsa del administrador",
            BirthDate = new DateOnly(1996, 3, 7),
            Country = "ES",
            Dni = "12345678A",
            LastName = "López Cobo",
            Name = "Antonio José",
            PhoneNumber = "123456789"
        };

        var adminResult = await userManager.CreateAsync(newAdminUser, adminPassword);
        if (adminResult.Succeeded)
        {
            await userManager.AddToRoleAsync(newAdminUser, "Administrador");
        }
    }
}