using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class ForecastingBenchmark
{
    public async static Task RunBitcoinForecastingAsync()
    {
        Console.WriteLine("Running Forecasting Bitcoin");
        var installingDirectory = Utils.CreateRandomDirectory();
        var success = Utils.RestoreMLNetCLI(installingDirectory);
        Console.WriteLine("Installing directory: " + installingDirectory);
        success.Should().BeTrue();

        // download bitcoin dataset from https://automlbenchmark.blob.core.windows.net/dataset/Binance_BTCUSDT_1h.csv
        var bitcoinDatasetPath = Path.Combine(installingDirectory, "bitcoin.csv");
        var bitcoinDatasetUrl = "https://automlbenchmark.blob.core.windows.net/dataset/Binance_BTCUSDT_1h.csv";
        Console.WriteLine($"Downloading bitcoin dataset from {bitcoinDatasetUrl} to {bitcoinDatasetPath}");
        success = await Utils.DownloadFileFromUrlAsync(bitcoinDatasetUrl, bitcoinDatasetPath);
        success.Should().BeTrue();

        var command = @"forecasting --dataset bitcoin.csv --label-col Open --time-col Date --horizon 10 --train-time 20 --name bitcoin";
        Console.WriteLine($"Running command: {command}");

        success = Utils.RunMLNetCLI(installingDirectory, command);
        success.Should().BeTrue();

        var consoleApp = Path.Combine(installingDirectory, "bitcoin");
        Directory.Exists(consoleApp).Should().BeTrue();
        Console.WriteLine($"build console app: {consoleApp}");

        success = Utils.BuildConsoleApp(consoleApp, "build");
        success.Should().BeTrue();
    }
}
