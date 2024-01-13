using FluentAssertions;

internal static class RecommendationBenchmark
{
    public async static Task RunMovieRecommendationAsync()
    {
        Console.WriteLine("Running Recommendation MovieRecommendation");
        var installingDirectory = Utils.CreateRandomDirectory();
        var success = Utils.RestoreMLNetCLI(installingDirectory);

        Console.WriteLine($"Installing directory: {installingDirectory}");
        success.Should().BeTrue();

        // download movie recommendation dataset from https://automlbenchmark.blob.core.windows.net/dataset/recommendation-ratings-train.csv
        var movieRecommendationDatasetPath = Path.Combine(installingDirectory, "recommendation-ratings-train.csv");
        var movieRecommendationDatasetUrl = "https://automlbenchmark.blob.core.windows.net/dataset/recommendation-ratings-train.csv";
        Console.WriteLine($"Downloading movie recommendation dataset from {movieRecommendationDatasetUrl} to {movieRecommendationDatasetPath}");
        success = await Utils.DownloadFileFromUrlAsync(movieRecommendationDatasetUrl, movieRecommendationDatasetPath);
        success.Should().BeTrue();

        var command = @"recommendation --dataset recommendation-ratings-train.csv --split-ratio 0.7 --rating-col rating --user-col userId --item-col movieId --train-time 100 --name movie";
        Console.WriteLine($"Running command: {command}");

        success = Utils.RunMLNetCLI(installingDirectory, command);
        success.Should().BeTrue();

        var consoleApp = Path.Combine(installingDirectory, "movie");
        Directory.Exists(consoleApp).Should().BeTrue();
        Console.WriteLine($"build console app: {consoleApp}");
        success = Utils.BuildConsoleApp(consoleApp, "build");
        success.Should().BeTrue();
    }
}
