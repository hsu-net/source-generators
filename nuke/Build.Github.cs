﻿using System;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using MimeMapping;
using Nuke.Common;
using Nuke.Common.CI.GitHubActions;
using Nuke.Common.IO;
using Nuke.Common.Tools.GitHub;
using Octokit;
using Serilog;
// ReSharper disable AllUnderscoreLocalParameterName

#pragma warning disable S3903

[GitHubActions(
    "build",
    GitHubActionsImage.UbuntuLatest,
    AutoGenerate = true,
    PublishArtifacts = true,
    EnableGitHubToken = true,
    OnPushBranches = new[] { DevelopBranch },
    InvokedTargets = new[] { nameof(Artifacts) },
    CacheKeyFiles = new string[0]
)]
[GitHubActions(
    "deploy",
    GitHubActionsImage.UbuntuLatest,
    AutoGenerate = true,
    PublishArtifacts = true,
    EnableGitHubToken = true,
    OnPushBranches = new[] { MainBranch, PreviewBranch },
    InvokedTargets = new[] { nameof(Deploy) },
    ImportSecrets = new[] { nameof(NuGetApiKey), nameof(MyGetApiKey) },
    CacheKeyFiles = new string[0]
)]
partial class Build
{
    GitHubActions GitHubActions => GitHubActions.Instance;

    Target Release => _ => _
        .Description("Release")
        .DependsOn(Artifacts)
        .ProceedAfterFailure()
        .Executes(async () =>
        {
            try
            {
                var version = $"{GetVersionPrefix()}{GetVersionSuffix()}";
                var tag = $"v{version}";
                GitHubTasks.GitHubClient.Credentials ??= new Credentials(GitHubActions.Token.NotNull());
                var release = await GitHubTasks.GitHubClient.Repository.Release.Create(
                    Repository.GetGitHubOwner(),
                    Repository.GetGitHubName(),
                    new NewRelease(tag)
                    {
                        Name = tag,
                        Prerelease = true,
                        Draft = true,
                        Body = $"Release v{version} at {DateTimeNow():yyyy-MM-dd HH:mm:ss}"
                    });

                var uploads = ArtifactsDirectory.GlobFiles("**/*").NotNull().Select(async x =>
                {
                    await using var assetFile = File.OpenRead(x);
                    var asset = new ReleaseAssetUpload
                    {
                        FileName = x.Name,
                        ContentType = MimeUtility.GetMimeMapping(x),
                        RawData = assetFile
                    };
                    await GitHubTasks.GitHubClient.Repository.Release.UploadAsset(release, asset);
                }).ToArray();

                Task.WaitAll(uploads);
            }
            catch (Exception e)
            {
                Log.Error(e, "Failed to create GitHub release");
            }
        });
}
