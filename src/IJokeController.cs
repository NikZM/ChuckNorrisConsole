namespace GetBusy.ChuckNorrisApi;

public interface IJokeController
{
    public bool HasPreviousJoke { get; }
    public bool HasNextJoke { get; }
    public Task<JokeResponse> GetNewJokeAsync();
    public JokeResponse? GetPreviousJoke();
    public JokeResponse? GetNextJoke();
}
