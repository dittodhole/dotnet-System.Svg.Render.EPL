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

      var svgDocumentTranslator = new SvgDocumentTranslator(svgUnitCalculator);

      {
        var svgLineTranslator = new SvgLineTranslator(svgUnitCalculator);

        var svgRectangleTranslator = new SvgRectangleTranslator(svgLineTranslator,
                                                                svgUnitCalculator);

        var svgTextTranslator = new SvgTextTranslator(svgUnitCalculator);

        var svgGroupTranslator = new SvgGroupTranslator(svgUnitCalculator);

        svgDocumentTranslator.RegisterTranslator(svgLineTranslator);
        svgDocumentTranslator.RegisterTranslator(svgRectangleTranslator);
        svgDocumentTranslator.RegisterTranslator(svgTextTranslator);
        svgDocumentTranslator.RegisterTranslator(svgGroupTranslator);
        svgDocumentTranslator.RegisterTranslator(svgDocumentTranslator);
      }

      return svgDocumentTranslator;
    }
  }
}