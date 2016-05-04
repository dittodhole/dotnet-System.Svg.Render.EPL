# dotnet-System.Svg.Render.EPL

This project aims to provide a translation/compilation/transpilation of SVG to EPL. You can read *.svg*-files with `System.Svg.SvgDocument.Open(path:string)`.

It references [System.Svg](https://github.com/dittodhole/dotnet-System.Svg), a fork of [SVG.NET](https://github.com/vvvv/SVG) without all the actual image-rendering bloat. Additionally, [ExCSS](https://github.com/TylerBrinks/ExCSS) and [Fizzler](https://code.google.com/archive/p/fizzler) are referenced instead.

## Getting started

You can start by using the `System.Svg.Render.EPL.DefaultBootstrapper`:

```
var sourceDpi = 90; // we are coming from Inkscape
var targetDpi = 203; // basic dpi for most EPL printers
var file = "";
var svgDocument = System.Svg.SvgDocument.Open(file);
var svgDocumentTranslator = System.Svg.Render.EPL.DefaultBootstrapper.Create(sourceDpi);
var translation = svgDocumentTranslator.Translate(svgDocument,
                                                  targetDpi);
// TADADADA
```

## I am interested, tell me more ...

### Currently implemented elements

- `<text>` and `<tspan>` (*No font-size and magnification-factor at the very moment ... :sad:*, with white stroke for inverting text)
- `<group>` (to define a transformation for a whole group of elements)
- `<rectangle>` (with white filling for inverting text)
- `<line>`
- `<svg>` (if any transformation exists on the root)

### Currently implemented attributes

- `transform`
- `text`
- `x`
- `x1` (`<line>`)
- `x2` (`<line>`)
- `y`
- `y1` (`<line>`)
- `y2` (`<line>`)
- `width`
- `height`
- content of `<text>` or `<tspan>`

### Currently implemented styles
- `fill` (`<rectangle>`) w/ solid color
- `stroke` (`<rectangle>`, `<line>`, `<text>`) w/ solid color
- `stroke-width`
- `visible`

## Logging

As barely any exceptions are thrown, it may be essential to attach a logging-suite and look for `error` messages.
[LibLog](https://github.com/damianh/LibLog) is used for logging, which silently integrates into your concrete logging framework. Alternatively, you can create your own logger by deriving from `System.Svg.Render.EPL.Logging.ILogProvider`

## Installing [![NuGet Status](http://img.shields.io/nuget/v/System.Svg.Render.EPL.svg?style=flat)](https://www.nuget.org/packages/System.Svg.Render.EPL/)

tbd

## License

dotnet-System.Svg.Render.EPL is published under [WTFNMFPLv3](https://github.com/dittodhole/WTFNMFPLv3).
