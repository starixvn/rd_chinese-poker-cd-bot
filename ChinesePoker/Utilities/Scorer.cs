using System.Text.Json;

namespace ChinesePoker.Utilities;

public class Scorer 
{
    private readonly Dictionary<string, int[]> _dict;

    public Scorer(string dataPath)
    {
        var stream = File.ReadAllText(Path.Combine(dataPath, "scores.json"));
        _dict = JsonSerializer.Deserialize<Dictionary<string, int[]>>(stream) ?? [];
    }
    
    public (int Score, int Type) this[IEnumerable<int> ids]
    {
        get
        {
            var idsSort = ids.Order().ToList();
            var value = _dict[string.Join(".", idsSort)];
            return (value[0], value[1]);
        }
    }
}