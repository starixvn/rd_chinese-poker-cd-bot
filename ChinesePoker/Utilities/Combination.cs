using System.Text.Json;

namespace ChinesePoker.Utilities;

public class Combination: ICombination
{
    private readonly List<List<int>> _fiveOf39s;
    private readonly List<List<int>> _threeOf39s;
    private readonly List<(List<int> Segment1, List<int> Segment2, List<int> Segment3)> _hands;

    public Combination()
    {
        _fiveOf39s = CombinationCalc(39, 5);
        _threeOf39s = CombinationCalc(39, 3);
        _hands = MakeHands();
    }

    public int FiveOf39Count() => _fiveOf39s.Count;

    public int ThreeOf39Count() => _threeOf39s.Count;

    public int HandsCount() => _hands.Count;

    public List<int> FiveOf39At(int n)
    {
        if (n >= _fiveOf39s.Count)
        {
            return [];
        }

        return _fiveOf39s[n];
    }

    public List<int> ThreeOf39At(int n)
    {
        if (n >= _threeOf39s.Count)
        {
            return [];
        }

        return _fiveOf39s[n];
    }

    public (List<int> Segment1, List<int> Segment2, List<int> Segment3) HandAt(int n)
    {
        if (n >= _hands.Count)
        {
            return ([], [], []);
        }

        return _hands[n];
    }

    public List<(List<int> Segment1, List<int> Segment2, List<int> Segment3)> MakeHands()
    {
        List<int> arr = [];
        for (var i = 0; i < 13; i++)
        {
            arr.Add(i);
        }

        List<(List<int> Segment1, List<int> Segment2, List<int> Segment3)> result = [];

        var combination1 = CombinationCalc(13, 5);
        var combination2 = CombinationCalc(8, 5);


        foreach (var item in combination1)
        {
            var segment1 = new List<int>(){arr[item[0]], arr[item[1]], arr[item[2]], arr[item[3]], arr[item[4]]};
            var remain = new HashSet<int>(arr).Except(new HashSet<int>(segment1)).ToList();
            foreach (var item2 in combination2)
            {
                var segment2 = new List<int>(){remain[item2[0]], remain[item2[1]], remain[item2[2]], remain[item2[3]], remain[item2[4]]};
                var segment3 = new HashSet<int>(remain).Except(new HashSet<int>(segment2)).ToList();
                result.Add((segment1, segment2, segment3));
            }
        }

        return result;
    }


    public List<List<int>> CombinationCalc(int n, int k)
    {
        List<int> arr = [];
        for (var i = 0; i < n; i++)
        {
            arr.Add(i);
        }

        var maxDict = new Dictionary<int, int>();
        for (var i = 0; i < k; i++)
        {
            maxDict[i] = arr.Count - k + i;
        }

        var initArr = maxDict.Keys.ToList();
        var index = initArr.Count - 1;
        var result = new List<List<int>>();
        var flagOut = false;
        while (true)
        {
            result.Add(new List<int>(initArr.Select(x => arr[x])));
            initArr[index]++;
            if (initArr[index] > maxDict[index])
            {
                while (initArr[index] >= maxDict[index])
                {
                    index--;
                    if (index < 0)
                    {
                        flagOut = true;
                        break;
                    }               
                }
                if (flagOut) break;
                initArr[index]++;
                for (var i = index + 1; i < initArr.Count; i++)
                {
                    initArr[i] = initArr[i - 1] + 1;
                }
                index = initArr.Count - 1;
            }
        }

        return result;
    }
}