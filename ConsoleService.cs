using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Configuration;

namespace GetBusy.ChuckNorrisApi;

public class ConsoleService : IHostedService
{
    private readonly IController controller;
    private IConfiguration configuration;
    private CancellationTokenSource? cancellationTokenSource;
    private int intialTopCursor;

    public ConsoleService(IController controller, IConfiguration configuration)
    {
        this.controller = controller;
        this.configuration = configuration;
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        this.cancellationTokenSource = new CancellationTokenSource();
        return Task.Factory.StartNew(async () =>
        {
            if (configuration.GetValue<bool>("displaySplash"))
            {
                DisplaySplash();
            }
            this.intialTopCursor = Console.CursorTop;
            while (!cancellationTokenSource!.IsCancellationRequested)
            {
                try
                {
                    switch (Console.ReadKey().KeyChar)
                    {
                        case 'j':
                            JokeResponse newJoke = await controller.GetNewJokeAsync();
                            UpdateConsoleLine(newJoke.Value);
                            break;
                        case 'n':
                            if (controller.HasNextJoke)
                            {
                                UpdateConsoleLine(controller.GetNextJoke().Value);
                            }
                            break;
                        case 'p':
                            if (controller.HasPreviousJoke)
                            {
                                UpdateConsoleLine(controller.GetPreviousJoke().Value);
                            }
                            break;

                    }
                }
                catch (ChuckNorrisException exception)
                {
                    UpdateConsoleLine(exception.Message);
                }
            }
        });
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        this.cancellationTokenSource?.Cancel();
        return Task.CompletedTask;
    }

    void DisplaySplash()
    {
        Console.Clear();
        string text = System.IO.File.ReadAllText("./splash.txt");
        Console.WriteLine(text);
    }

    void UpdateConsoleLine(string message)
    {
        int currentTop = Console.CursorTop;

        // Multi-line jokes leaves ghosts of long jokes on following lines
        if (currentTop > this.intialTopCursor)
        {
            foreach (int cursorTop in Enumerable.Range(this.intialTopCursor, currentTop))
            {
                Console.SetCursorPosition(0, cursorTop);
                Console.Write(new string(' ', Console.WindowWidth) + "\r");
            }
        }
        Console.SetCursorPosition(0, this.intialTopCursor);
        Console.Write(new string(' ', Console.WindowWidth) + "\r");
        Console.Write(message);
    }
}