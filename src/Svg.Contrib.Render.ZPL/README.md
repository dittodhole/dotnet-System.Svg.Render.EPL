![](assets/noun_201040_cc.png)

# dotnet-Svg.Contrib.Render.ZPL

> Convert [SVG](https://en.wikipedia.org/wiki/Scalable_Vector_Graphics) to [Zebra Programming Language (ZPL)](https://en.wikipedia.org/wiki/Zebra_(programming_language))

## Build status

[![](https://img.shields.io/appveyor/ci/dittodhole/dotnet-svg-contrib-render.svg)](https://ci.appveyor.com/project/dittodhole/dotnet-svg-contrib-render)

## Installing

### myget.org

[![](https://img.shields.io/myget/dittodhole/vpre/Svg.Contrib.Render.ZPL.svg)](https://www.myget.org/feed/dittodhole/package/nuget/Svg.Contrib.Render.ZPL)

```powershell
PM> Install-Package -Id Svg.Contrib.Render.ZPL -pre --source https://www.myget.org/F/dittodhole/api/v2
```

### nuget.org

[![](https://img.shields.io/nuget/v/Svg.Contrib.Render.ZPL.svg)](https://www.nuget.org/packages/Svg.Contrib.Render.ZPL)

```powershell
PM> Install-Package -Id Svg.Contrib.Render.ZPL
```

## Example

```csharp
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

[Zebra](https://thenounproject.com/term/zebra/201040/) by [Cole M Johnstone](https://thenounproject.com/colemjohnstone) from the Noun Project.
