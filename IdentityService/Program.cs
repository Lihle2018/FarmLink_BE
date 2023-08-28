using FarmLink.Shared.Services;
using IdentityService.Data;
using IdentityService.Data.Interfaces;
using IdentityService.Repositories;
using IdentityService.Repositories.Interfaces;
using IdentityService.Services;
var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IUserRepository, UserRepository>();
builder.Services.AddTransient<IEmailService, EmailService>();
builder.Services.AddTransient<IHashingService, HashingService>();
builder.Services.AddScoped<IFarmLinkContext,FarmLinkContext>();
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();
app.UseCors("corsapp");

app.UseAuthorization();

app.MapControllers();

app.Run();
