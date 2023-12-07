using ChinesePoker.Models;

namespace ChinesePoker.Utilities;

public interface ISettler
{
    Dictionary<string, SettleResult> Settle(Dictionary<string, SegmentCards> inputCards);
}