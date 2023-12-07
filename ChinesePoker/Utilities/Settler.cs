using ChinesePoker.Models;

namespace ChinesePoker.Utilities;

public class Settler(Scorer scorer): ISettler
{

    public Dictionary<string, SettleResult> Settle(Dictionary<string, SegmentCards> inputCards)
    {
        List<int> segment1 = [];
        List<int> segment2 = [];
        List<int> segment3 = [];

        Dictionary<string, SegmentValues> segmentValues = [];
        foreach (var item in inputCards)
        {
            var segmentValue = new SegmentValues()
            {
                Segment1 = scorer[item.Value.Segment1].Score,
                Segment2 = scorer[item.Value.Segment2].Score,
                Segment3 = scorer[item.Value.Segment3].Score
            };
            
            if ((segmentValue.Segment1 < segmentValue.Segment2) || (segmentValue.Segment2 < segmentValue.Segment3))
            {
                segmentValue = new SegmentValues() {Segment1 = 0, Segment2 = 0, Segment3 = 0};
            }

            segment1.Add(segmentValue.Segment1);
            segment2.Add(segmentValue.Segment2);
            segment3.Add(segmentValue.Segment3);
            segmentValues.Add(item.Key, segmentValue);
        }

        segment1.Sort();
        segment2.Sort();
        segment3.Sort();

        Dictionary<string, SettleResult> result = [];

        foreach (var item in segmentValues)
        {
            var scores = new SegmentValues() 
            {
                Segment1 = CountLower(segment1, item.Value.Segment1) - CountGreater(segment1, item.Value.Segment1),
                Segment2 = CountLower(segment2, item.Value.Segment2) - CountGreater(segment2, item.Value.Segment2),
                Segment3 = CountLower(segment3, item.Value.Segment3) - CountGreater(segment3, item.Value.Segment3)
            };

            var totalScore = scores.Segment1 + scores.Segment2 + scores.Segment3;

            var settleResult = new SettleResult()
            {
                PlayerId = item.Key,
                RawScores = item.Value,
                Scores = scores,
                ScoresSum = totalScore,
                Failed = item.Value.Segment1 < 0,
                Win = totalScore > 0
            };

            result.Add(item.Key, settleResult);
        }

        return result;
    }

    public static int CountGreater(List<int> list, int value)
    {
        var start = 0;
        var end = list.Count - 1;

        while (start < end)
        {
            var mid = (start + end) / 2;
            if (list[end] <= value)
            {
                return list.Count - end -1;
            }
            if ((start + 1 == end) && (list[start] <= value))
            {
                return list.Count - start - 1;
            }
            if (list[mid] > value)
            {
                end = mid;
            }
            else
            {
                start = mid;
            }
        }

        return list.Count - 1;
    }

    public static int CountLower(List<int> list, int value)
    {
        var start = 0;
        var end = list.Count - 1;

        while (start < end)
        {
            var mid = (start + end) / 2;

            if (list[end] < value)
            {
                return end + 1;
            }
            
            if ((start + 1 == end) && (list[start] < value))
            {
                return start + 1;
            }

            if (list[mid] >= value)
            {
                end = mid;
            }
            else
            {
                start = mid;
            }
        }

        return 0;
    }
}