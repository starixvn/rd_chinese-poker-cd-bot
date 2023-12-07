using System.Text.Json;

namespace ChinesePoker.Utilities;

public class Combination: ICombination
{
    public List<List<int>> FiveOf39s { get; }
    public List<List<int>> ThreeOf39s { get; }
    public List<List<List<int>>> Hands { get; }

    public Combination(string dataPath)
    {
        var dataHand = File.ReadAllText(Path.Combine(dataPath, "5_5_3.json"));
        var dataFive = File.ReadAllText(Path.Combine(dataPath, "5_of_39.json"));
        var dataThree = File.ReadAllText(Path.Combine(dataPath, "3_of_39.json"));

        Hands = JsonSerializer.Deserialize<List<List<List<int>>>>(dataHand) ?? [];
        FiveOf39s = JsonSerializer.Deserialize<List<List<int>>>(dataFive) ?? [];
        ThreeOf39s = JsonSerializer.Deserialize<List<List<int>>>(dataThree) ?? [];
    }
}