using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace GetBusy.ChuckNorrisApi;

public class RandomChuckNorrisJoke : IDataProvider<JokeResponse>
{
    private readonly IHttpClientFactory httpClientFactory;
    private readonly IConfiguration configuration;
    private readonly JsonSerializerOptions serializerOptions;

    public RandomChuckNorrisJoke(IHttpClientFactory httpClientFactory, IConfiguration configuration)
    {
        this.httpClientFactory = httpClientFactory;
        this.configuration = configuration;
        this.serializerOptions = new JsonSerializerOptions() { PropertyNamingPolicy = JsonNamingPolicy.CamelCase };
    }

    public async Task<JokeResponse?> Get()
    {
        try
        {
            using HttpClient client = httpClientFactory.CreateClient();
            await using Stream stream = await client.GetStreamAsync(configuration["url"]);
            return await JsonSerializer.DeserializeAsync<JokeResponse>(stream, serializerOptions);
        }
        catch (Exception err)
        {
            throw new ChuckNorrisException($"When Chuck Norris throws exceptions, it's across the room.", err);
        }
    }
}