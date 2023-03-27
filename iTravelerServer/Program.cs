using System.Text;
using Automarket;

using iTravelerServer.DAL;
using iTravelerServer.Domain.Helpers;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
var secretKey = builder.Configuration.GetSection("AppSettings:Key").Value;
var key = new SymmetricSecurityKey(Encoding.UTF8
    .GetBytes(secretKey));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    // services.AddAuthentication("Bearer")
    .AddJwtBearer(opt =>
    {
        opt.TokenValidationParameters = new TokenValidationParameters
        {
            ValidateIssuerSigningKey = true,
            ValidateIssuer = false,
            ValidateAudience = false,
            IssuerSigningKey = key
        };
    });
// builder.Services.AddAuthentication(options =>
//     {
//         options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
//         options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
//         
//     })
//     .AddJwtBearer(options =>
//     {
//         //options.RequireHttpsMetadata = true;
//         options.TokenValidationParameters = new TokenValidationParameters
//         {
//             // укзывает, будет ли валидироваться издатель при валидации токена
//             ValidateIssuer = true,
//             // будет ли валидироваться потребитель токена
//             ValidateAudience = true,
//             // будет ли валидироваться время существования
//             ValidateLifetime = true,
//             // валидация ключа безопасности
//             ValidateIssuerSigningKey = true,
//             
//             // строка, представляющая издателя
//             ValidIssuer = "https://localhost:7138",
//             // установка потребителя токена
//             ValidAudience = "https://localhost:7138",
//     
//             // установка ключа безопасности
//             IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes("iTravelerSecretKey"))
//         };
//     });

builder.Services.AddCors(options =>
{
    options.AddDefaultPolicy(
        builder => { builder.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader(); });
});

builder.Services.AddAutoMapper(typeof(AutoMapperProfiles).Assembly);


// Add services to the container.
var connection = builder.Configuration.GetConnectionString("DefaultConnection");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connection));


// builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
//     .AddCookie(options =>
//     {
//         options.LoginPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
//         options.AccessDeniedPath = new Microsoft.AspNetCore.Http.PathString("/Account/Login");
//     });

builder.Services.InitializeRepositories();
builder.Services.InitializeServices();


// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors();
app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();

app.Run();