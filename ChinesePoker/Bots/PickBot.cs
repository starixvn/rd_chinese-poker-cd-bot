using ChinesePoker.Models;
using ChinesePoker.Utilities;

namespace ChinesePoker.Bots;



public class PickBot : IBot
{
    const double SAMPLE_5 = 2598960;
    const double SAMPLE_3 = 22100;
    private readonly IScorer _scorer;
    private readonly ICombination _combination;
    private readonly ISettler _settler;
    private readonly Dictionary<int, double> _probType;


    public PickBot(Scorer scorer, Combination combination, Settler settler)
    {
        _settler = settler;
        _scorer = scorer;
        _combination = combination;
        _probType = new() {
         { (int)HandKind.HighCard5, 1302540/SAMPLE_5 },
         { (int)HandKind.Pair5, 1098240/SAMPLE_5 },
         { (int)HandKind.TwoPair5, 123552/SAMPLE_5 },
         { (int)HandKind.ThreeOfKind5, 54912/SAMPLE_5 },
         { (int)HandKind.Straight5, 10200/SAMPLE_5 },
         { (int)HandKind.Flush5, 5108/SAMPLE_5 },
         { (int)HandKind.FullHouse5, 3744/SAMPLE_5 },
         { (int)HandKind.FourOfKind5, 624/SAMPLE_5 },
         { (int)HandKind.StraightFlush5, 40/SAMPLE_5 },
         { (int)HandKind.HighCard3, 18304/SAMPLE_3 },
         { (int)HandKind.Pair3, 3744/SAMPLE_3 },
         { (int)HandKind.ThreeOfKind3, 52/SAMPLE_3 }
     };
    }

    public SegmentCards Arrange(List<int> input)
    {
        var allCase = ProduceHands(input);
        var temp = allCase.OrderBy(x => x.Value[0].ProbKind).First();
        var candidates = temp.Value.ToDictionary(x => $"{temp.Value.IndexOf(x)}", x => x.Hand ?? new SegmentCards());
        var settleResult = _settler.Settle(candidates);
        var bestOption = settleResult.MaxBy(x => x.Value.ScoresSum);
        return candidates[bestOption.Key];
    }

    private Dictionary<string, List<HandInfo>> ProduceHands(List<int> input)
    {
        Dictionary<string, List<HandInfo>> allCase = [];

        for (var i = 0; i < _combination.HandsCount(); i++)
        {
            var (segment1, segment2, segment3) = _combination.HandAt(i);
            var segmentCards = new SegmentCards()
            {
                Segment1 = segment1.Select(x => input[x]).ToList(),
                Segment2 = segment2.Select(x => input[x]).ToList(),
                Segment3 = segment3.Select(x => input[x]).ToList()
            };

            var valueS1 = _scorer[segmentCards.Segment1];
            var valueS2 = _scorer[segmentCards.Segment2];
            var valueS3 = _scorer[segmentCards.Segment3];

            segmentCards.S1Score = valueS1.Score;
            segmentCards.S2Score = valueS2.Score;
            segmentCards.S3Score = valueS3.Score;

            if ((valueS1.Score >= valueS2.Score) && (valueS2.Score >= valueS3.Score) && valueS1.Type != (int)HandKind.HighCard5)
            {
                var key = $"{valueS1.Type}.{valueS2.Type}.{valueS3.Type}";
                if (allCase.TryGetValue(key, out List<HandInfo>? value))
                {
                    value.Add(new HandInfo
                    {
                        Hand = segmentCards,
                        Kind = key,
                        ProbKind = _probType[valueS1.Type] * _probType[valueS2.Type] * _probType[valueS3.Type],
                        S1Score = valueS1.Score,
                        S2Score = valueS2.Score,
                        S3Score = valueS3.Score,
                        Score = valueS1.Score + valueS2.Score + valueS3.Score
                    });
                }
                else
                {
                    allCase.Add(key, [ new HandInfo
                    {
                        Hand = segmentCards,
                        Kind = key,
                        ProbKind = _probType[valueS1.Type]*_probType[valueS2.Type]*_probType[valueS3.Type],
                        S1Score = valueS1.Score,
                        S2Score = valueS2.Score,
                        S3Score = valueS3.Score,
                        Score = valueS1.Score + valueS2.Score + valueS3.Score
                    }]);
                }
            }
        }

        return allCase;
    }
}