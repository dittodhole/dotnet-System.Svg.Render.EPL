namespace System.Svg.Render.EPL
{
  public static class DefaultBootstrapper
  {
    public static EPLRenderer Create(float sourceDpi,
                                     float targetDpi)
    {
      var svgUnitCalculator = new SvgUnitCalculator();

      var viewMatrix = SvgUnitCalculator.CreateViewMatrix(sourceDpi,
                                                          targetDpi);
      var eplRenderer = new EPLRenderer(svgUnitCalculator,
                                        viewMatrix);

      {
        var svgLineTranslator = new SvgLineTranslator(svgUnitCalculator);

        var svgRectangleTranslator = new SvgRectangleTranslator(svgUnitCalculator,
                                                                svgLineTranslator);

        var svgTextTranslator = new SvgTextBaseTranslator<SvgText>(svgUnitCalculator);
        var svgTextSpanTranslator = new SvgTextBaseTranslator<SvgTextSpan>(svgUnitCalculator);

        var svgPathTranslator = new SvgPathTranslator(svgLineTranslator);

        var svgImageTranslator = new SvgImageTranslator(svgUnitCalculator);

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