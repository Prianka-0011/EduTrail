using EduTrail.Infrastructure;
using EduTrail.Application;
using EduTrail.Infrastructure.Data;
using Microsoft.EntityFrameworkCore;
using EduTrail.API.Middlewares;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddControllers();
builder.Services.AddApplication();
builder.Services.AddInfrastructure(builder.Configuration);
builder.Services.AddCors(option=> option.AddPolicy("AllowWebSpa", policy =>
{
    policy.WithOrigins("http://localhost:4200")
          .AllowAnyHeader()
          .AllowAnyMethod();
}));

builder.Services.AddSpaStaticFiles(configuration =>
{
    configuration.RootPath = System.IO.Path.Combine(builder.Environment.ContentRootPath, "..", "..", "WebSpa", "dist", "web-spa");
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
app.UseGlobalExceptionHandler();
// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("AllowWebSpa");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseSpaStaticFiles();
app.UseSpa(spa =>
{
    spa.Options.SourcePath = System.IO.Path.Combine("..", "..", "WebSpa");
});
app.Run();


