using ChinesePoker.Bots;
using ChinesePoker.Utilities;

var scorer = new Scorer(dataPath: @"D:\Projects\ChinesePokerBot\BotTest\data\data.json");
var settler = new Settler(scorer);
var combination = new Combination(dataPath: @"D:\Projects\ChinesePokerBot\BotTest\data");
var bot = new CombineBot(scorer, combination, settler);

List<int> input = [31, 3, 4, 21, 18, 48, 0, 9, 2, 16, 19, 39, 38];
var watch = new System.Diagnostics.Stopwatch();
watch.Start();
var suggest = bot.Arrange(input);
watch.Stop();
Console.WriteLine($"Execution Time: {watch.ElapsedMilliseconds} ms");
Console.WriteLine("Done!");