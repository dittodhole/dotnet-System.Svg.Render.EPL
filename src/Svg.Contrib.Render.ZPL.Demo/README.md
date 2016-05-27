# dotnet-Svg.Contrib.Render.ZPL.Demo

This demo converts [`assets/label.svg`](assets/label.svg) into [Zebra Programming Language (ZPL)](https://en.wikipedia.org/wiki/Zebra_(programming_language)) code and sends it to all connected printers:

![](assets/label.gif)

![](https://media.giphy.com/media/lA3qoZE4TKQhi/giphy.gif)

## Features

This demo has some additional hacks to show off the extensibility of [Svg.Contrib.Render.ZPL](../Svg.Contrib.Render.ZPL):

- [`Svg.Contrib.Render.ZPL.Demo.CustomBootstrapper`](CustomBootstrapper.cs)
  - adapts some factories
- [`Svg.Contrib.Render.ZPL.Demo.SvgImageTranslator`](SvgImageTranslator.cs)
  - when encountering a `SvgImage`-instance with `data-barcode` attribute set, the barcode is written directly instead of writing a graphic
  - adapts the barcode selection for some images
