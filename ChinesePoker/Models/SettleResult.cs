namespace ChinesePoker.Models;

public class SettleResult
{
    public string PlayerId { get; set; } = string.Empty;

    public SegmentValues RawScores { get; set; } = new SegmentValues();

    public SegmentValues Scores { get; set; } = new SegmentValues();

    public int ScoresSum { get; set; }

    public bool Failed { get; set; }

    public bool Win { get; set; }
}