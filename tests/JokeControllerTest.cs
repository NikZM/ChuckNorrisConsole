namespace GetBusy.ChuckNorrisApi.Tests;

public class JokeControllerTest
{
    [Fact]
    public async void GetNewJokeAsync_AppendsJokeToHistory_WhenCalledWithResult()
    {
        var dateProvider = new Mock<IDataProvider<JokeResponse>>();
        var historyProvider = new Mock<IHistoryService<JokeResponse>>();
        dateProvider.Setup(x => x.Get()).ReturnsAsync(new JokeResponse() { Value = "Chuck Norris can unit test entire applications with a single assert." });
        historyProvider.Setup(x => x.Previous()).Returns(default(JokeResponse));
        historyProvider.Setup(x => x.Next()).Returns(default(JokeResponse));
        IJokeController controller = new ChuckNorrisJokeController(dateProvider.Object, historyProvider.Object);

        await controller.GetNewJokeAsync();
        historyProvider.Verify(x => x.Append(It.IsAny<JokeResponse>()));
    }

    [Fact]
    public async void GetNewJokeAsync_DoesNotAppendsJokeToHistory_WhenCalledWithNoResult()
    {
        var dateProvider = new Mock<IDataProvider<JokeResponse>>();
        var historyProvider = new Mock<IHistoryService<JokeResponse>>();
        dateProvider.Setup(x => x.Get()).ReturnsAsync(new JokeResponse() { });
        historyProvider.Setup(x => x.Previous()).Returns(default(JokeResponse));
        historyProvider.Setup(x => x.Next()).Returns(default(JokeResponse));
        IJokeController controller = new ChuckNorrisJokeController(dateProvider.Object, historyProvider.Object);

        await Assert.ThrowsAsync<ChuckNorrisException>(() => controller.GetNewJokeAsync());
        historyProvider.Verify(x => x.Append(It.IsAny<JokeResponse>()), Times.Never());
    }

    [Fact]
    public void GetPreviousJoke_ReturnsHistoryPrevious_WhenCalled()
    {
        var dateProvider = new Mock<IDataProvider<JokeResponse>>();
        var historyProvider = new Mock<IHistoryService<JokeResponse>>();
        historyProvider.Setup(x => x.Previous()).Returns(default(JokeResponse));
        historyProvider.Setup(x => x.Next()).Returns(default(JokeResponse));
        IJokeController controller = new ChuckNorrisJokeController(dateProvider.Object, historyProvider.Object);

        Assert.Equal(default(JokeResponse), controller.GetPreviousJoke());
        historyProvider.Verify(x => x.Previous());
    }

    [Fact]
    public void GetNextJoke_ReturnsHistoryNext_WhenCalled()
    {
        var dateProvider = new Mock<IDataProvider<JokeResponse>>();
        var historyProvider = new Mock<IHistoryService<JokeResponse>>();
        historyProvider.Setup(x => x.Previous()).Returns(default(JokeResponse));
        historyProvider.Setup(x => x.Next()).Returns(default(JokeResponse));
        IJokeController controller = new ChuckNorrisJokeController(dateProvider.Object, historyProvider.Object);

        Assert.Equal(default(JokeResponse), controller.GetNextJoke());
        historyProvider.Verify(x => x.Next());
    }

}
