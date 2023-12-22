using Bot.BLL;
using Bot.WebApp;
using Bot.Ws;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.Configure<BotSettings>(builder.Configuration.GetSection("BotSettings"));
builder.Services.AddSingleton<TelegramBot>(); 
builder.Services.AddSingleton<IBotService, BotService>();
// builder.Services.AddSingleton<IEngine, Engine>();
builder.Services.AddHostedService<TelegramBot>();
builder.Services.AddHostedService<Engine>();
// builder.Services.AddSingleton<System.Threading.Timer>();
// builder.Services.AddSingleton<System.Threading.Timer>(_ => new System.Threading.Timer(_ => { }, null, Timeout.Infinite, Timeout.Infinite));


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