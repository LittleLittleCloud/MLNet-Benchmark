using FluentAssertions;
using Microsoft.ML.ModelBuilder.Configuration;
using Microsoft.ML.ModelBuilder.Configuration.Extension;
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
        var mbConfigFile = Path.Combine(consoleApp, "bitcoin.mbconfig");
        File.Exists(mbConfigFile).Should().BeTrue();
        var config = Utils.LoadTrainingConfigurationFromFileAsync(mbConfigFile);
        config.IsForecasting().Should().BeTrue();
        config.GetTrainingTime().Should().Be(20);
        (config.TrainingOption as IForecastingTrainingOption)?.Horizon.Should().Be(10);
        (config.TrainingOption as IForecastingTrainingOption)?.TimeColumn.Should().Be("Date");
        (config.TrainingOption as IForecastingTrainingOption)?.LabelColumn.Should().Be("Open");
        config.GetBestTrial()!.Score.Should().BeLessThan(200);
        config.TrainResult!.Trials!.Count().Should().BeGreaterThan(0);

        Console.WriteLine($"build console app: {consoleApp}");

        success = Utils.BuildConsoleApp(consoleApp, "build");
        success.Should().BeTrue();
    }
}
