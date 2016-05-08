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

      var eplRenderer = new EPLRenderer();

      {
        var svgLineTranslator = new SvgLineTranslator(svgUnitCalculator);

        var svgRectangleTranslator = new SvgRectangleTranslator(svgLineTranslator,
                                                                svgUnitCalculator);

        var svgTextTranslator = new SvgTextTranslator(svgUnitCalculator);
        var svgTextSpanTranslator = new SvgTextSpanTranslator(svgUnitCalculator);

        eplRenderer.RegisterTranslator(svgLineTranslator);
        eplRenderer.RegisterTranslator(svgRectangleTranslator);
        eplRenderer.RegisterTranslator(svgTextTranslator);
        eplRenderer.RegisterTranslator(svgTextSpanTranslator);
      }

      return eplRenderer;
    }
  }
}