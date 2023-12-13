namespace ChinesePoker.Models;

public class HandInfo
{
    public SegmentCards? Hand { get; set; }

    public string Kind { get; set; } = string.Empty;
    
    public double ProbKind { get; set; }

    public int S1Score { get; set; }

    public int S2Score { get; set; }

    public int S3Score { get; set; }

    public int Score { get; set; }
}