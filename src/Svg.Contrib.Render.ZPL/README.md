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
var zplTransformer = bootstrapper.CreateZplTransformer();
var zplRenderer = bootstrapper.CreateZplRenderer(zplTransformer,
                                                 characterSet: CharacterSet.ZebraCodePage850);
var viewMatrix = bootstrapper.CreateViewMatrix(zplTransformer,
                                               sourceDpi: 90f,
                                               destinationDpi: 203f,
                                               viewRotation: ViewRotation.Normal);
var zplContainer = zplRenderer.GetTranslation(svgDocument,
                                              viewMatrix);

var encoding = zplRenderer.GetEncoding();
var array = zplContainer.ToByteStream(encoding)
                        .ToArray();

// TODO send to printer over USB/COM/Network
```

## Features

- `SvgText`
- `SvgTextSpan`
- `SvgRectangle`
- `SvgLineSegment`
- `SvgLine`
- `SvgImage`
- native barcodes

## License

dotnet-Svg.Contrib.Render.ZPL is published under [WTFNMFPLv3](https://github.com/dittodhole/WTFNMFPLv3)

## Icon

[Zebra](https://thenounproject.com/term/zebra/201040/) by [Cole M Johnstone](https://thenounproject.com/colemjohnstone) from the Noun Project
