![](assets/noun_201040_cc.png)

# dotnet-Svg.Contrib.Render.EPL

> Convert [SVG](https://en.wikipedia.org/wiki/Scalable_Vector_Graphics) to [Eltron Programming Language (EPL)](https://en.wikipedia.org/wiki/Eltron_Programming_Language)

## Build status

[![](https://img.shields.io/appveyor/ci/dittodhole/dotnet-svg-contrib-render.svg)](https://ci.appveyor.com/project/dittodhole/dotnet-svg-contrib-render)

## Installing

### myget.org

[![](https://img.shields.io/myget/dittodhole/vpre/Svg.Contrib.Render.EPL.svg)](https://www.myget.org/feed/dittodhole/package/nuget/Svg.Contrib.Render.EPL)

```powershell
PM> Install-Package -Id Svg.Contrib.Render.EPL -pre --source https://www.myget.org/F/dittodhole/api/v2
```

### nuget.org

[![](https://img.shields.io/nuget/v/Svg.Contrib.Render.EPL.svg)](https://www.nuget.org/packages/Svg.Contrib.Render.EPL)

```powershell
PM> Install-Package -Id Svg.Contrib.Render.EPL
```

## Example

```csharp
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
var viewMatrix = bootstrapper.CreateViewMatrix(eplTransformer,
                                               sourceDpi: 90f,
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
- native barcodes

## License

dotnet-Svg.Contrib.Render.EPL is published under [WTFNMFPLv3](https://github.com/dittodhole/WTFNMFPLv3)

## Icon

[Zebra](https://thenounproject.com/term/zebra/201040/) by [Cole M Johnstone](https://thenounproject.com/colemjohnstone) from the Noun Project.
