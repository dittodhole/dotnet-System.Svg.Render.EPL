![Icon](assets/icon.png)

# dotnet-Svg.Contrib.Render.FingerPrint
> Convert [SVG](https://en.wikipedia.org/wiki/Scalable_Vector_Graphics) to [Intermec FingerPrint](http://www.adc-distribution.de/intermec_etikettendrucker/fingerprint_info.pdf)

## Installing

[![NuGet Status](http://img.shields.io/nuget/v/Svg.Contrib.Render.FingerPrint.svg?style=flat-square)](https://www.nuget.org/packages/Svg.Contrib.Render.FingerPrint/) https://www.nuget.org/packages/Svg.Contrib.Render.FingerPrint/

    PM> Install-Package Svg.Contrib.Render.FingerPrint

## Example

```
using System.Linq;
using System.Text;
using Svg;
using Svg.Contrib.Render.FingerPrint;

var file = "";
var svgDocument = SvgDocument.Open(file);
var bootstrapper = new DefaultBootstrapper();
var fingerPrintTransformer = bootstrapper.CreateFingerPrintTransformer();
var fingerPrintRenderer = bootstrapper.CreateFingerPrintRenderer(fingerPrintTransformer);
var viewMatrix = bootstrapper.CreateViewMatrix(fingerPrintTransformer,
                                               sourceDpi: 90f,
                                               destinationDpi: 203f,
                                               viewRotation: ViewRotation.Normal);
var fingerPrintContainer = fingerPrintRenderer.GetTranslation(svgDocument,
                                                              viewMatrix);

var encoding = fingerPrintRenderer.GetEncoding();
var array = fingerPrintContainer.ToByteStream(encoding)
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

dotnet-Svg.Contrib.Render.FingerPrint is published under [WTFNMFPLv3](https://github.com/dittodhole/WTFNMFPLv3)

## Icon

[Fingerprint](https://thenounproject.com/term/fingerprint/286941/) by [Roselin Christina.S](https://thenounproject.com/rosttarose) from the Noun Project
