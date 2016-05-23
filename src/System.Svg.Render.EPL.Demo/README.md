# dotnet-System.Svg.Render.EPL.Demo

This demo converts [`assets/label.svg`](assets/label.svg) into [EPL](https://en.wikipedia.org/wiki/Eltron_Programming_Language) code and sends it to all connected printers:

![](assets/label.gif)

![](https://media.giphy.com/media/lA3qoZE4TKQhi/giphy.gif)

## Developing

Open [`src/System.Svg.Render.EPL.Demo.sln`](src/System.Svg.Render.EPL.Demo.sln) with Microsoft Visual Studio 2015 and build it.

Following NuGet packages will be restored:
- [PInvoke.Kernel32](https://www.nuget.org/packages/PInvoke.Kernel32)
- [PInvoke.SetupApi](https://www.nuget.org/packages/PInvoke.SetupApi)
- [PInvoke.Windows.Core](https://www.nuget.org/packages/PInvoke.Windows.Core)
- [JetBrains.Annotations](https://www.nuget.org/packages/JetBrains.Annotations)
- [System.Svg.Render.EPL](https://www.nuget.org/packages/System.Svg.Render.EPL/)
  - [Magick.NET-Q8-AnyCPU](https://www.nuget.org/packages/Magick.NET-Q8-AnyCPU)
  - [System.Svg.Render](https://www.nuget.org/packages/System.Svg.Render/)
    - [System.Svg](https://www.nuget.org/packages/System.Svg)
      - [ExCSS Stylesheet Parser](https://www.nuget.org/packages/ExCSS/2.0.5)
      - [Fizzler](https://www.nuget.org/packages/Fizzler)

## Features

This demo has some additional hacks to show off the extensibility of [System.Svg.Render.EPL](https://www.nuget.org/packages/System.Svg.Render.EPL/):

- [`ConsoleApplication1.CustomBootstrapper`](src/ConsoleApplication1/CustomBootstrapper.cs)
  - adapts some factories
- [`ConsoleApplication1.EplTransformer`](src/ConsoleApplication1/EplTransformer.cs)
  - adapts the font selection for some labels
  - adapts the position of some labels
- [`ConsoleApplication1.SvgImageTranslator`](src/ConsoleApplication1/SvgImageTranslator.cs)
  - when encountering a `SvgImage`-instance with `data-barcode` attribute set, the barcode is written directly instead of writing a graphic
  - adapts the barcode selection for some images
  - adapts the position for some images

## License

dotnet-System.Svg.Render.EPL.Demo is published under [WTFNMFPLv3](https://github.com/dittodhole/WTFNMFPLv3).

## Icon

[Zebra](https://thenounproject.com/term/zebra/201040/) by [Cole M Johnstone](https://thenounproject.com/colemjohnstone) from the Noun Project.
