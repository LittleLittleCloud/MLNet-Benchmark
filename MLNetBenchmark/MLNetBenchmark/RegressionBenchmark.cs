using FluentAssertions;
using Microsoft.ML.ModelBuilder.Configuration.Extension;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class RegressionBenchmark
{
    public async static Task RunTaxiFareAsync()
    {
        Console.WriteLine("Running Regression Taxi Fare");
        var installingDirectory = Utils.CreateRandomDirectory();
        var success = Utils.RestoreMLNetCLI(installingDirectory);

        Console.WriteLine($"Installing directory: {installingDirectory}");
        success.Should().BeTrue();

        // download taxi fare dataset from https://automlbenchmark.blob.core.windows.net/dataset/taxi-fare_train.csv
        var taxiFareTrain = Path.Combine(installingDirectory, "taxi-fare_train.csv");
        var taxiFareTrainDataPath = "https://automlbenchmark.blob.core.windows.net/dataset/taxi-fare_train.csv";
        Console.WriteLine($"Downloading truecar dataset from {taxiFareTrainDataPath} to {taxiFareTrain}");
        success = await Utils.DownloadFileFromUrlAsync(taxiFareTrainDataPath, taxiFareTrain);
        success.Should().BeTrue();

        var command = @"regression --dataset taxi-fare_train.csv --label-col fare_amount --train-time 30 --name tax_fare";
        Console.WriteLine($"Running command: {command}");

        success = Utils.RunMLNetCLI(installingDirectory, command);
        success.Should().BeTrue();

        var consoleApp = Path.Combine(installingDirectory, "tax_fare");
        Directory.Exists(consoleApp).Should().BeTrue();
        var mbConfigFile = Path.Combine(consoleApp, "tax_fare.mbconfig");
        File.Exists(mbConfigFile).Should().BeTrue();
        var config = Utils.LoadTrainingConfigurationFromFileAsync(mbConfigFile);
        config.IsRegression().Should().BeTrue();
        config.GetTrainingTime().Should().Be(30);
        config.GetLabelName().Should().Be("fare_amount");
        config.IsMaximizeMetric().Should().BeTrue();
        config.GetMetricName().Should().Be("RSquared");
        config.GetBestTrial()!.Score.Should().BeGreaterThan(0.5);

        Console.WriteLine($"build console app: {consoleApp}");
        success = Utils.BuildConsoleApp(consoleApp, "build");
        success.Should().BeTrue();
    }
}
