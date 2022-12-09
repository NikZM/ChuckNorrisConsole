using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace GetBusy.ChuckNorrisApi;

public class ConsoleService : IHostedService
{
    private readonly IController controller;
    private IConfiguration configuration;
    private CancellationTokenSource? cancellationTokenSource;

    public ConsoleService(IController controller, IConfiguration configuration)
    {
        this.controller = controller;
        this.configuration = configuration;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        cancellationTokenSource = new CancellationTokenSource();
        return Task.Factory.StartNew(async () =>
        {
            if (configuration.GetValue<bool>("displaySplash"))
            {
                DisplaySplash();
            }
            int initialTop = Console.CursorTop;
            while (!cancellationTokenSource!.IsCancellationRequested)
            {
                try
                {
                    switch (Console.ReadKey().KeyChar)
                    {
                        case 'j':
                            Console.SetCursorPosition(0, initialTop);
                            Console.Write(new string(' ', Console.WindowWidth) + "\r");
                            var l = await controller.GetNewJokeAsync();
                            Console.Write(l?.Value);
                            break;
                        case 'n':
                            if (controller.HasNextJoke)
                            {
                                Console.SetCursorPosition(0, initialTop);
                                Console.Write(new string(' ', Console.WindowWidth) + "\r");
                                Console.Write(controller.GetNextJoke().Value);
                            }
                            break;
                        case 'p':
                            if (controller.HasPreviousJoke)
                            {
                                Console.SetCursorPosition(0, initialTop);
                                Console.Write(new string(' ', Console.WindowWidth) + "\r");
                                Console.Write(controller.GetPreviousJoke().Value);
                            }
                            break;

                    }
                }
                catch (ChuckNorrisException exception)
                {
                    Console.SetCursorPosition(0, initialTop);
                    Console.Write(new string(' ', Console.WindowWidth) + "\r");
                    Console.Write(exception.Message);
                }
            }
        });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        cancellationTokenSource?.Cancel();
        return Task.CompletedTask;
    }

    void DisplaySplash()
    {
        Console.Clear();
        string text = System.IO.File.ReadAllText("./splash.txt");
        Console.WriteLine(text);
    }
}