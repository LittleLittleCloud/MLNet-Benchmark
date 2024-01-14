using System.Diagnostics;
using System.IO.Compression;
using System.Reflection;
using System.Runtime.InteropServices;

internal static class Utils
{
    public static bool RestoreMLNetCLI(string installingDirectory)
    {
        Console.WriteLine("Restore dotnet cli");
        // write RestoreMLNetCli.config from embedded resource to this.workingDirectory
        var assembly = Assembly.GetAssembly(typeof(Program))!;
        var resourceName = "MLNetBenchmark.RestoreMLNetCli.config";
        using (var stream = assembly.GetManifestResourceStream(resourceName)!)
        using (var fileStream = File.Create(Path.Combine(installingDirectory, "Nuget.Config")))
        {
            stream.CopyTo(fileStream);
        }

        // write dotnet-tool.json from embedded resource to workingDirectory according to os platform
        // available platform: win-x64, linux-x64, osx-x64, linux-arm64, win-arm64, osx-arm64

        var packageID = (OperatingSystem.IsWindows(), OperatingSystem.IsLinux(), OperatingSystem.IsMacOS(), RuntimeInformation.ProcessArchitecture) switch
        {
            (true, false, false, Architecture.X64) => "mlnet-win-x64",
            (false, true, false, Architecture.X64) => "mlnet-linux-x64",
            (false, false, true, Architecture.X64) => "mlnet-osx-x64",
            (false, true, false, Architecture.Arm64) => "mlnet-linux-arm64",
            (true, false, false, Architecture.Arm64) => "mlnet-win-arm64",
            (false, false, true, Architecture.Arm64) => "mlnet-osx-arm64",
            _ => throw new PlatformNotSupportedException("Platform not supported")
        };


        using (var stream2 = assembly.GetManifestResourceStream("MLNetBenchmark.dotnet-tools.json")!)
        using (var fileStream2 = File.Create(Path.Combine(installingDirectory, "dotnet-tools.json")))
        {
            var content = new StreamReader(stream2).ReadToEnd();
            content = content.Replace("PACKAGE_ID", packageID);
            var writer = new StreamWriter(fileStream2);
            writer.Write(content);
            writer.Flush();
        }

        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"tool restore --configfile Nuget.Config",
            WorkingDirectory = installingDirectory,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var process = new Process { StartInfo = psi };
        process.OutputDataReceived += PrintProcessOutput;
        process.ErrorDataReceived += PrintProcessOutput;
        process.Start();
        process.BeginErrorReadLine();
        process.BeginOutputReadLine();
        process.WaitForExit();

        return process.ExitCode == 0;
    }

    private static void PrintProcessOutput(object sender, DataReceivedEventArgs e)
    {
        if (e.Data != null)
        {
            Console.WriteLine(e.Data);
        }
    }

    internal static string CreateRandomDirectory()
    {
        var random = new Random();
        var randomDirectoryName = Path.Combine(Path.GetTempPath(), $"MLNetBenchmark-{random.Next()}");
        Directory.CreateDirectory(randomDirectoryName);
        return randomDirectoryName;
    }

    internal static async Task<bool> DownloadFileFromUrlAsync(string url, string filePath)
    {
        using var client = new HttpClient();

        var response = await client.GetAsync(url);
        if (response.IsSuccessStatusCode)
        {
            await using var fileStream = new FileStream(filePath, FileMode.Create);
            await response.Content.CopyToAsync(fileStream);
            return true;
        }
        else
        {
            return false;
        }
    }

    internal static bool RunMLNetCLI(string installingDirectory, string command)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"tool run mlnet {command}",
            WorkingDirectory = installingDirectory,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var process = new Process { StartInfo = psi };
        process.OutputDataReceived += PrintProcessOutput;
        process.ErrorDataReceived += PrintProcessOutput;
        process.Start();
        process.BeginErrorReadLine();
        process.BeginOutputReadLine();
        process.WaitForExit();

        return process.ExitCode == 0;
    }

    internal static bool BuildConsoleApp(string installingDirectory, string command)
    {
        var psi = new ProcessStartInfo
        {
            FileName = "dotnet",
            Arguments = $"{command}",
            WorkingDirectory = installingDirectory,
            RedirectStandardInput = true,
            RedirectStandardOutput = true,
            RedirectStandardError = true,
            UseShellExecute = false,
            CreateNoWindow = true,
        };

        using var process = new Process { StartInfo = psi };
        process.OutputDataReceived += PrintProcessOutput;
        process.ErrorDataReceived += PrintProcessOutput;
        process.Start();
        process.BeginErrorReadLine();
        process.BeginOutputReadLine();
        process.WaitForExit();

        return process.ExitCode == 0;
    }

    internal static bool UnzipFile(string zipPath, string unzipPath)
    {
        ZipFile.ExtractToDirectory(zipPath, unzipPath);
        return true;
    }
}
