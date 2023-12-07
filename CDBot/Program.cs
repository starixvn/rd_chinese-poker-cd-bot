using ChinesePoker.Bots;
using ChinesePoker.Utilities;
using Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UsePathBase(app.Configuration.GetValue<string>("PathBase"));
app.UseHttpsRedirection();

var dataPath = app.Configuration.GetValue<string>("DataPath") ?? string.Empty;
var scorer = new Scorer(dataPath: dataPath);
var settler = new Settler(scorer);
var combination = new Combination(dataPath: dataPath);
var bot = new CombineBot(scorer, combination, settler);


app.MapGet("/health", () => "Server is running");
app.MapPost("/move", (MoveRequestModel request) => 
{

    if (request.Private is null || request.Private.Input is null)
    {
        return null;
    }
    
    var result = bot.Arrange(request.Private.Input);
    request.Private.Output = result;
    return request.Private;
});

app.Run();

