﻿[cmdletbinding()]
Param(
	[string]$applicationName,
	[string]$token,
	[string]$repo,
	[string]$owner,
	[string]$tagName,
	[string]$releaseName,
	[string]$releaseBody,
	[string]$draft,
	[string]$branch,
	[string]$assetsPattern
)
Write-Verbose -Verbose "Entering script PublishRelease.ps1"
Write-Verbose -Verbose "applicationName = $applicationName"
Write-Verbose -Verbose "token = $token"
Write-Verbose -Verbose "repo = $repo"
Write-Verbose -Verbose "owner = $owner"
Write-Verbose -Verbose "tagName = $tagName"
Write-Verbose -Verbose "releaseName = $releaseName"
Write-Verbose -Verbose "releaseBody = $releaseBody"
Write-Verbose -Verbose "draft = $draft"
Write-Verbose -Verbose "branch = $branch"
Write-Verbose -Verbose "assetsPattern = $assetsPattern"

[bool]$prereleaseBool = $false
If ($branch -eq "develop") {$prereleaseBool = $true}

# Convert checkbox params to booleans
[bool]$draftBool = [System.Convert]::ToBoolean($draftBool)

Write-Output "draftBool = $draftBool"
Write-Output "prereleaseBool = $prereleaseBool"

#$env:PSModulePath=$env:PSModulePath + ";" + "F:\Program Files (x86)\Microsoft SQL Server\110\Tools\PowerShell\Modules"

# Import the Task.Common and Task.Internal dll that has all the cmdlets we need for Build
import-module "$($env:AGENT_HOMEDIRECTORY)\externals\vstshost\Microsoft.TeamFoundation.DistributedTask.Task.Internal"
import-module "$($env:AGENT_HOMEDIRECTORY)\externals\vstshost\Microsoft.TeamFoundation.DistributedTask.Task.Common"

# Import PublishGitHubRelease assembly
$scriptDir = Split-Path -Parent $MyInvocation.MyCommand.Path
$pathToModule = Join-Path $scriptDir "PublishGitHubRelease.dll"
import-module $pathToModule

# Travers all matching files
$assets = Find-Files -SearchPattern $assetsPattern
Publish-GitHubRelease -ApplicationName $applicationName -Token $token -Repo $repo -Owner $owner -TagName $tagName -ReleaseName $releaseName -ReleaseBody $releaseBody -Draft $draftBool -PreRelease $prereleaseBool -Assets $assets
