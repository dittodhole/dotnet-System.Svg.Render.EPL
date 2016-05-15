using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public static class DefaultBootstrapper
  {
    [NotNull]
    public static EplRenderer Create(float sourceDpi,
                                     float targetDpi,
                                     PrinterCodepage printerCodepage,
                                     int countryCode,
                                     bool assumeStoredInInternalMemory = false)
    {
      var svgUnitReader = new SvgUnitReader();
      var eplTransformer = new EplTransformer(svgUnitReader);
      var viewMatrix = eplTransformer.CreateViewMatrix(sourceDpi,
                                                       targetDpi);
      var eplRenderer = new EplRenderer(viewMatrix,
                                        printerCodepage,
                                        countryCode);

      var eplCommands = new EplCommands();

      {
        var svgLineTranslator = new SvgLineTranslator(eplTransformer,
                                                      eplCommands);

        var svgRectangleTranslator = new SvgRectangleTranslator(eplTransformer,
                                                                eplCommands,
                                                                svgUnitReader);

        var svgTextTranslator = new SvgTextBaseTranslator<SvgText>(eplTransformer,
                                                                   eplCommands);
        var svgTextSpanTranslator = new SvgTextBaseTranslator<SvgTextSpan>(eplTransformer,
                                                                           eplCommands);

        var svgPathTranslator = new SvgPathTranslator(eplTransformer,
                                                      eplCommands);

        var svgImageTranslator = new SvgImageTranslator(eplTransformer,
                                                        eplCommands)
                                 {
                                   AssumeStoredInInternalMemory = assumeStoredInInternalMemory
                                 };

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