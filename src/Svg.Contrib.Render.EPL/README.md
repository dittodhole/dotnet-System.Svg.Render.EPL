![Icon](assets/icon.png)

# dotnet-Svg.Contrib.Render.EPL
> Convert [SVG](https://en.wikipedia.org/wiki/Scalable_Vector_Graphics) to [Eltron Programming Language (EPL)](https://en.wikipedia.org/wiki/Eltron_Programming_Language)

## Installing

[![NuGet Status](http://img.shields.io/nuget/v/Svg.Contrib.Render.EPL.svg?style=flat-square)](https://www.nuget.org/packages/Svg.Contrib.Render.EPL/) https://www.nuget.org/packages/Svg.Contrib.Render.EPL/

    PM> Install-Package Svg.Contrib.Render.EPL

## Example

```
using System.Linq;
using Svg;
using Svg.Contrib.Render.EPL;

var file = "";
var svgDocument = SvgDocument.Open(file);
var bootstrapper = new DefaultBootstrapper();
var eplTransformer = bootstrapper.CreateEplTransformer();
var eplRenderer = bootstrapper.CreateEplRenderer(eplTransformer,
                                                 printerCodepage: PrinterCodepage.Dos850,
                                                 countryCode: 850);
var viewMatrix = bootstrapper.CreateViewMatrix(sourceDpi: 90f,
                                               destinationDpi: 203f,
                                               viewRotation: ViewRotation.Normal);
var eplContainer = eplRenderer.GetTranslation(svgDocument,
                                              viewMatrix);

var encoding = eplRenderer.GetEncoding();
var array = eplContainer.ToByteStream(encoding)
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
- native barcodes (see [Svg.Contrib.Render.EPL.Demo](../Svg.Contrib.Render.EPL.Demo))

## License

dotnet-Svg.Contrib.Render.EPL is published under [WTFNMFPLv3](https://github.com/dittodhole/WTFNMFPLv3)

## Icon

[Zebra](https://thenounproject.com/term/zebra/201040/) by [Cole M Johnstone](https://thenounproject.com/colemjohnstone) from the Noun Project
