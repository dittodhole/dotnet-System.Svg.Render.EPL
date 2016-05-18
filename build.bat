powershell-nuget-packager\NuGet.exe restore src/System.Svg.Render.EPL.sln
"%PROGRAMFILES(x86)%\MSBuild\14.0\Bin\MsBuild.exe" src/System.Svg.Render.EPL.sln
powershell powershell-nuget-packager\Package.ps1