$repositoryRoot = split-path $MyInvocation.MyCommand.Definition
$toolsPath = join-path $repositoryRoot ".dotnet"
$getDotNet = join-path $toolsPath "dotnet-install.ps1"
$nugetExePath = join-path $toolsPath "nuget.exe"

write-host "Download latest install script from CLI repo"

New-Item -type directory -f -path $toolsPath | Out-Null

Invoke-WebRequest https://raw.githubusercontent.com/dotnet/cli/rel/1.0.0-preview2/scripts/obtain/dotnet-install.ps1 -OutFile $getDotNet

$env:DOTNET_INSTALL_DIR="$repositoryRoot\.dotnet\win7-x64"

if (!(Test-Path $env:DOTNET_INSTALL_DIR)) {
    New-Item -type directory -path $env:DOTNET_INSTALL_DIR | Out-Null
}


& $getDotNet -arch x64

$env:PATH = "$env:DOTNET_INSTALL_DIR;$env:PATH"

$scriptPath = split-path -parent $MyInvocation.MyCommand.Definition

$configuration = "Release"

$nuspecDir = Join-Path $scriptPath "NuSpecs"

if (!(Test-Path .\nuget.exe)) {
    wget "https://dist.nuget.org/win-x86-commandline/latest/nuget.exe" -outfile .\nuget.exe
}
$msbuild = Get-ItemProperty "hklm:\SOFTWARE\Microsoft\MSBuild\ToolsVersions\14.0"

# TODO: if not found, bail out
$msbuildExe = Join-Path $msbuild.MSBuildToolsPath "msbuild.exe"

# get version
.\nuget.exe install -excludeversion -pre gitversion.commandline -outputdirectory packages
.\packages\gitversion.commandline\tools\gitversion.exe /l console /output buildserver /updateassemblyinfo

$versionObj = .\packages\gitversion.commandline\tools\gitversion.exe | ConvertFrom-Json 

$version = $versionObj.MajorMinorPatch
$tag = $versionObj.PreReleaseLabel
$preRelNum = $versionObj.CommitsSinceVersionSourcePadded

if($tag -ne ""){
  $version = "$version-$tag-$preRelNum"
}


Write-Host "Version: $version"

# Get Reference Generator
.\nuget.exe install -excludeversion -pre NuSpec.ReferenceGenerator -outputdirectory packages

Write-Host "Restoring packages" -Foreground Green
dotnet restore $scriptPath | out-null

#need to ensure core is built first due to bad dependency order determination by dotnet build

Write-Host "Building projects" -Foreground Green
$projects = gci $scriptPath -Directory -Recurse `
   | Where-Object { ($_.Name -notlike "*DeviceRunner") -and (Test-Path (Join-Path $_.FullName "project.json"))  } `

foreach ($project in $projects) {
  dotnet build -c "$configuration" $project.FullName    
}

