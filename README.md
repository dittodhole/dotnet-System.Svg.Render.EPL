# dotnet-Svg.Contrib.Render
> This is 2016, and we are still writing printer commands ... burn in hell!

![](https://media.giphy.com/media/gCHHJZGvOtstO/giphy.gif)

## Installing

[![NuGet Status](http://img.shields.io/nuget/v/Svg.Contrib.Render.EPL.svg?style=flat-square)](https://www.nuget.org/packages/Svg.Contrib.Render.EPL/) https://www.nuget.org/packages/Svg.Contrib.Render.EPL/

    PM> Install-Package Svg.Contrib.Render.EPL

[![NuGet Status](http://img.shields.io/nuget/v/Svg.Contrib.Render.ZPL.svg?style=flat-square)](https://www.nuget.org/packages/Svg.Contrib.Render.ZPL/) https://www.nuget.org/packages/Svg.Contrib.Render.ZPL/

    PM> Install-Package Svg.Contrib.Render.ZPL

[![NuGet Status](http://img.shields.io/nuget/v/Svg.Contrib.Render.FingerPrint.svg?style=flat-square)](https://www.nuget.org/packages/Svg.Contrib.Render.ZPL/) https://www.nuget.org/packages/Svg.Contrib.Render.FingerPrint/

    PM> Install-Package Svg.Contrib.Render.FingerPrint

[![NuGet Status](http://img.shields.io/nuget/v/Svg.Contrib.ViewModel.svg?style=flat-square)](https://www.nuget.org/packages/Svg.Contrib.ViewModel/) https://www.nuget.org/packages/Svg.Contrib.ViewModel/

    PM> Install-Package Svg.Contrib.ViewModel

## Developing

[![Travis](https://img.shields.io/travis/dittodhole/dotnet-Svg.Contrib.Render.svg?style=flat-square)](https://travis-ci.org/dittodhole/dotnet-Svg.Contrib.Render)

Open [`src/Svg.Contrib.Render.sln`](src/Svg.Contrib.Render.sln) with Microsoft Visual Studio 2015 and build it.

Following [NuGet](https://www.nuget.org/) packages will be restored:
- [Svg](https://www.nuget.org/packages/Svg)
- [JetBrains.Annotations](https://www.nuget.org/packages/JetBrains.Annotations)
- [Magick.NET-Q8-AnyCPU](https://www.nuget.org/packages/Magick.NET-Q8-AnyCPU)
- [PInvoke.Kernel32](https://www.nuget.org/packages/PInvoke.Kernel32)
- [PInvoke.SetupApi](https://www.nuget.org/packages/PInvoke.SetupApi)
- [PInvoke.Windows.Core](https://www.nuget.org/packages/PInvoke.Windows.Core)

### Deploying

In order to push *.nupkg*-files to [NuGet](https://www.nuget.org/), you need following command:

    > build.bat

[powershell-nuget-packager](https://github.com/dittodhole/powershell-nuget-packager) is used to package the assembly and is included as a submodule
