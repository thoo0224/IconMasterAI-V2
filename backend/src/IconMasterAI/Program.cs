using Serilog;
using Serilog.Core;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace IconMasterAI;

public class Program
{
    public static async Task Main(string[] args)
    {
        Log.Logger = CreateLogger();

        try
        {
            var app = CreateHostBuilder(args).Build();
            await app.RunAsync();
        }
        catch (Exception e)
        {
            Log.Fatal(e, "Host terminated unexpectedly. {Message}", e.Message);
        }
        finally
        {
            await Log.CloseAndFlushAsync();
        }
    }

    private static IHostBuilder CreateHostBuilder(string[] args)
    {
        return Host.CreateDefaultBuilder(args)
            .ConfigureHostConfiguration(builder =>
            {
                var env = Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT");
                var isDevelopment = env == Environments.Development;
                if (!isDevelopment)
                    builder.AddEnvironmentVariables();
            })
            .ConfigureWebHostDefaults(webHost =>
            {
                webHost.UseStartup<Startup>();
            })
            .UseSerilog();
    }

    private static Logger CreateLogger()
    {
#if RELEASE
        var configuration = new ConfigurationBuilder()
            .AddJsonFile("appsettings.json")
            .Build();
#endif

        return new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("System", LogEventLevel.Warning)
            .MinimumLevel.Override("Microsoft.Hosting.Lifetime", LogEventLevel.Information)
            .Enrich.FromLogContext()
#if RELEASE
            .WriteTo.Seq("https://seq.cfxstore.nl", apiKey: configuration["SeqApiKey"])
#endif
            .WriteTo.Console(
                theme: AnsiConsoleTheme.Code,
                outputTemplate: "[{Timestamp:G}] [{Level:u3}] {Message:l}{NewLine}")
            .WriteTo.File(
                path: "logs/log_.txt",
                rollingInterval: RollingInterval.Day,
                outputTemplate:
                "[{Timestamp:G}] [{Level:u3}] {Message:l}{NewLine:1}{Properties:1j}{NewLine:1}{Exception:1}")
            .CreateLogger();
    }
}
