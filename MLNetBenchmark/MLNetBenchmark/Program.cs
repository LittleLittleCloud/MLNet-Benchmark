await TextClassificationBenchmark.RunWikipediaAsync();
await BinaryClassificationBenchmark.RunTitanicAsync();
await ObjectDetectionBenchmark.RunCatObjectDetectionVottAsync();
await ForecastingBenchmark.RunBitcoinForecastingAsync();
await ObjectDetectionBenchmark.RunCatObjectDetectionCoCoAsync();
await RegressionBenchmark.RunTaxiFareAsync();
await RecommendationBenchmark.RunMovieRecommendationAsync();
await ImageClassificationBenchmark.RunWeatherDataAsync();
