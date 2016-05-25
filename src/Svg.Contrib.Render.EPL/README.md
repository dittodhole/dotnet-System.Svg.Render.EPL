![Icon](assets/icon.png)

# dotnet-Svg.Contrib.Render.EPL
> Convert [SVG](https://en.wikipedia.org/wiki/Scalable_Vector_Graphics) to [Eltron Programming Language (EPL)](https://en.wikipedia.org/wiki/Eltron_Programming_Language)

## Installing

[![NuGet Status](http://img.shields.io/nuget/v/Svg.Contrib.Render.EPL.svg?style=flat)](https://www.nuget.org/packages/Svg.Contrib.Render.EPL/) https://www.nuget.org/packages/Svg.Contrib.Render.EPL/

    PM> Install-Package Svg.Contrib.Render.EPL

## Example

```
var file = "";
var svgDocument = Svg.SvgDocument.Open(file);
var bootstrapper = new Svg.Contrib.Render.EPL.DefaultBootstrapper();
var eplRenderer = bootstrapper.BuildUp(sourceDpi: 90f,
                                       destinationDpi: 203f,
                                       printerCodepage: PrinterCodepage.Dos850,
                                       countryCode: 850);
var encoding = eplRenderer.GetEncoding();

// you can obtain eplStreams to set the internal memory of the printer
// this is especially useful when there are static images inside your svgDocument.
// these can be uploaded instead of direct embedding, which takes off some bytes
// from the concrete translation.
var eplStreams = eplRenderer.GetInternalMemoryTranslation(svgDocument);

var eplStream = eplRenderer.GetTranslation(svgDocument);
var array = eplStream.GetByteArray(encoding);
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

## Features

- `<text>` and `<tspan>`
- `<rectangle>`
- `<line>`
- `<path>` (only L supported at the moment)
- `<image>`
- `transform=""`
- `x=""`
- `x1=""`
- `x2=""`
- `y=""`
- `y1=""`
- `y2=""`
- `width=""`
- `height=""`
- `style="fill"`
- `style="stroke"`
- `style="stroke-width"`
- `style="visible"`
- native barcodes (see [Svg.Contrib.Render.EPL.Demo](https://github.com/dittodhole/dotnet-Svg.Contrib.Render/tree/master/src/Svg.Contrib.Render.EPL.Demo))

## License

dotnet-Svg.Contrib.Render.EPL is published under [WTFNMFPLv3](https://github.com/dittodhole/WTFNMFPLv3)

## Icon

[Zebra](https://thenounproject.com/term/zebra/201040/) by [Cole M Johnstone](https://thenounproject.com/colemjohnstone) from the Noun Project
