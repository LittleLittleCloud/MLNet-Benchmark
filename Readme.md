## automl benchmark test using mlnet cli

## To train models for benchmark test
- Install mlnet cli first, make sure your mlnet's version is newer than `16.8.3`. Pick up the right mlnet based on your platform.

`dotnet tool install microsoft.ml.modelbuilder.cli.osx` // MacOS
`dotnet tool install microsoft.ml.modelbuilder.cli.linux64` // Linux
`dotnet tool install microsoft.ml.modelbuilder.cli.win64` // Win

- Then launch an E2E test using the following command:

`dotnet msbuild Run.proj`

The command above will test happy path using these [datasets](./Dataset.props), including training, build generated projects, retraining with exsiting mbconfigs and build those generated projects again.
