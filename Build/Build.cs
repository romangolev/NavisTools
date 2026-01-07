using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Nuke.Common;
using Nuke.Common.CI;
using Nuke.Common.Execution;
using Nuke.Common.IO;
using Nuke.Common.ProjectModel;
using Nuke.Common.Tools.MSBuild;
using Nuke.Common.Tooling;
using Nuke.Common.Utilities.Collections;
using static Nuke.Common.EnvironmentInfo;
using static Nuke.Common.IO.PathConstruction;
using static Nuke.Common.Tools.MSBuild.MSBuildTasks;

class Build : NukeBuild
{
    /// Support plugins are available for:
    ///   - JetBrains ReSharper        https://nuke.build/resharper
    ///   - JetBrains Rider            https://nuke.build/rider
    ///   - Microsoft VisualStudio     https://nuke.build/visualstudio
    ///   - Microsoft VSCode           https://nuke.build/vscode

    public static int Main () => Execute<Build>(x => x.CreateBundle);

    [Parameter("Configuration to build - Default is 'Debug' (local) or 'Release' (server)")]
    readonly Configuration Configuration = IsLocalBuild ? Configuration.Debug : Configuration.Release;

    [Solution] readonly Solution Solution;

    AbsolutePath SourceDirectory => RootDirectory / "NavisTools";
    AbsolutePath OutputDirectory => RootDirectory / "output";
    AbsolutePath BundleDirectory => OutputDirectory / "NavisTools.bundle";

    // Navisworks versions to build
    string[] NavisworksVersions => new[] { "2022", "2023", "2024", "2025", "2026" };

    Target Clean => _ => _
        .Before(Restore)
        .Executes(() =>
        {
            OutputDirectory.CreateOrCleanDirectory();
        });

    Target Restore => _ => _
        .Executes(() =>
        {
            MSBuild(s => s
                .SetTargetPath(Solution)
                .SetTargets("Restore"));
        });

    Target Compile => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            MSBuild(s => s
                .SetTargetPath(Solution)
                .SetTargets("Build")
                .SetConfiguration(Configuration)
                .SetMaxCpuCount(Environment.ProcessorCount)
                .SetNodeReuse(IsLocalBuild));
        });

    Target BuildAllReleaseConfigurations => _ => _
        .DependsOn(Restore)
        .Executes(() =>
        {
            var navisToolsProject = Solution.GetProject("NavisTools");

            foreach (var version in NavisworksVersions)
            {
                var configuration = $"Release N{version.Substring(2)}"; // "2022" -> "Release N22"

                Serilog.Log.Information($"Building configuration: {configuration}");

                MSBuild(s => s
                    .SetTargetPath(navisToolsProject)
                    .SetTargets("Build")
                    .SetConfiguration(configuration)
                    .SetMaxCpuCount(Environment.ProcessorCount)
                    .SetProperty("SkipPostBuildEvent", "true") // Skip custom post-build targets
                    .SetNodeReuse(IsLocalBuild));
            }
        });

    Target CreateBundle => _ => _
        .DependsOn(BuildAllReleaseConfigurations)
        .Executes(() =>
        {
            var navisToolsProject = Solution.GetProject("NavisTools");

            // Clean bundle directory
            BundleDirectory.CreateOrCleanDirectory();

            Serilog.Log.Information($"Creating bundle at: {BundleDirectory}");

            foreach (var version in NavisworksVersions)
            {
                var configuration = $"Release N{version.Substring(2)}";
                var binPath = navisToolsProject.Directory / "bin" / configuration / "net48";
                var contentPath = BundleDirectory / "Contents" / version;

                Serilog.Log.Information($"Processing {version}...");

                // Create directory structure
                contentPath.CreateDirectory();

                // Copy DLLs
                var dllPath = binPath / "NavisTools.dll";
                if (File.Exists(dllPath))
                {
                    File.Copy(dllPath, contentPath / "NavisTools.dll", true);
                    Serilog.Log.Information($"  Copied: NavisTools.dll");
                }

                // Copy XAML files to en-US subdirectory
                var enUsPath = contentPath / "en-US";
                enUsPath.CreateDirectory();

                var xamlFiles = Directory.GetFiles(navisToolsProject.Directory, "*.xaml");
                foreach (var xamlFile in xamlFiles)
                {
                    File.Copy(xamlFile, enUsPath / Path.GetFileName(xamlFile), true);
                    Serilog.Log.Information($"  Copied: {Path.GetFileName(xamlFile)}");
                }

                // Copy .name files to en-US subdirectory
                var nameFiles = Directory.GetFiles(navisToolsProject.Directory, "*.name");
                foreach (var nameFile in nameFiles)
                {
                    File.Copy(nameFile, enUsPath / Path.GetFileName(nameFile), true);
                    Serilog.Log.Information($"  Copied: {Path.GetFileName(nameFile)}");
                }

                // Copy icon files to Images subdirectory
                var imagesPath = contentPath / "Images";
                var projectImagesPath = navisToolsProject.Directory / "Images";

                if (Directory.Exists(projectImagesPath))
                {
                    imagesPath.CreateDirectory();
                    var iconFiles = Directory.GetFiles(projectImagesPath, "*.ico");
                    foreach (var iconFile in iconFiles)
                    {
                        File.Copy(iconFile, imagesPath / Path.GetFileName(iconFile), true);
                        Serilog.Log.Information($"  Copied: {Path.GetFileName(iconFile)}");
                    }
                }
            }

            // Copy PackageContents.xml to bundle root
            var packageContentsPath = navisToolsProject.Directory / "PackageContents.xml";
            if (File.Exists(packageContentsPath))
            {
                File.Copy(packageContentsPath, BundleDirectory / "PackageContents.xml", true);
                Serilog.Log.Information("Copied: PackageContents.xml");
            }

            Serilog.Log.Information($"Bundle created successfully at: {BundleDirectory}");
        });

}
