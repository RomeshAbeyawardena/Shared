Param(
    [string]$directory,
    [string]$version
)

if($directory -eq [System.String]::Empty) {
    $directory = $PSScriptRoot
}

&"$directory\UpdateVersion-Powershell.ps1" -FileName $directory\Directory.Build.Props -Version $version  

$child_directories = Get-ChildItem $directory -Directory
Foreach ($dir in $child_directories)
{
    "--------------- Processing $dir ---------------" 
    cd $dir.FullName
    dotnet pack --include-symbols --include-source -o "$directory/nuget"
    cd ..
    "--------------- Processed $dir ---------------"
}