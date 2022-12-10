namespace GetBusy.ChuckNorrisApi.Tests;

public class InMemoryHistoryTest
{
    [Fact]
    public void HasNext_WhenInitialized_ReturnsFalse()
    {
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>();
        Assert.False(inMemoryHistory.HasNext);
    }

    [Fact]
    public void HasPrevious_WhenInitialized_ReturnsFalse()
    {
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>();
        Assert.False(inMemoryHistory.HasPrevious);
    }

    [Fact]
    public void HasPrevious_WhenAppendedOnce_ReturnsFalse()
    {
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>();
        inMemoryHistory.Append("test");
        Assert.False(inMemoryHistory.HasPrevious);
    }

    [Fact]
    public void HasNext_WhenAppendedOnce_ReturnsFalse()
    {
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>();
        inMemoryHistory.Append("test");
        Assert.False(inMemoryHistory.HasNext);
    }

    [Fact]
    public void HasPrevious_WhenAppendedTwice_ReturnsTrue()
    {
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>();
        inMemoryHistory.Append("test");
        inMemoryHistory.Append("test2");
        Assert.True(inMemoryHistory.HasPrevious);
    }

    [Fact]
    public void HasNext_WhenAppended_ReturnsFalse()
    {
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>();
        inMemoryHistory.Append("test");
        Assert.False(inMemoryHistory.HasNext);
    }

    [Fact]
    public void HasNext_WhenAppendedAndPrevious_ReturnsTrue()
    {
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>();
        inMemoryHistory.Append("test");
        inMemoryHistory.Append("test2");
        inMemoryHistory.Previous();
        Assert.True(inMemoryHistory.HasNext);
    }

    [Fact]
    public void Next_WhenAppendedTwice_ReturnsLastResult()
    {
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>();
        inMemoryHistory.Append("test");
        inMemoryHistory.Append("test2");
        string? result = inMemoryHistory.Next();
        Assert.Equal("test2", result);
    }

    [Fact]
    public void Previous_WhenAppendedTwice_ReturnsFirstResult()
    {
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>();
        inMemoryHistory.Append("test");
        inMemoryHistory.Append("test2");
        string? result = inMemoryHistory.Previous();
        Assert.Equal("test", result);
    }

    [Fact]
    public void Append_WhenAppended_MovesCursorToEnd()
    {
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>();
        inMemoryHistory.Append("test");
        inMemoryHistory.Append("test2");
        inMemoryHistory.Append("test3");
        inMemoryHistory.Previous();
        string? result = inMemoryHistory.Previous();
        Assert.Equal("test", result);
        inMemoryHistory.Append("test4");
        result = inMemoryHistory.Previous();
        Assert.Equal("test3", result);
    }

}