![Icon](assets/icon.png)

# dotnet-System.Svg.Render.EPL
> Convert [SVG](https://en.wikipedia.org/wiki/Scalable_Vector_Graphics) to [Eltron Programming Language (EPL)](https://en.wikipedia.org/wiki/Eltron_Programming_Language)

## Installing [![NuGet Status](http://img.shields.io/nuget/v/System.Svg.Render.EPL.svg?style=flat)](https://www.nuget.org/packages/System.Svg.Render.EPL/)

https://www.nuget.org/packages/System.Svg.Render.EPL/

    PM> Install-Package System.Svg.Render.EPL

## Example

```
var file = "";
var svgDocument = System.Svg.SvgDocument.Open(file);
var bootstrapper = new System.Svg.Render.EPL.DefaultBootstrapper();
var eplRenderer = bootstrapper.BuildUp(sourceDpi: 90f,
                                       destinationDpi: 203f,
                                       printerCodepage: PrinterCodepage.Dos850,
                                       countryCode: 850,
                                       assumeStoredInInternalMemory: false);
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

I strongly encourage you to use the [`DefaultBootstrapper`](src/System.Svg.Render.EPL/DefaultBootstrapper.cs) (or extend it) to build up [`EplRenderer`](src/System.Svg.Render.EPL/EplRenderer.cs)-instances.

#### sourceDpi
Type: `float`

Define the DPI used to create the [SVG](https://en.wikipedia.org/wiki/Scalable_Vector_Graphics)-file (if using [Inkscape](https://inkscape.org): `90f`).

#### destinationDpi
Type: `float`

Define the DPI of the printer (usually `203f`).

#### printerCodepage
Type: [`PrinterCodepage`](src/System.Svg.Render.EPL/Enums.cs#L3)

Depending on the text used in `A`-command you can set a codepage to guarantee a correct output.

#### countryCode
Type: `int`

See [`printerCodepage`](#printercodepage)

#### assumeStoredInInternalMemory
Type: `bool`

Default: `false`

[`SvgImageTranslator`](src/System.Svg.Render.EPL/SvgImageTranslator.cs) keeps track of previously uploaded images through [`TranslateForStoring`](src/System.Svg.Render.EPL/SvgImageTranslator.cs#L127)-calls. If images are not stored, the image is printed directly with a [`GW`](http://support.zebra.com/cpws/docs/eltron/epl2/GW_Command.pdf) command (which is the preferred soltion for non-static images). Otherwise a [`GG`](http://support.zebra.com/cpws/docs/eltron/epl2/GG_Command.pdf) command is used (which is the preferred solution for static images).

This detection is bound to an actual [`SvgImageTranslator`](src/System.Svg.Render.EPL/SvgImageTranslator.cs) instance - so the reusage of the same instance is strongly recommended throughout the application's lifetime. (Default behaviour with [`System.Svg.Render.EPL.DefaultBootstrapper`](src/System.Svg.Render.EPL/DefaultBootstrapper.cs))

To further minimize the writes to the internal memory (Zebra claims around 100k write at most), you can force [`GG`](http://support.zebra.com/cpws/docs/eltron/epl2/GG_Command.pdf) commands even if the current instance did not write the image to the internal memory.

*Please read [this Stackoverflow answer](http://stackoverflow.com/a/18559256/57508) for more information on this issue.*

**CAUTION:** This may result in broken labels, so you should know what you are doing.

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
- native barcodes (see [System.Svg.Render.EPL.Demo](https://github.com/dittodhole/dotnet-Svg.Contrib.Render/tree/master/src/System.Svg.Render.EPL.Demo))

## License

dotnet-System.Svg.Render.EPL is published under [WTFNMFPLv3](https://github.com/dittodhole/WTFNMFPLv3)

## Icon

[Zebra](https://thenounproject.com/term/zebra/201040/) by [Cole M Johnstone](https://thenounproject.com/colemjohnstone) from the Noun Project
