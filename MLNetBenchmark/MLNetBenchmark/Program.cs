using System.Runtime.InteropServices;

await BinaryClassificationBenchmark.RunTitanicAsync();
await RecommendationBenchmark.RunMovieRecommendationAsync();
await RegressionBenchmark.RunTaxiFareAsync();

if (RuntimeInformation.ProcessArchitecture != Architecture.Arm64)
{
    await TextClassificationBenchmark.RunWikipediaAsync();
    await ObjectDetectionBenchmark.RunCatObjectDetectionVottAsync();
    await ObjectDetectionBenchmark.RunCatObjectDetectionCoCoAsync();
    await ImageClassificationBenchmark.RunWeatherDataAsync();
    await ForecastingBenchmark.RunBitcoinForecastingAsync();
}
