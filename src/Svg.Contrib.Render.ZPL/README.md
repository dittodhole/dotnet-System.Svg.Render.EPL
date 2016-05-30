![Icon](assets/icon.png)

# dotnet-Svg.Contrib.Render.ZPL
> Convert [SVG](https://en.wikipedia.org/wiki/Scalable_Vector_Graphics) to [Zebra Programming Language (ZPL)](https://en.wikipedia.org/wiki/Zebra_(programming_language))

## Installing

[![NuGet Status](http://img.shields.io/nuget/v/Svg.Contrib.Render.ZPL.svg?style=flat-square)](https://www.nuget.org/packages/Svg.Contrib.Render.ZPL/) https://www.nuget.org/packages/Svg.Contrib.Render.ZPL/

    PM> Install-Package Svg.Contrib.Render.ZPL

## Example

```
using System.Linq;
using Svg;
using Svg.Contrib.Render.ZPL;

var file = "";
var svgDocument = SvgDocument.Open(file);
var bootstrapper = new DefaultBootstrapper();
var zplRenderer = bootstrapper.BuildUp(sourceDpi: 90f,
                                       destinationDpi: 203f,
                                       characterSet: CharacterSet.ZebraCodePage850,
                                       viewRotation: ViewRotation.Normal);
var encoding = zplRenderer.GetEncoding();

var zplContainer = zplRenderer.GetTranslation(svgDocument);
var array = zplContainer.ToByteStream(encoding)
                        .ToArray();

// TODO send to printer over USB/COM/Network
```

## Configuration

I strongly encourage you to use the [`DefaultBootstrapper`](DefaultBootstrapper.cs) (or extend it) to build up [`ZplRenderer`](ZplRenderer.cs)-instances.

#### sourceDpi
Type: `float`

Define the DPI used to create the [SVG](https://en.wikipedia.org/wiki/Scalable_Vector_Graphics)-file (if using [Inkscape](https://inkscape.org): `90f`).

#### destinationDpi
Type: `float`

Define the DPI of the printer (usually `203f`).

#### characterSet
Type: [`CharacterSet`](Enums.cs#L36)  
Default: `CharacterSet.ZebraCodePage850`

Define the character set used for encoding your strings.

#### viewRotation
Type: [`ViewRotation`](../Svg.Contrib.Render/Enums.cs#L6)  
Default: `ViewRotation.Normal`

Define the rotation of the label.

## Features

- `SvgText`
- `SvgTextSpan`
- `SvgRectangle`
- `SvgLineSegment`
- `SvgLine`
- `SvgImage`
- native barcodes (see [Svg.Contrib.Render.ZPL.Demo](../Svg.Contrib.Render.ZPL.Demo))

## License

dotnet-Svg.Contrib.Render.ZPL is published under [WTFNMFPLv3](https://github.com/dittodhole/WTFNMFPLv3)

## Icon

[Zebra](https://thenounproject.com/term/zebra/201040/) by [Cole M Johnstone](https://thenounproject.com/colemjohnstone) from the Noun Project
