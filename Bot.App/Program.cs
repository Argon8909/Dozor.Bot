using Bot.WebApp;
using Bot.Ws;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
//builder.Services.AddHostedService<Worker>();

// builder.Services.Configure<BotSettings>(builder.Configuration.GetSection("BotSettings"));
// builder.Services.AddHostedService<TelegramBot>();

builder.Services.AddSingleton<TelegramBot>(); // Добавьте эту строку
builder.Services.Configure<BotSettings>(builder.Configuration.GetSection("BotSettings"));
builder.Services.AddHostedService<TelegramBot>();


builder.Services.AddControllers();
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

app.UseAuthorization();

app.MapControllers();

app.Run();