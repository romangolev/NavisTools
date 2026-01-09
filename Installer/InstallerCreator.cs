using System.Diagnostics;
using System.Reflection;

namespace Installer
{
    internal class InstallerCreator
    {
        static int Main(string[] args)
        {
            Console.WriteLine("NavisTools Installer Creator");
            Console.WriteLine("============================");
            Console.WriteLine();

            try
            {
                // Get the solution root directory
                var solutionDir = GetSolutionDirectory();
                if (solutionDir == null)
                {
                    Console.Error.WriteLine("Error: Could not find solution directory");
                    return 1;
                }

                // Get version from assembly (set by Directory.Build.props)
                var version = Assembly.GetExecutingAssembly().GetName().Version;
                var versionString = $"{version.Major}.{version.Minor}.{version.Build}";
                
                Console.WriteLine($"Version: {versionString}");

                // Paths
                var issScriptPath = Path.Combine(solutionDir, "Installer", "NavisTools.iss");
                var bundlePath = Path.Combine(solutionDir, "output", "NavisTools.bundle");
                var outputDir = Path.Combine(solutionDir, "output", "installer");

                // Verify bundle exists
                if (!Directory.Exists(bundlePath))
                {
                    Console.Error.WriteLine($"Error: Bundle not found at: {bundlePath}");
                    Console.Error.WriteLine("Please run 'nuke CreateBundle' first to build the bundle.");
                    return 1;
                }

                // Verify Inno Setup script exists
                if (!File.Exists(issScriptPath))
                {
                    Console.Error.WriteLine($"Error: Inno Setup script not found at: {issScriptPath}");
                    return 1;
                }

                // Find Inno Setup compiler
                var isccPath = FindInnoSetupCompiler();
                if (isccPath == null)
                {
                    Console.Error.WriteLine("Error: Inno Setup compiler (ISCC.exe) not found.");
                    Console.Error.WriteLine("Please install Inno Setup from: https://jrsoftware.org/isinfo.php");
                    Console.Error.WriteLine("Or ensure it's in your PATH or installed in the default location.");
                    return 1;
                }

                Console.WriteLine($"Found Inno Setup compiler: {isccPath}");
                Console.WriteLine($"Bundle location: {bundlePath}");
                Console.WriteLine($"Output directory: {outputDir}");
                Console.WriteLine();

                // Ensure output directory exists
                Directory.CreateDirectory(outputDir);

                // Compile the installer with version parameter
                Console.WriteLine("Compiling installer...");
                var processInfo = new ProcessStartInfo
                {
                    FileName = isccPath,
                    Arguments = $"\"{issScriptPath}\" /DMyAppVersion=\"{versionString}\"",
                    UseShellExecute = false,
                    RedirectStandardOutput = true,
                    RedirectStandardError = true,
                    CreateNoWindow = true
                };

                using var process = Process.Start(processInfo);
                if (process == null)
                {
                    Console.Error.WriteLine("Error: Failed to start Inno Setup compiler");
                    return 1;
                }

                // Read output
                var output = process.StandardOutput.ReadToEnd();
                var error = process.StandardError.ReadToEnd();

                process.WaitForExit();

                if (!string.IsNullOrEmpty(output))
                {
                    Console.WriteLine(output);
                }

                if (!string.IsNullOrEmpty(error))
                {
                    Console.Error.WriteLine(error);
                }

                if (process.ExitCode == 0)
                {
                    Console.WriteLine();
                    Console.WriteLine("✓ Installer created successfully!");
                    Console.WriteLine($"  Location: {outputDir}");

                    // Find the generated installer
                    var installerFiles = Directory.GetFiles(outputDir, "NavisTools-Setup-*.exe");
                    if (installerFiles.Length > 0)
                    {
                        Console.WriteLine($"  Installer: {Path.GetFileName(installerFiles[0])}");
                    }

                    return 0;
                }
                else
                {
                    Console.Error.WriteLine($"Error: Inno Setup compiler exited with code {process.ExitCode}");
                    return 1;
                }
            }
            catch (Exception ex)
            {
                Console.Error.WriteLine($"Error: {ex.Message}");
                Console.Error.WriteLine(ex.StackTrace);
                return 1;
            }
        }

        static string? GetSolutionDirectory()
        {
            var currentDir = Directory.GetCurrentDirectory();

            // Walk up the directory tree to find the solution directory
            while (!string.IsNullOrEmpty(currentDir))
            {
                if (Directory.GetFiles(currentDir, "*.sln").Length > 0)
                {
                    return currentDir;
                }

                var parent = Directory.GetParent(currentDir);
                if (parent == null)
                    break;

                currentDir = parent.FullName;
            }

            return null;
        }

        static string? FindInnoSetupCompiler()
        {
            // Check common installation paths
            var possiblePaths = new[]
            {
                @"C:\Program Files (x86)\Inno Setup 6\ISCC.exe",
                @"C:\Program Files\Inno Setup 6\ISCC.exe",
                @"C:\Program Files (x86)\Inno Setup 5\ISCC.exe",
                @"C:\Program Files\Inno Setup 5\ISCC.exe"
            };

            foreach (var path in possiblePaths)
            {
                if (File.Exists(path))
                    return path;
            }

            // Check if it's in PATH
            var pathEnv = Environment.GetEnvironmentVariable("PATH");
            if (pathEnv != null)
            {
                var paths = pathEnv.Split(Path.PathSeparator);
                foreach (var path in paths)
                {
                    var isccPath = Path.Combine(path, "ISCC.exe");
                    if (File.Exists(isccPath))
                        return isccPath;
                }
            }

            return null;
        }
    }
}
