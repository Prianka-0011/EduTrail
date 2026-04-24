using EduTrail.Infrastructure;
using EduTrail.Application;
using EduTrail.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using EduTrail.API.Middlewares;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using static EduTrail.Shared.CustomCategory;
using EduTrail.API.Hubs;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddSignalR();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();

builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);

builder.Services
    .AddHttpContextAccessor()
    .AddAuthorization()
    .AddCors(option => option.AddPolicy("AllowWebSpa", policy =>
    {
        policy.WithOrigins("http://localhost:4200", "http://localhost:5001")
              .AllowAnyHeader()
              .AllowCredentials()
              .AllowAnyMethod();
    }));

builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
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
        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"])
        )
    };

    options.Events = new JwtBearerEvents
    {
        OnMessageReceived = context =>
        {
            var token = context.Request.Cookies[AuthsVariable.AuthTokenName];

            if (!string.IsNullOrEmpty(token))
            {
                context.Token = token;
            }

            return Task.CompletedTask;
        }
    };
});

static void UpdateDatabase(IApplicationBuilder app)
{
    using var serviceScope = app.ApplicationServices
        .GetRequiredService<IServiceScopeFactory>()
        .CreateScope();

    using var context = serviceScope.ServiceProvider
        .GetService<AppDbContext>()
        ?? throw new Exception("AppDbContext service not found");

    context.Database.Migrate();
}

var app = builder.Build();

UpdateDatabase(app);

// Swagger only in dev
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseGlobalExceptionHandler();

app.UseHttpsRedirection();

// IMPORTANT: serve Angular from wwwroot
app.UseDefaultFiles();
app.UseStaticFiles();

app.UseCors("AllowWebSpa");

app.UseAuthentication();
app.UseAuthorization();

app.MapHub<ChatHub>("/hubs/chat");

app.MapControllers();

app.Run();

