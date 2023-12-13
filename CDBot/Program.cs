using ChinesePoker.Bots;
using ChinesePoker.Utilities;

var builder = WebApplication.CreateBuilder(args);


var dataPath = builder.Configuration.GetValue<string>("DataPath") ?? string.Empty;
var scorer = new Scorer(dataPath: dataPath);
builder.Services.AddSingleton<IScorer>(scorer);

var combination = new Combination();
builder.Services.AddSingleton<ICombination>(combination);

builder.Services.AddSingleton<IBot>(new PickBot(scorer, combination));

builder.Services.AddControllers();

const string ALLOWALLPOLICY = "AllowAll";

builder.Services.AddCors(options => 
{
  options.AddPolicy(ALLOWALLPOLICY, policy =>
  {
    policy
      .AllowAnyOrigin()
      .AllowAnyMethod()
      .AllowAnyHeader();
  });
});

var app = builder.Build();

app.UsePathBase(app.Configuration.GetValue<string>("PathBase"));
app.UseHttpsRedirection();
app.UseCors(ALLOWALLPOLICY);


app.MapControllers();

app.Run();

