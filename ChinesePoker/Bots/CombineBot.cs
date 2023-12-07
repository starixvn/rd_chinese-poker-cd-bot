using ChinesePoker.Models;
using ChinesePoker.Utilities;

namespace ChinesePoker.Bots;

public class CombineBot
{
    private readonly Scorer _scorer;
    private readonly Combination _combination;
    private readonly Settler _settler;
    private readonly List<int> _segment1Rank;
    private readonly List<int> _segment2Rank;
    private readonly List<int> _segment3Rank;

    public CombineBot(Scorer scorer, Combination combination, Settler settler)
    {
        _settler = settler;
        _scorer = scorer;
        _combination = combination;
        _segment1Rank = [];
        _segment2Rank = [];
        _segment3Rank = [];
    }



    protected void InitComponents(List<int> inputCards)
    {
        HashSet<int> deck = [];
        for (int i = 0; i < 52; i++)
        {
            deck.Add(i);
        }

        var cards = new HashSet<int>(inputCards);

        var remainCards = deck.Except(cards).ToList();

        foreach (var index in _combination.FiveOf39s)
        {
            var item = new List<int>()
            {
                remainCards[index[0]],
                remainCards[index[1]],
                remainCards[index[2]],
                remainCards[index[3]],
                remainCards[index[4]]
            };

            var value = _scorer[item];

            if (value.Type != (int)HandKindFive.HighCard)
            {
                _segment1Rank.Add(value.Score);
            }

            _segment2Rank.Add(value.Score);
        }

        foreach (var index in _combination.ThreeOf39s)
        {
            var item = new List<int>()
            {
                remainCards[index[0]],
                remainCards[index[1]],
                remainCards[index[2]]
            };

            _segment3Rank.Add(_scorer[item].Score);
        }

        _segment1Rank.Sort();
        _segment2Rank.Sort();
        _segment3Rank.Sort();
    }

    protected List<SegmentCards> GetAllValidCases(List<int> inputCards)
    {
        List<SegmentCards> result = [];

        foreach (var hand in _combination.Hands)
        {
            List<int> segment1 = [inputCards[hand[0][0]], inputCards[hand[0][1]],
                                    inputCards[hand[0][2]], inputCards[hand[0][3]], inputCards[hand[0][4]]];
            List<int> segment2 = [inputCards[hand[1][0]], inputCards[hand[1][1]],
                                    inputCards[hand[1][2]], inputCards[hand[1][3]], inputCards[hand[1][4]]];
            List<int> segment3 = [inputCards[hand[2][0]], inputCards[hand[2][1]], inputCards[hand[2][2]]];

            var valueS1 = _scorer[segment1];
            var valueS2 = _scorer[segment2];
            var valueS3 = _scorer[segment3];

            if (valueS1.Score >= valueS2.Score && valueS2.Score >= valueS3.Score && valueS1.Type != (int)HandKindFive.HighCard)
            {
                result.Add(new SegmentCards
                {
                    Segment1 = segment1,
                    Segment2 = segment2,
                    Segment3 = segment3
                });
            }
        }

        return result;
    }

    protected (double segment1, double segment2, double segment3) CalculateBaseScore(SegmentCards player)
    {
        var segment1Score = _scorer[player.Segment1].Score;
        var segment1Win = Settler.CountLower(_segment1Rank, segment1Score);

        var segment2Score = _scorer[player.Segment2].Score;
        var segment2Win = Settler.CountLower(_segment2Rank, segment2Score);

        var segment3Score = _scorer[player.Segment3].Score;
        var segment3Win = Settler.CountLower(_segment3Rank, segment3Score);

        return (segment1Win * 1.0 / _segment1Rank.Count, segment2Win * 1.0 / _segment2Rank.Count,
                segment3Win * 1.0 / _segment3Rank.Count);
    }

    public SegmentCards Arrange(List<int> inputCards)
    {
        InitComponents(inputCards);
        var allCase = GetAllValidCases(inputCards);

        Dictionary<string, double> maxCaseScore = new() { { "kind1", 0 }, { "kind2", 0 }, { "kind3", 0 }, { "kind4", 0 } };
        Dictionary<string, List<SegmentCards>> allCaseScore = new() 
        { 
            { "kind1", new List<SegmentCards>() }, 
            { "kind2", new List<SegmentCards>() }, 
            { "kind3", new List<SegmentCards>() }, 
            { "kind4", new List<SegmentCards>() } 
        };

        var test = new SegmentCards()
        {
            Segment1 = [1, 2, 3, 4, 5],
            Segment2 = [1, 2, 3, 4, 5],
            Segment3 = [1, 2, 3]
        };

        var score_test = CalculateBaseScore(test);

        foreach (var solution in allCase)
        {
            var (s1Score, s2Score, s3Score) = CalculateBaseScore(solution);
            var kind1 = s1Score + s2Score + s3Score;
            var kind2 = s1Score * 1000 + s2Score * 1000 + s3Score;
            var kind3 = s1Score + s2Score * 1000 + s3Score * 1000;
            var kind4 = s1Score * 1000 + s2Score + s3Score * 1000;
            FindMax(ref maxCaseScore, ref allCaseScore, solution, kind1, "kind1");
            FindMax(ref maxCaseScore, ref allCaseScore, solution, kind2, "kind2");
            FindMax(ref maxCaseScore, ref allCaseScore, solution, kind3, "kind3");
            FindMax(ref maxCaseScore, ref allCaseScore, solution, kind4, "kind4");
        }

        var candidates = new List<SegmentCards>();
        candidates.AddRange(allCaseScore["kind1"]);
        candidates.AddRange(allCaseScore["kind2"]);
        candidates.AddRange(allCaseScore["kind3"]);
        candidates.AddRange(allCaseScore["kind4"]);

        Dictionary<string, SegmentCards> candidatesDict = [];
        foreach (var candidate in candidates)
        {
            candidatesDict[candidates.IndexOf(candidate).ToString()] = candidate;
        }

        var settleResult = _settler.Settle(candidatesDict);

        var bestOption = settleResult.MaxBy(x => x.Value.ScoresSum);

        return candidatesDict[bestOption.Key];
    }

    private static void FindMax(ref Dictionary<string, double> maxCaseScore, 
                                    ref Dictionary<string, 
                                    List<SegmentCards>> allCaseScore, 
                                    SegmentCards solution, 
                                    double kind1,
                                    string key)
    {
        if (kind1 > maxCaseScore[key])
        {
            maxCaseScore[key] = kind1;
            allCaseScore[key].Clear();
            allCaseScore[key].Add(solution);
        }
        else if (kind1 == maxCaseScore[key])
        {
            allCaseScore[key].Add(solution);
        }
    }
}