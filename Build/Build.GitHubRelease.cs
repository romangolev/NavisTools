using Nuke.Common;
using Nuke.Common.Git;
using Nuke.Common.Tools.GitHub;
using Nuke.Common.Tools.GitVersion;
using Octokit;
using Serilog;
using System.Text.RegularExpressions;

partial class Build
{
    [GitVersion(NoFetch = true)] readonly GitVersion GitVersion;
    [Parameter] string GitHubToken { get; set; }
    [GitRepository] readonly GitRepository GitRepository;

    AbsolutePath ChangeLogPath => RootDirectory / "CHANGELOG.md";

    Target PublishGitHubRelease => _ => _
        .DependsOn(CreateInstaller)
        .Requires(() => GitHubToken)
        .Requires(() => GitRepository)
        .Requires(() => GitVersion)
        .OnlyWhenStatic(() => GitRepository.IsOnMainOrMasterBranch())
        .OnlyWhenStatic(() => IsServerBuild)
        .Executes(() =>
        {
            GitHubTasks.GitHubClient = new GitHubClient(new ProductHeaderValue(Solution.Name))
            {
                Credentials = new Credentials(GitHubToken)
            };

            var gitHubName = GitRepository.GetGitHubName();
            var gitHubOwner = GitRepository.GetGitHubOwner();
            var installerFiles = Directory.GetFiles(OutputDirectory / "installer", "*.exe");
            
            if (installerFiles.Length == 0)
            {
                throw new Exception("No installer files found to publish");
            }

            var version = GetVersionFromInstaller(installerFiles[0]);
            
            CheckTags(gitHubOwner, gitHubName, version);
            Log.Information("Publishing Release: {Version}", version);

            var newRelease = new NewRelease(version)
            {
                Name = $"NavisTools v{version}",
                Body = CreateChangelog(version),
                Draft = false,
                TargetCommitish = GitVersion.Sha
            };

            var release = CreateRelease(gitHubOwner, gitHubName, newRelease);
            UploadArtifacts(release, installerFiles);
            
            Log.Information("Release published successfully!");
        });

    string GetVersionFromInstaller(string installerPath)
    {
        var fileName = Path.GetFileName(installerPath);
        var match = Regex.Match(fileName, @"v(\d+\.\d+\.\d+)");
        if (match.Success)
        {
            return match.Groups[1].Value;
        }
        
        // Fallback to assembly version
        var version = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version;
        return $"{version.Major}.{version.Minor}.{version.Build}";
    }

    void CheckTags(string gitHubOwner, string gitHubName, string version)
    {
        var tag = $"v{version}";
        try
        {
            var release = GitHubTasks.GitHubClient.Repository.Release.Get(gitHubOwner, gitHubName, tag).Result;
            if (release != null)
            {
                throw new Exception($"Release {tag} already exists. Please increment version in Directory.Build.props");
            }
        }
        catch (AggregateException ex) when (ex.InnerException is NotFoundException)
        {
            // Tag doesn't exist, which is what we want
        }
    }

    string CreateChangelog(string version)
    {
        if (!File.Exists(ChangeLogPath))
        {
            Log.Warning("CHANGELOG.md not found at: {Path}", ChangeLogPath);
            return string.Empty;
        }

        Log.Information("Reading changelog from: {Path}", ChangeLogPath);

        var logBuilder = new System.Text.StringBuilder();
        var versionRegex = new Regex($@"##\s*\[?{Regex.Escape(version)}\]?");
        var sectionStarted = false;

        foreach (var line in File.ReadLines(ChangeLogPath))
        {
            if (sectionStarted)
            {
                // Stop at next version section
                if (line.StartsWith("## [") || line.StartsWith("## v"))
                    break;
                    
                logBuilder.AppendLine(line);
            }
            else if (versionRegex.IsMatch(line))
            {
                sectionStarted = true;
            }
        }

        if (logBuilder.Length == 0)
        {
            Log.Warning("No changelog entry found for version: {Version}", version);
            return $"Release v{version}";
        }

        return logBuilder.ToString().Trim();
    }

    Release CreateRelease(string gitHubOwner, string gitHubName, NewRelease newRelease)
    {
        Log.Information("Creating release: {Name}", newRelease.Name);
        
        var release = GitHubTasks.GitHubClient.Repository.Release
            .Create(gitHubOwner, gitHubName, newRelease)
            .Result;
            
        return release;
    }

    void UploadArtifacts(Release release, string[] artifacts)
    {
        foreach (var artifact in artifacts)
        {
            var fileName = Path.GetFileName(artifact);
            Log.Information("Uploading artifact: {File}", fileName);

            using var stream = File.OpenRead(artifact);
            var upload = new ReleaseAssetUpload
            {
                FileName = fileName,
                ContentType = "application/octet-stream",
                RawData = stream
            };

            GitHubTasks.GitHubClient.Repository.Release
                .UploadAsset(release, upload)
                .Wait();
                
            Log.Information("  Uploaded: {File}", fileName);
        }
    }
}
