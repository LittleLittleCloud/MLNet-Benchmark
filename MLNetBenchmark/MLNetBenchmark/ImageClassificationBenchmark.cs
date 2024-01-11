using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class ImageClassificationBenchmark
{
    public static async Task RunWeatherDataAsync()
    {
        Console.WriteLine("Running ImageClassification WeatherData");
        var installingDirectory = Utils.CreateRandomDirectory();
        var success = Utils.RestoreMLNetCLI(installingDirectory);
        Console.WriteLine($"Installing directory: {installingDirectory}");
        success.Should().BeTrue();

        // download weather dataset from https://automlbenchmark.blob.core.windows.net/dataset/WeatherData.zip
        var weatherDatasetPath = Path.Combine(installingDirectory, "WeatherData.zip");
        var weatherDatasetUrl = "https://automlbenchmark.blob.core.windows.net/dataset/WeatherData.zip";
        Console.WriteLine($"Downloading weather dataset from {weatherDatasetUrl} to {weatherDatasetPath}");
        success = await Utils.DownloadFileFromUrlAsync(weatherDatasetUrl, weatherDatasetPath);

        var weatherDatasetUnzipPath = Path.Combine(installingDirectory, "WeatherData");
        Console.WriteLine($"Unzipping weather dataset from {weatherDatasetPath} to {weatherDatasetUnzipPath}");
        success = Utils.UnzipFile(weatherDatasetPath, weatherDatasetUnzipPath);

        var command = @"image-classification --dataset WeatherData --name weather";
        Console.WriteLine($"Running command: {command}");

        success = Utils.RunMLNetCLI(installingDirectory, command);
        success.Should().BeTrue();

        var consoleApp = Path.Combine(installingDirectory, "weather");
        Directory.Exists(consoleApp).Should().BeTrue();
        Console.WriteLine($"build console app: {consoleApp}");
        success = Utils.BuildConsoleApp(consoleApp, "build");
        success.Should().BeTrue();
    }
}
