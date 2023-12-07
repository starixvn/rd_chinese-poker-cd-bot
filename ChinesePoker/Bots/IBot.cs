using ChinesePoker.Models;

namespace ChinesePoker.Bots;

public interface IBot
{
    SegmentCards Arrange(List<int> input);
}