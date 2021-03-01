## automl benchmark test using mlnet cli

## To train models for benchmark test
- Install mlnet cli first, make sure your mlnet's version is no earlier than `16.4.4`.

`dotnet tool install mlnet -g --add-source https://devdiv.pkgs.visualstudio.com/_packaging/ModelBuilder/nuget/v3/index.json`

- Then launch a benchark test using the following command:

`dotnet msbuild Train.proj -t:Train`

## To generate report
`dotnet msbuild Train.proj -t:GenerateReportFile`
