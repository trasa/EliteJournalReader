$ErrorActionPreference = "Stop"

$sdkProjectPath = "EliteJournalReader/EliteJournalReader.csproj"
$feedTesterPath = "EliteJournalFeedTester/EliteJournalFeedTester.csproj"
$distDir = "dist"
$nupkgOutDir = "$distDir/nupkg"
$feedTesterOutDir = "$distDir/FeedTester"

# clean
Remove-Item $nupkgOutDir -Recurse -Force -ErrorAction SilentlyContinue

# rebuild dirs
New-Item -ItemType Directory -Path $nupkgOutDir | Out-Null

dotnet build $sdkProjectPath --configuration Release

dotnet pack $sdkProjectPath `
    --configuration Release `
    --output $nupkgOutDir `
    /p:PackageId="EliteJournalReader" `
    /p:IncludeSymbols=false `
    /p:IncludeSource=false

dotnet publish $feedTesterPath `
    --configuration Debug `
    --output $feedTesterOutDir `
    --self-contained true `
    --runtime win-x64 `
    -p:PublishSingleFile=true `
    -p:PublishTrimmed=false

# local only: copy the package to my local packages dir:
cp $nupkgOutDir/*.nupkg ~/local-nuget-packages

