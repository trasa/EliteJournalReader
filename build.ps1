$ErrorActionPreference = "Stop"

$sdkProjectPath = "EliteJournalReader/EliteJournalReader.csproj"
$distDir = "dist"
$nupkgOutDir = "$distDir/nupkg"

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
