using System.Runtime.InteropServices;
if (RuntimeInformation.ProcessArchitecture != Architecture.Arm64 || Environment.GetEnvironmentVariable("IS_ARM64") != "1")
{
    await ImageClassificationBenchmark.RunWeatherDataAsync();
    await TextClassificationBenchmark.RunWikipediaAsync();
    await ObjectDetectionBenchmark.RunCatObjectDetectionVottAsync();
    await ObjectDetectionBenchmark.RunCatObjectDetectionCoCoAsync();
    await ForecastingBenchmark.RunBitcoinForecastingAsync();
}

await BinaryClassificationBenchmark.RunTitanicAsync();
await RecommendationBenchmark.RunMovieRecommendationAsync();
await RegressionBenchmark.RunTaxiFareAsync();


