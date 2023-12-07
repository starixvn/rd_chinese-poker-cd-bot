namespace ChinesePoker.Utilities;

public interface IScorer
{
    (int Score, int Type) this[IEnumerable<int> ids]
    {
        get;
    }
}