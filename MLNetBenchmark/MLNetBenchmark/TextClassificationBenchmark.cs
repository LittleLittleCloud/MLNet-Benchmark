using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class TextClassificationBenchmark
{
    public async static Task RunWikipediaAsync()
    {
        Console.WriteLine("Running TextClassification SentimentAnalysis");
        var installingDirectory = Utils.CreateRandomDirectory();
        var success = Utils.RestoreMLNetCLI(installingDirectory);

        Console.WriteLine($"Installing directory: {installingDirectory}");
        success.Should().BeTrue();

        // download sentiment analysis dataset from https://automlbenchmark.blob.core.windows.net/dataset/wikipedia-detox-250-line-data.tsv
        var sentimentAnalysisDatasetPath = Path.Combine(installingDirectory, "wikipedia-detox-250-line-data.tsv");
        var sentimentAnalysisDatasetUrl = "https://automlbenchmark.blob.core.windows.net/dataset/wikipedia-detox-250-line-data.tsv";
        Console.WriteLine($"Downloading sentiment analysis dataset from {sentimentAnalysisDatasetUrl} to {sentimentAnalysisDatasetPath}");
        success = await Utils.DownloadFileFromUrlAsync(sentimentAnalysisDatasetUrl, sentimentAnalysisDatasetPath);
        success.Should().BeTrue();

        var command = @"text-classification --dataset wikipedia-detox-250-line-data.tsv --label-col Sentiment --text-col SentimentText --device cpu --max-epoch 1 --split-ratio 0.7 --name sentiment";
        Console.WriteLine($"Running command: {command}");

        success = Utils.RunMLNetCLI(installingDirectory, command);
        success.Should().BeTrue();

        var consoleApp = Path.Combine(installingDirectory, "sentiment");
        Directory.Exists(consoleApp).Should().BeTrue();
        Console.WriteLine($"build console app: {consoleApp}");
        success = Utils.BuildConsoleApp(consoleApp, "build");
        success.Should().BeTrue();
    }
}
