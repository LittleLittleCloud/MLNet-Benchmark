using FluentAssertions;
using Microsoft.ML.ModelBuilder.Configuration;
using Microsoft.ML.ModelBuilder.Configuration.Extension;
using Newtonsoft.Json;
using System.Text.Json;

internal static class BinaryClassificationBenchmark
{
    public async static Task RunTitanicAsync()
    {
        Console.WriteLine("Running BinaryClassification Titanic");
        var installingDirectory = Utils.CreateRandomDirectory();
        var success = Utils.RestoreMLNetCLI(installingDirectory);

        Console.WriteLine($"Installing directory: {installingDirectory}");
        success.Should().BeTrue();

        // download titanic dataset from https://automlbenchmark.blob.core.windows.net/dataset/titanic.csv
        var titanicDatasetPath = Path.Combine(installingDirectory, "titanic.csv");
        var titanicDatasetUrl = "https://automlbenchmark.blob.core.windows.net/dataset/titanic.csv";

        Console.WriteLine($"Downloading titanic dataset from {titanicDatasetUrl} to {titanicDatasetPath}");
        success = await Utils.DownloadFileFromUrlAsync(titanicDatasetUrl, titanicDatasetPath);
        success.Should().BeTrue();

        var command = @"classification --dataset titanic.csv --label-col Survived --train-time 20 --name titanic";

        Console.WriteLine($"Running command: {command}");
        success = Utils.RunMLNetCLI(installingDirectory, command);
        success.Should().BeTrue();

        var consoleApp = Path.Combine(installingDirectory, "titanic");
        Directory.Exists(consoleApp).Should().BeTrue();

        var mbConfigFile = Path.Combine(consoleApp, "titanic.mbconfig");
        File.Exists(mbConfigFile).Should().BeTrue();
        var config = Utils.LoadTrainingConfigurationFromFileAsync(mbConfigFile);

        config.Should().NotBeNull();
        config!.IsClassification().Should().BeTrue();
        config!.GetTrainingTime().Should().Be(20);
        config!.GetBestTrial().Should().NotBeNull();
        config!.GetBestTrial()!.Score.Should().BeGreaterThan(0.5);
        config!.GetLabelName().Should().Be("Survived");
        config!.IsMaximizeMetric().Should().BeTrue();

        Console.WriteLine($"build console app: {consoleApp}");
        success = Utils.BuildConsoleApp(consoleApp, "build");
        success.Should().BeTrue();
    }
}
