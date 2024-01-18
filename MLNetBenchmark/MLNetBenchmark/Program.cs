using System.Runtime.InteropServices;
if (RuntimeInformation.ProcessArchitecture != Architecture.Arm64)
{
    await TextClassificationBenchmark.RunWikipediaAsync();
    await ImageClassificationBenchmark.RunWeatherDataAsync();
    await ObjectDetectionBenchmark.RunCatObjectDetectionVottAsync();
    await ObjectDetectionBenchmark.RunCatObjectDetectionCoCoAsync();
    await ForecastingBenchmark.RunBitcoinForecastingAsync();
}

await BinaryClassificationBenchmark.RunTitanicAsync();
await RecommendationBenchmark.RunMovieRecommendationAsync();
await RegressionBenchmark.RunTaxiFareAsync();


