![Icon](assets/icon.png)

# dotnet-Svg.Contrib.Render.EPL
> Convert [SVG](https://en.wikipedia.org/wiki/Scalable_Vector_Graphics) to [Eltron Programming Language (EPL)](https://en.wikipedia.org/wiki/Eltron_Programming_Language)

## Installing

[![NuGet Status](http://img.shields.io/nuget/v/Svg.Contrib.Render.EPL.svg?style=flat)](https://www.nuget.org/packages/Svg.Contrib.Render.EPL/) https://www.nuget.org/packages/Svg.Contrib.Render.EPL/

    PM> Install-Package Svg.Contrib.Render.EPL

## Example

```
using Svg;
using Svg.Contrib.Render.EPL;

var file = "";
var svgDocument = SvgDocument.Open(file);
var bootstrapper = new DefaultBootstrapper();
var eplRenderer = bootstrapper.BuildUp(sourceDpi: 90f,
                                       destinationDpi: 203f,
                                       printerCodepage: PrinterCodepage.Dos850,
                                       viewRotation: ViewRotation.Normal);
var encoding = eplRenderer.GetEncoding();

var eplStream = eplRenderer.GetTranslation(svgDocument);
var array = eplStream.GetByteArray(encoding);

// TODO send to printer over USB/COM/Network
```

## Configuration

I strongly encourage you to use the [`DefaultBootstrapper`](DefaultBootstrapper.cs) (or extend it) to build up [`EplRenderer`](EplRenderer.cs)-instances.

#### sourceDpi
Type: `float`

Define the DPI used to create the [SVG](https://en.wikipedia.org/wiki/Scalable_Vector_Graphics)-file (if using [Inkscape](https://inkscape.org): `90f`).

#### destinationDpi
Type: `float`

Define the DPI of the printer (usually `203f`).

#### printerCodepage
Type: [`PrinterCodepage`](Enums.cs#L6)

Depending on the text used in `A`-command you can set a codepage to guarantee a correct output.

#### countryCode
Type: `int`

See [`printerCodepage`](#printercodepage)

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
- native barcodes (see [Svg.Contrib.Render.EPL.Demo](../Svg.Contrib.Render.EPL.Demo))

## License

dotnet-Svg.Contrib.Render.EPL is published under [WTFNMFPLv3](https://github.com/dittodhole/WTFNMFPLv3)

## Icon

[Zebra](https://thenounproject.com/term/zebra/201040/) by [Cole M Johnstone](https://thenounproject.com/colemjohnstone) from the Noun Project
