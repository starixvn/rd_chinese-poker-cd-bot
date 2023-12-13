namespace ChinesePoker.Utilities;

public interface ICombination
{
    List<int> FiveOf39At(int n);

    List<int> ThreeOf39At(int n);

    (List<int> Segment1, List<int> Segment2, List<int> Segment3) HandAt(int n);

    public int FiveOf39Count();

    public int ThreeOf39Count();

    public int HandsCount();
}