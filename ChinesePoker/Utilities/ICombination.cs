namespace ChinesePoker.Utilities;

public interface ICombination
{
    List<List<int>> FiveOf39s { get; }
    List<List<int>> ThreeOf39s { get; }
    List<List<List<int>>> Hands { get; }
}