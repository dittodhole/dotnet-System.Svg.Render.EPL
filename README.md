# dotnet-Svg.Contrib.Render
> This is 2016, and we are still writing printer commands ... burn in hell!

![](https://media.giphy.com/media/gCHHJZGvOtstO/giphy.gif)

## Developing

Open [`src/Svg.Contrib.Render.sln`](src/Svg.Contrib.Render.sln) with Microsoft Visual Studio 2015 and build it.

Following [NuGet](https://www.nuget.org/) packages will be restored:
- [System.Svg](https://www.nuget.org/packages/System.Svg)
  - [ExCSS Stylesheet Parser](https://www.nuget.org/packages/ExCSS/2.0.5)
  - [Fizzler](https://www.nuget.org/packages/Fizzler)
- [JetBrains.Annotations](https://www.nuget.org/packages/JetBrains.Annotations)
- [Magick.NET-Q8-AnyCPU](https://www.nuget.org/packages/Magick.NET-Q8-AnyCPU)
- [PInvoke.Kernel32](https://www.nuget.org/packages/PInvoke.Kernel32)
- [PInvoke.SetupApi](https://www.nuget.org/packages/PInvoke.SetupApi)
- [PInvoke.Windows.Core](https://www.nuget.org/packages/PInvoke.Windows.Core)

### Deploying

In order to push *.nupkg*-files to [NuGet](https://www.nuget.org/), you need following command:

    > build.bat

[powershell-nuget-packager](https://github.com/dittodhole/powershell-nuget-packager) is used to package the assembly and is included as a submodule
