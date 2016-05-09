namespace System.Svg.Render.EPL
{
  public static class DefaultBootstrapper
  {
    public static EPLRenderer Create(int sourceDpi,
                                     SvgUnitType userUnitTypeSubstituion = SvgUnitType.Pixel)
    {
      var svgUnitCalculator = new SvgUnitCalculator
                              {
                                SourceDpi = sourceDpi,
                                UserUnitTypeSubstitution = userUnitTypeSubstituion
                              };

      var eplRenderer = new EPLRenderer(svgUnitCalculator);

      {
        var svgLineTranslator = new SvgLineTranslator(svgUnitCalculator);

        var svgRectangleTranslator = new SvgRectangleTranslator(svgUnitCalculator);

        var svgTextTranslator = new SvgTextBaseTranslator<SvgText>(svgUnitCalculator);
        var svgTextSpanTranslator = new SvgTextBaseTranslator<SvgTextSpan>(svgUnitCalculator);

        eplRenderer.RegisterTranslator(svgLineTranslator);
        eplRenderer.RegisterTranslator(svgRectangleTranslator);
        eplRenderer.RegisterTranslator(svgTextTranslator);
        eplRenderer.RegisterTranslator(svgTextSpanTranslator);
      }

      return eplRenderer;
    }
  }
}