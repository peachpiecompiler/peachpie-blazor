Param(
  [string]$version,
  [string]$suffix
)

# nuget.exe pack
$nugetargs = "pack", "$PSScriptRoot/../src/Peachpie.Blazor.Templates.nuspec", "-OutputDirectory", "out"
if ($version) { $nugetargs += "-Version", $version }
if ($suffix) { $nugetargs += "-Suffix", $suffix }
./tools/nuget.exe $nugetargs