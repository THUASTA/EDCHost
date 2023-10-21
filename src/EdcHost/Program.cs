using System.Text.RegularExpressions;
using dotenv.net;
using dotenv.net.Utilities;
using Serilog;

namespace EdcHost;

class Program
{
    const int DefaultServerPort = 8080;
    const string SerilogTemplate = "[{Timestamp:HH:mm:ss} {Level:u3}] <{Component}> {Message:lj}{NewLine}{Exception}";

    static void Main()
    {
        // Setup logger using default settings before calling dotenv.
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            SetupDotEnv();
            SetupSerilog();
            SetupAndRunEdcHost();

        }
        catch (Exception exception)
        {
            Log.Fatal(exception, "encountered an unhandled exception");
        }
    }

    static List<Tuple<int, int>> ParseMineList(string input)
    {
        List<Tuple<int, int>> mines = new();
        Regex regex = new(@"\((\d+),(\d+)\)");
        MatchCollection matches = regex.Matches(input);
        foreach (Match match in matches.Cast<Match>())
        {
            int x = int.Parse(match.Groups[1].Value);
            int y = int.Parse(match.Groups[2].Value);
            mines.Add(new Tuple<int, int>(x, y));
        }
        return mines;
    }

    static void SetupAndRunEdcHost()
    {
        List<Tuple<int, int>> gameDiamondMines = EnvReader.TryGetStringValue("GAME_DIAMOND_MINES", out string? gameDiamondMinesString) ? ParseMineList(gameDiamondMinesString) : new();
        List<Tuple<int, int>> gameGoldMines = EnvReader.TryGetStringValue("GAME_GOLD_MINES", out string? gameGoldMinesString) ? ParseMineList(gameGoldMinesString) : new();
        List<Tuple<int, int>> gameIronMines = EnvReader.TryGetStringValue("GAME_IRON_MINES", out string? gameIronMinesString) ? ParseMineList(gameIronMinesString) : new();
        int serverPort = EnvReader.TryGetIntValue("SERVER_PORT", out serverPort) ? serverPort : DefaultServerPort;

        IEdcHost edcHost = IEdcHost.Create(new IEdcHost.EdcHostOptions
        (
            gameDiamondMines: gameDiamondMines,
            gameGoldMines: gameGoldMines,
            gameIronMines: gameIronMines,
            serverPort: serverPort
        ));

        edcHost.Start();
    }

    static void SetupDotEnv()
    {
        DotEnv.Load(new DotEnvOptions
        (
            trimValues: true
        ));
    }

    static void SetupSerilog()
    {
        // Get logging level from environment variables
        if (EnvReader.TryGetStringValue("LOGGING_LEVEL", out string? loggingLevelString) == false)
        {
            Log.Warning("LOGGING_LEVEL not set, using default value: Information");
            loggingLevelString = "Information";
        }

        // Configure Serilog
        Log.Logger = loggingLevelString switch
        {
            "Verbose" => new LoggerConfiguration()
                .MinimumLevel.Verbose()
                .WriteTo.Console(outputTemplate: SerilogTemplate)
                .Enrich.WithProperty("Component", "Program")
                .CreateLogger(),
            "Debug" => new LoggerConfiguration()
                .MinimumLevel.Debug()
                .WriteTo.Console(outputTemplate: SerilogTemplate)
                .Enrich.WithProperty("Component", "Program")
                .CreateLogger(),
            "Information" => new LoggerConfiguration()
                .MinimumLevel.Information()
                .WriteTo.Console(outputTemplate: SerilogTemplate)
                .Enrich.WithProperty("Component", "Program")
                .CreateLogger(),
            "Warning" => new LoggerConfiguration()
                .MinimumLevel.Warning()
                .WriteTo.Console(outputTemplate: SerilogTemplate)
                .Enrich.WithProperty("Component", "Program")
                .CreateLogger(),
            "Error" => new LoggerConfiguration()
                .MinimumLevel.Error()
                .WriteTo.Console(outputTemplate: SerilogTemplate)
                .Enrich.WithProperty("Component", "Program")
                .CreateLogger(),
            "Fatal" => new LoggerConfiguration()
                .MinimumLevel.Fatal()
                .WriteTo.Console(outputTemplate: SerilogTemplate)
                .Enrich.WithProperty("Component", "Program")
                .CreateLogger(),
            _ => throw new ArgumentOutOfRangeException(nameof(loggingLevelString), loggingLevelString, "invalid logging level")
        };

        // Configure Fleck logging
        ILogger fleckLogger = Log.Logger.ForContext("Component", "Fleck");
        Fleck.FleckLog.LogAction = (level, message, ex) =>
        {
            switch (level)
            {
                case Fleck.LogLevel.Debug:
                    fleckLogger.Debug(message, ex);
                    break;
                case Fleck.LogLevel.Info:
                    fleckLogger.Information(message, ex);
                    break;
                case Fleck.LogLevel.Warn:
                    fleckLogger.Warning(message, ex);
                    break;
                case Fleck.LogLevel.Error:
                    fleckLogger.Error(message, ex);
                    break;
            }
        };
    }
}
