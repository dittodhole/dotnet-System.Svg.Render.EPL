using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class DefaultBootstrapper
  {
    [NotNull]
    protected virtual SvgUnitReader CreateSvgUnitReader() => new SvgUnitReader();

    [NotNull]
    protected virtual EplTransformer CreateEplTransformer([NotNull] SvgUnitReader svgUnitReader) => new EplTransformer(svgUnitReader);

    [NotNull]
    protected virtual Matrix CreateViewMatrix([NotNull] EplTransformer eplTransformer,
                                              float sourceDpi,
                                              float destinationDpi) => eplTransformer.CreateViewMatrix(sourceDpi,
                                                                                                       destinationDpi);

    [NotNull]
    protected virtual EplRenderer CreateEplRenderer([NotNull] Matrix viewMatrix,
                                                    [NotNull] EplCommands eplCommands,
                                                    PrinterCodepage printerCodepage,
                                                    int countryCode) => new EplRenderer(viewMatrix,
                                                                                        eplCommands,
                                                                                        printerCodepage,
                                                                                        countryCode);

    [NotNull]
    protected virtual EplCommands CreateEplCommands() => new EplCommands();

    [NotNull]
    protected virtual SvgLineTranslator CreateSvgLineTranslator([NotNull] EplTransformer eplTransformer,
                                                                [NotNull] EplCommands eplCommands) => new SvgLineTranslator(eplTransformer,
                                                                                                                            eplCommands);

    [NotNull]
    protected virtual SvgRectangleTranslator CreateSvgRectangleTranslator([NotNull] EplTransformer eplTransformer,
                                                                          [NotNull] EplCommands eplCommands,
                                                                          [NotNull] SvgUnitReader svgUnitReader) => new SvgRectangleTranslator(eplTransformer,
                                                                                                                                               eplCommands,
                                                                                                                                               svgUnitReader);

    [NotNull]
    protected virtual SvgTextBaseTranslator<SvgText> CreateSvgTextTranslator([NotNull] EplTransformer eplTransformer,
                                                                             [NotNull] EplCommands eplCommands) => new SvgTextBaseTranslator<SvgText>(eplTransformer,
                                                                                                                                                      eplCommands);

    [NotNull]
    protected virtual SvgTextBaseTranslator<SvgTextSpan> CreateSvgTextSpanTranslator([NotNull] EplTransformer eplTransformer,
                                                                                     [NotNull] EplCommands eplCommands) => new SvgTextBaseTranslator<SvgTextSpan>(eplTransformer,
                                                                                                                                                                  eplCommands);

    [NotNull]
    protected virtual SvgPathTranslator CreateSvgPathTranslator([NotNull] EplTransformer eplTransformer,
                                                                [NotNull] EplCommands eplCommands) => new SvgPathTranslator(eplTransformer,
                                                                                                                            eplCommands);

    [NotNull]
    protected virtual SvgImageTranslator CreateSvgImageTranslator([NotNull] EplTransformer eplTransformer,
                                                                  [NotNull] EplCommands eplCommands,
                                                                  bool assumeStoredInInternalMemory) => new SvgImageTranslator(eplTransformer,
                                                                                                                               eplCommands)
                                                                                                        {
                                                                                                          AssumeStoredInInternalMemory = assumeStoredInInternalMemory
                                                                                                        };

    [NotNull]
    public virtual EplRenderer BuildUp(float sourceDpi,
                                       float destinationDpi,
                                       PrinterCodepage printerCodepage,
                                       int countryCode,
                                       bool assumeStoredInInternalMemory = false)
    {
      var svgUnitReader = this.CreateSvgUnitReader();
      var eplTransformer = this.CreateEplTransformer(svgUnitReader);
      var viewMatrix = this.CreateViewMatrix(eplTransformer,
                                             sourceDpi,
                                             destinationDpi);
      var eplCommands = this.CreateEplCommands();
      var eplRenderer = this.CreateEplRenderer(viewMatrix,
                                               eplCommands,
                                               printerCodepage,
                                               countryCode);
      var svgLineTranslator = this.CreateSvgLineTranslator(eplTransformer,
                                                           eplCommands);
      var svgRectangleTranslator = this.CreateSvgRectangleTranslator(eplTransformer,
                                                                     eplCommands,
                                                                     svgUnitReader);
      var svgTextTranslator = this.CreateSvgTextTranslator(eplTransformer,
                                                           eplCommands);
      var svgTextSpanTranslator = this.CreateSvgTextSpanTranslator(eplTransformer,
                                                                   eplCommands);
      var svgPathTranslator = this.CreateSvgPathTranslator(eplTransformer,
                                                           eplCommands);
      var svgImageTranslator = this.CreateSvgImageTranslator(eplTransformer,
                                                             eplCommands,
                                                             assumeStoredInInternalMemory);

      eplRenderer.RegisterTranslator(svgLineTranslator);
      eplRenderer.RegisterTranslator(svgRectangleTranslator);
      eplRenderer.RegisterTranslator(svgTextTranslator);
      eplRenderer.RegisterTranslator(svgTextSpanTranslator);
      eplRenderer.RegisterTranslator(svgPathTranslator);
      eplRenderer.RegisterTranslator(svgImageTranslator);

      return eplRenderer;
    }
  }
}