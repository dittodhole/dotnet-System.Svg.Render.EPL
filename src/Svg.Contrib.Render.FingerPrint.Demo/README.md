# dotnet-Svg.Contrib.Render.FingerPrint.Demo

This demo converts [`assets/label.svg`](assets/label.svg) into [Intermec FingerPrint](http://www.adc-distribution.de/intermec_etikettendrucker/fingerprint_info.pdf) code and sends it to COM1:

![](assets/label.gif)

![](https://media.giphy.com/media/lA3qoZE4TKQhi/giphy.gif)

## Features

This demo has some additional hacks to show off the extensibility of [Svg.Contrib.Render.FingerPrint](../Svg.Contrib.Render.FingerPrint):

- [`CustomBootstrapper`](CustomBootstrapper.cs)
  - adapts some factories
- [`SvgTextBaseTranslator`](SvgTextBaseTranslator.cs)
  - adapts the position of some labels
- [`SvgImageTranslator`](SvgImageTranslator.cs)
  - when encountering a `SvgImage`-instance with `data-barcode` attribute set, the barcode is written directly instead of writing a graphic
  - adapts the barcode selection for some images
