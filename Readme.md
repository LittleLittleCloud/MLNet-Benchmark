## automl benchmark test using mlnet cli

## Start a benchmark test

You can start a benchmark test on a specific mlnet cli by creating a pull request with the following steps:
- Go to [dotnet-tools.json](./MLNetBenchmark/MLNetBenchmark/dotnet-tools.json)
- Update the mlnet cli version to the one you want to test

```json
{
  "version": 1,
  "isRoot": true,
  "tools": {
    "PACKAGE_ID": {
      "version": "16.15.1", // update this version
      "commands": [
        "mlnet"
      ]
    }
  }
}
```
- Create a pull request with the updated version

