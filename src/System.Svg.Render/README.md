![Icon](assets/icon.png)

# dotnet-System.Svg.Render

This project provides base functionality used by following [SVG](https://en.wikipedia.org/wiki/Scalable_Vector_Graphics)-compilers:

- [dotnet-System.Svg.Render.EPL](https://github.com/dittodhole/dotnet-System.Svg.Render.EPL)
- [dotnet-System.Svg.Render.ZPL](https://github.com/dittodhole/dotnet-System.Svg.Render.ZPL)

## Installing [![NuGet Status](http://img.shields.io/nuget/v/System.Svg.Render.svg?style=flat)](https://www.nuget.org/packages/System.Svg.Render/)

https://www.nuget.org/packages/System.Svg.Render/

    PM> Install-Package System.Svg.Render

## Developing

Open [`src/System.Svg.Render.sln`](src/System.Svg.Render.sln) with Microsoft Visual Studio 2015 and build it.

Following [NuGet](https://www.nuget.org/) packages will be restored:
- [System.Svg](https://www.nuget.org/packages/System.Svg)
  - [ExCSS Stylesheet Parser](https://www.nuget.org/packages/ExCSS/2.0.5)
  - [Fizzler](https://www.nuget.org/packages/Fizzler)
- [JetBrains.Annotations](https://www.nuget.org/packages/JetBrains.Annotations)

### Deploying

In order to push *.nupkg*-files to [NuGet](https://www.nuget.org/), you need following command:

    > build.bat

[powershell-nuget-packager](https://github.com/dittodhole/powershell-nuget-packager) is used to package the assembly and is included as a submodule.

## License

dotnet-System.Svg.Render is published under [WTFNMFPLv3](https://github.com/dittodhole/WTFNMFPLv3).

## Icon

[SVG File](https://thenounproject.com/term/svg-file/321865/) by [Viktor Vorobyev](https://thenounproject.com/vityavorobyev) from the Noun Project.
