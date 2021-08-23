## automl benchmark test using mlnet cli

## To train models for benchmark test
- Install mlnet cli first, make sure your mlnet's version is no earlier than `16.4.4`.

`dotnet tool install mlnet -g --add-source https://mlnetcli.blob.core.windows.net/mlnetcli/index.json`

- Then launch an E2E test using the following command:

`dotnet msbuild Run.proj`

The command above will test happy path using these [datasets](Dataset.props), including training and build generated projects. And it will generate a final report in the same folder when the test is finished.
