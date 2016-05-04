namespace System.Svg.Render.EPL
{
  public static class DefaultBootstrapper
  {
    public static SvgDocumentTranslator Create(int sourceDpi)
    {
      var svgUnitCalculator = new SvgUnitCalculator
                              {
                                SourceDpi = sourceDpi
                              };

      var svgLineTranslator = new SvgLineTranslator(svgUnitCalculator);

      var svgRectangleTranslator = new SvgRectangleTranslator(svgLineTranslator,
                                                              svgUnitCalculator);

      var svgTextTranslator = new SvgTextTranslator(svgUnitCalculator);

      var svgGroupTranslator = new SvgGroupTranslator(svgUnitCalculator);

      var svgDocumentTranslator = new SvgDocumentTranslator();
      svgDocumentTranslator.AddSvgElementTranslator(svgLineTranslator);
      svgDocumentTranslator.AddSvgElementTranslator(svgRectangleTranslator);
      svgDocumentTranslator.AddSvgElementTranslator(svgTextTranslator);
      svgDocumentTranslator.AddSvgElementTranslator(svgGroupTranslator);

      return svgDocumentTranslator;
    }
  }
}