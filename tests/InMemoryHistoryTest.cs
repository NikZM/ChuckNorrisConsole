using Microsoft.Extensions.Configuration;

namespace GetBusy.ChuckNorrisApi.Tests;

public class InMemoryHistoryTest
{
    [Fact]
    public void HasNext_WhenInitialized_ReturnsFalse()
    {
        var configuration = new Mock<IConfiguration>();
        // Moq is unable to stub extension methods however looking at the source code
        // on github shows it leverages GetSection which is declared in the standard interface
        var section = new Mock<IConfigurationSection>();
        section.SetupGet(x => x.Value).Returns(string.Empty);
        configuration.Setup(x => x.GetSection("maxHistory")).Returns(section.Object);
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>(configuration.Object);
        Assert.False(inMemoryHistory.HasNext);
    }

    [Fact]
    public void HasPrevious_WhenInitialized_ReturnsFalse()
    {
        var configuration = new Mock<IConfiguration>();
        var section = new Mock<IConfigurationSection>();
        section.SetupGet(x => x.Value).Returns(string.Empty);
        configuration.Setup(x => x.GetSection("maxHistory")).Returns(section.Object);
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>(configuration.Object);
        Assert.False(inMemoryHistory.HasPrevious);
    }

    [Fact]
    public void HasPrevious_WhenAppendedOnce_ReturnsFalse()
    {
        var configuration = new Mock<IConfiguration>();
        var section = new Mock<IConfigurationSection>();
        section.SetupGet(x => x.Value).Returns(string.Empty);
        configuration.Setup(x => x.GetSection("maxHistory")).Returns(section.Object);
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>(configuration.Object);
        inMemoryHistory.Append("test");
        Assert.False(inMemoryHistory.HasPrevious);
    }

    [Fact]
    public void HasNext_WhenAppendedOnce_ReturnsFalse()
    {
        var configuration = new Mock<IConfiguration>();
        var section = new Mock<IConfigurationSection>();
        section.SetupGet(x => x.Value).Returns(string.Empty);
        configuration.Setup(x => x.GetSection("maxHistory")).Returns(section.Object);
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>(configuration.Object);
        inMemoryHistory.Append("test");
        Assert.False(inMemoryHistory.HasNext);
    }

    [Fact]
    public void HasPrevious_WhenAppendedTwice_ReturnsTrue()
    {
        var configuration = new Mock<IConfiguration>();
        var section = new Mock<IConfigurationSection>();
        section.SetupGet(x => x.Value).Returns(string.Empty);
        configuration.Setup(x => x.GetSection("maxHistory")).Returns(section.Object);
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>(configuration.Object);
        inMemoryHistory.Append("test");
        inMemoryHistory.Append("test2");
        Assert.True(inMemoryHistory.HasPrevious);
    }

    [Fact]
    public void HasNext_WhenAppended_ReturnsFalse()
    {
        var configuration = new Mock<IConfiguration>();
        var section = new Mock<IConfigurationSection>();
        section.SetupGet(x => x.Value).Returns(string.Empty);
        configuration.Setup(x => x.GetSection("maxHistory")).Returns(section.Object);
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>(configuration.Object);
        inMemoryHistory.Append("test");
        Assert.False(inMemoryHistory.HasNext);
    }

    [Fact]
    public void HasNext_WhenAppendedAndPrevious_ReturnsTrue()
    {
        var configuration = new Mock<IConfiguration>();
        var section = new Mock<IConfigurationSection>();
        section.SetupGet(x => x.Value).Returns(string.Empty);
        configuration.Setup(x => x.GetSection("maxHistory")).Returns(section.Object);
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>(configuration.Object);
        inMemoryHistory.Append("test");
        inMemoryHistory.Append("test2");
        inMemoryHistory.Previous();
        Assert.True(inMemoryHistory.HasNext);
    }

    [Fact]
    public void Next_WhenAppendedTwice_ReturnsLastResult()
    {
        var configuration = new Mock<IConfiguration>();
        var section = new Mock<IConfigurationSection>();
        section.SetupGet(x => x.Value).Returns(string.Empty);
        configuration.Setup(x => x.GetSection("maxHistory")).Returns(section.Object);
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>(configuration.Object);
        inMemoryHistory.Append("test");
        inMemoryHistory.Append("test2");
        string? result = inMemoryHistory.Next();
        Assert.Equal("test2", result);
    }

    [Fact]
    public void Previous_WhenAppendedTwice_ReturnsFirstResult()
    {
        var configuration = new Mock<IConfiguration>();
        var section = new Mock<IConfigurationSection>();
        section.SetupGet(x => x.Value).Returns(string.Empty);
        configuration.Setup(x => x.GetSection("maxHistory")).Returns(section.Object);
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>(configuration.Object);
        inMemoryHistory.Append("test");
        inMemoryHistory.Append("test2");
        string? result = inMemoryHistory.Previous();
        Assert.Equal("test", result);
    }

    [Fact]
    public void Append_WhenAppended_MovesCursorToEnd()
    {
        var configuration = new Mock<IConfiguration>();
        var section = new Mock<IConfigurationSection>();
        section.SetupGet(x => x.Value).Returns(string.Empty);
        configuration.Setup(x => x.GetSection("maxHistory")).Returns(section.Object);
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>(configuration.Object);
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

    [Fact]
    public void Append_WhenAppendedThreeTimeWithMaxBufferOfTwo_DropsFirstResult()
    {
        var configuration = new Mock<IConfiguration>();
        var section = new Mock<IConfigurationSection>();
        section.SetupGet(x => x.Value).Returns("2");
        configuration.Setup(x => x.GetSection("maxHistory")).Returns(section.Object);
        IHistoryService<string> inMemoryHistory = new InMemoryHistory<string>(configuration.Object);
        inMemoryHistory.Append("test");
        inMemoryHistory.Append("test2");
        inMemoryHistory.Append("test3");
        inMemoryHistory.Previous();
        inMemoryHistory.Previous();
        Assert.False(inMemoryHistory.HasPrevious);
        string? result = inMemoryHistory.Previous();
        Assert.Equal("test2", result);
    }

}
