using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using GetBusy.ChuckNorrisApi;

using IHost host = Host.CreateDefaultBuilder(args)
  .ConfigureServices((_, services) =>
      services.AddHttpClient()
        .AddSingleton<IHistoryService<JokeResponse>, InMemoryHistory<JokeResponse>>()
        .AddSingleton<IDataProvider<JokeResponse>, RandomChuckNorrisJoke>()
        .AddSingleton<IController, Controller>()
        .AddHostedService<ConsoleService>()
  ).Build();

  host.Run();
