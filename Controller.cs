namespace GetBusy.ChuckNorrisApi;

public class Controller : IController
{
    private readonly IDataProvider<JokeResponse> dataProvider;
    private readonly IHistoryService<JokeResponse> historyProvider;
    public bool HasPreviousJoke => this.historyProvider.HasPrevious;
    public bool HasNextJoke => this.historyProvider.HasNext;
    public Controller(IDataProvider<JokeResponse> dataProvider, IHistoryService<JokeResponse> historyProvider)
    {
        this.dataProvider = dataProvider;
        this.historyProvider = historyProvider;
    }

    public async Task<JokeResponse> GetNewJokeAsync()
    {
        JokeResponse? joke = await this.dataProvider.get();
        if (string.IsNullOrEmpty(joke?.Value))
        {
            throw new ChuckNorrisException("When Chuck Norris throws exceptions, it's across the room.");
        }
        this.historyProvider.Append(joke!);
        return joke!;
    }

    public JokeResponse? GetPreviousJoke()
    {
        return this.historyProvider.Previous();
    }

    public JokeResponse? GetNextJoke()
    {
        return this.historyProvider.Next();
    }
}