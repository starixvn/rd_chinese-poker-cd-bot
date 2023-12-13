using System.Text.Json.Serialization;
using ChinesePoker.Bots;
using ChinesePoker.Utilities;
using Models;

var builder = WebApplication.CreateSlimBuilder(args);

var dataPath = builder.Configuration.GetValue<string>("DataPath") ?? string.Empty;
var scorer = new Scorer(dataPath: dataPath);
var combination = new Combination();
var settler = new Settler(scorer);
var bot = new CombineBot(scorer, combination, settler);

builder.Services.ConfigureHttpJsonOptions(options =>
{
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, AppJsonSerializerContext.Default);
    options.SerializerOptions.TypeInfoResolverChain.Insert(0, MyContext.Default);
});

var app = builder.Build();

app.MapGet("/health", () => "Server is running!");
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


[JsonSerializable(typeof(MoveRequestModel[]))]
internal partial class AppJsonSerializerContext : JsonSerializerContext
{

}
[JsonSerializable(typeof(Dictionary<string, int[]>[]))]
public partial class MyContext : JsonSerializerContext { }
