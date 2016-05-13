namespace System.Svg.Render.EPL
{
  public static class DefaultBootstrapper
  {
    public static EPLRenderer Create(float sourceDpi,
                                     float targetDpi,
                                     PrinterCodepage printerCodepage,
                                     int countryCode)
    {
      var svgUnitCalculator = new SvgUnitCalculator();

      var viewMatrix = SvgUnitCalculator.CreateViewMatrix(sourceDpi,
                                                          targetDpi);
      var eplRenderer = new EPLRenderer(svgUnitCalculator,
                                        viewMatrix,
                                        printerCodepage,
                                        countryCode);

      var encoding = eplRenderer.Encoding;

      var eplCommands = new EplCommands(encoding);

      {
        var svgLineTranslator = new SvgLineTranslator(svgUnitCalculator,
                                                      eplCommands);

        var svgRectangleTranslator = new SvgRectangleTranslator(svgUnitCalculator,
                                                                svgLineTranslator,
                                                                encoding);

        var svgTextTranslator = new SvgTextBaseTranslator<SvgText>(svgUnitCalculator,
                                                                   encoding);
        var svgTextSpanTranslator = new SvgTextBaseTranslator<SvgTextSpan>(svgUnitCalculator,
                                                                           encoding);

        var svgPathTranslator = new SvgPathTranslator(svgLineTranslator,
                                                      encoding);

        var svgImageTranslator = new SvgImageTranslator(svgUnitCalculator,
                                                        eplCommands);

        eplRenderer.RegisterTranslator(svgLineTranslator);
        eplRenderer.RegisterTranslator(svgRectangleTranslator);
        eplRenderer.RegisterTranslator(svgTextTranslator);
        eplRenderer.RegisterTranslator(svgTextSpanTranslator);
        eplRenderer.RegisterTranslator(svgPathTranslator);
        eplRenderer.RegisterTranslator(svgImageTranslator);
      }

      return eplRenderer;
    }
  }
}