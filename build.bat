powershell-nuget-packager\NuGet.exe restore src/Svg.Contrib.Render.sln
"%PROGRAMFILES(x86)%\MSBuild\14.0\Bin\MsBuild.exe" src/Svg.Contrib.Render.sln
powershell powershell-nuget-packager\Package.ps1