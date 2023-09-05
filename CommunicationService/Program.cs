using CommunicationService.Data;
using CommunicationService.Data.Interfaces;
using CommunicationService.Hubs;
using CommunicationService.Repositories;
using CommunicationService.Repositories.Interfaces;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddTransient<IMessageChatRepository, MessageChatRepository>();
builder.Services.AddScoped<IFarmlinkContext, FarmLinkContext>();
builder.Services.AddCors(p => p.AddPolicy("corsapp", builder =>
{
    builder.WithOrigins("*").AllowAnyMethod().AllowAnyHeader();
}));
builder.Services.AddSignalR();

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
app.MapHub<ChatHub>("/chatHub");
app.MapControllers();

app.Run();
