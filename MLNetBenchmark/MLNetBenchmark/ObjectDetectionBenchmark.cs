using FluentAssertions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

internal static class ObjectDetectionBenchmark
{
    public static async Task RunCatObjectDetectionCoCoAsync()
    {
        Console.WriteLine("Running ObjectDetection CatObjectDetection");
        var installingDirectory = Utils.CreateRandomDirectory();
        var success = Utils.RestoreMLNetCLI(installingDirectory);
        Console.WriteLine($"Installing directory: {installingDirectory}");
        success.Should().BeTrue();

        // download cat object detection dataset from https://automlbenchmark.blob.core.windows.net/dataset/od-cats.zip
        var catDatasetPath = Path.Combine(installingDirectory, "od-cats.zip");
        var catDatasetUrl = "https://automlbenchmark.blob.core.windows.net/dataset/od-cats.zip";
        Console.WriteLine($"Downloading cat object detection dataset from {catDatasetUrl} to {catDatasetPath}");
        success = await Utils.DownloadFileFromUrlAsync(catDatasetUrl, catDatasetPath);
        success.Should().BeTrue();

        var catDatasetUnzipPath = Path.Combine(installingDirectory, "Cat");
        Console.WriteLine($"Unzipping cat object detection dataset from {catDatasetPath} to {catDatasetUnzipPath}");
        success = Utils.UnzipFile(catDatasetPath, catDatasetUnzipPath);
        success.Should().BeTrue();

        var command = @"object-detection --dataset Cat/coco_cat.json --name cat --split-ratio 0.3 --device cpu --width 800 --height 600 --epoch 1";
        Console.WriteLine($"Running command: {command}");

        success = Utils.RunMLNetCLI(installingDirectory, command);
        success.Should().BeTrue();

        var consoleApp = Path.Combine(installingDirectory, "cat");
        Directory.Exists(consoleApp).Should().BeTrue();
        Console.WriteLine($"build console app: {consoleApp}");
        success = Utils.BuildConsoleApp(consoleApp, "build");
        success.Should().BeTrue();
    }

    public static async Task RunCatObjectDetectionVottAsync()
    {
        Console.WriteLine("Running ObjectDetection CatObjectDetection");
        var installingDirectory = Utils.CreateRandomDirectory();
        var success = Utils.RestoreMLNetCLI(installingDirectory);
        Console.WriteLine($"Installing directory: {installingDirectory}");
        success.Should().BeTrue();

        // download cat object detection dataset from https://automlbenchmark.blob.core.windows.net/dataset/od-cats.zip
        var catDatasetPath = Path.Combine(installingDirectory, "od-cats.zip");
        var catDatasetUrl = "https://automlbenchmark.blob.core.windows.net/dataset/od-cats.zip";
        Console.WriteLine($"Downloading cat object detection dataset from {catDatasetUrl} to {catDatasetPath}");
        success = await Utils.DownloadFileFromUrlAsync(catDatasetUrl, catDatasetPath);
        success.Should().BeTrue();

        var catDatasetUnzipPath = Path.Combine(installingDirectory, "Cat");
        Console.WriteLine($"Unzipping cat object detection dataset from {catDatasetPath} to {catDatasetUnzipPath}");
        success = Utils.UnzipFile(catDatasetPath, catDatasetUnzipPath);
        success.Should().BeTrue();

        var command = @"object-detection --dataset Cat/cats-export.json --name cat --split-ratio 0.3 --device cpu --width 800 --height 600 --epoch 1";
        Console.WriteLine($"Running command: {command}");

        success = Utils.RunMLNetCLI(installingDirectory, command);
        success.Should().BeTrue();

        var consoleApp = Path.Combine(installingDirectory, "cat");
        Directory.Exists(consoleApp).Should().BeTrue();
        Console.WriteLine($"build console app: {consoleApp}");
        success = Utils.BuildConsoleApp(consoleApp, "build");
        success.Should().BeTrue();
    }
}
