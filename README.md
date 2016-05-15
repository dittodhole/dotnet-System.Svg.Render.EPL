![Icon](assets/icon.png)
# dotnet-System.Svg.Render.EPL

Yo, it's 2016 and we are still writing [EPL](https://en.wikipedia.org/wiki/Eltron_Programming_Language) code.

![](https://media.giphy.com/media/YA6dmVW0gfIw8/giphy.gif)

This has some major downsides:
- You have to write commands without any visual feedback
- Testing the layout of a label can only be done by printing it
- There is no common abstraction, which brings massive pain when creating different representations of the same label ([PNG](https://en.wikipedia.org/wiki/Portable_Network_Graphics) for A4 printers, [ZPL](https://en.wikipedia.org/wiki/ZPL_programming_language), [Fingerprint](http://apps.intermec.com/downloads/eps_man/937-023-003/Default.htm), ... whatsoever)
- Some *clever* one may come up with sending an image to the printer, but please read [this article](http://web.archive.org/web/20150306101851/http://nicholas.piasecki.name/blog/2009/03/sending-raw-epl2-directly-to-a-zebra-lp2844-via-c) to realize the limitations

This proof-of-concept project tries to overcome this limitations by translating [SVG](https://en.wikipedia.org/wiki/Scalable_Vector_Graphics)-files to [EPL](https://en.wikipedia.org/wiki/Eltron_Programming_Language).

![](https://media.giphy.com/media/1rpg1ZDVKcdSo/giphy.gif)

## Getting started

Open `src/System.Svg.Render.EPL.sln` with Microsoft Visual Studio 2015 and build it.

Following NuGet packages will be restored:
- [System.Svg](https://www.nuget.org/packages/System.Svg)
  - [ExCSS Stylesheet Parser](https://www.nuget.org/packages/ExCSS/2.0.5)
  - [Fizzler](https://www.nuget.org/packages/Fizzler)
- [Magick.NET-Q8-AnyCPU](https://www.nuget.org/packages/Magick.NET-Q8-AnyCPU)
- [JetBrains.Annotations](https://www.nuget.org/packages/JetBrains.Annotations)

A fully working demo is available in the `demo`-branch.

You can start by using the `System.Svg.Render.EPL.DefaultBootstrapper`:

## Installing [![NuGet Status](http://img.shields.io/nuget/v/System.Svg.Render.EPL.svg?style=flat)](https://www.nuget.org/packages/System.Svg.Render.EPL/)

tbd

## Example

```
var file = "";
var svgDocument = System.Svg.SvgDocument.Open(file);
var bootstrapper = new System.Svg.Render.EPL.DefaultBootstrapper();
var eplRenderer = bootstrapper.BuildUp(90f,
                                       203f,
                                       PrinterCodepage.Dos850,
                                       850,
                                       false);
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

I strongly encourage you to use the `DefaultBootstrapper` (or extend it) to build up `EplRenderer`-instances.

#### sourceDpi
Type: `float`

Define the DPI used to create the [SVG](https://en.wikipedia.org/wiki/Scalable_Vector_Graphics)-file (if using [Inkscape](https://inkscape.org): `90f`).

#### targetDpi
Type: `float`

Define the DPI of the printer (usually `203f`).

#### printerCodepage
Type: `PrinterCodepage`

Depending on the text used in `A`-command you can set a codepage to guarantee a correct output.

#### countryCode
Type: `int`

See [`printerCodepage`](#printerCodepage)

#### assumeStoredInInternalMemory
Type: `bool`
Default: `false`

[`SvgImageTranslator`](src/System.Svg.Render.EPL/SvgImageTranslator.cs) keeps track of previously uploaded images through `TranslateForStoring`-calls. If images are not stored, the image is printed directly with a [`GW`](http://support.zebra.com/cpws/docs/eltron/epl2/GW_Command.pdf) command (which is the preferred soltion for non-static images). Otherwise a [`GG`](http://support.zebra.com/cpws/docs/eltron/epl2/GG_Command.pdf) command is used (which is the preferred solution for static images).

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
- native barcode printing - see [DEMO](https://github.com/dittodhole/dotnet-System.Svg.Render.EPL/tree/demo)

## License

dotnet-System.Svg.Render.EPL is published under [WTFNMFPLv3](https://github.com/dittodhole/WTFNMFPLv3).

## Icon

[Zebra](https://thenounproject.com/term/zebra/201040/) by [Cole M Johnstone](https://thenounproject.com/colemjohnstone) from the Noun Project.
