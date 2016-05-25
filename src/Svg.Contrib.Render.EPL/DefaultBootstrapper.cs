using System.Drawing.Drawing2D;
using System.Svg;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class DefaultBootstrapper
  {
    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual SvgUnitReader CreateSvgUnitReader(float sourceDpi) => new SvgUnitReader(sourceDpi);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual EplTransformer CreateEplTransformer([NotNull] SvgUnitReader svgUnitReader) => new EplTransformer(svgUnitReader);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual Matrix CreateViewMatrix([NotNull] EplTransformer eplTransformer,
                                              float sourceDpi,
                                              float destinationDpi,
                                              ViewRotation viewRotation) => eplTransformer.CreateViewMatrix(sourceDpi,
                                                                                                            destinationDpi,
                                                                                                            viewRotation);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual EplRenderer CreateEplRenderer([NotNull] Matrix viewMatrix,
                                                    [NotNull] EplCommands eplCommands,
                                                    PrinterCodepage printerCodepage,
                                                    int countryCode) => new EplRenderer(viewMatrix,
                                                                                        eplCommands,
                                                                                        printerCodepage,
                                                                                        countryCode);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual EplCommands CreateEplCommands() => new EplCommands();

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual SvgLineTranslator CreateSvgLineTranslator([NotNull] EplTransformer eplTransformer,
                                                                [NotNull] EplCommands eplCommands) => new SvgLineTranslator(eplTransformer,
                                                                                                                            eplCommands);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual SvgRectangleTranslator CreateSvgRectangleTranslator([NotNull] EplTransformer eplTransformer,
                                                                          [NotNull] EplCommands eplCommands,
                                                                          [NotNull] SvgUnitReader svgUnitReader) => new SvgRectangleTranslator(eplTransformer,
                                                                                                                                               eplCommands,
                                                                                                                                               svgUnitReader);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual SvgTextBaseTranslator<SvgText> CreateSvgTextTranslator([NotNull] EplTransformer eplTransformer,
                                                                             [NotNull] EplCommands eplCommands) => new SvgTextBaseTranslator<SvgText>(eplTransformer,
                                                                                                                                                      eplCommands);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual SvgTextBaseTranslator<SvgTextSpan> CreateSvgTextSpanTranslator([NotNull] EplTransformer eplTransformer,
                                                                                     [NotNull] EplCommands eplCommands) => new SvgTextBaseTranslator<SvgTextSpan>(eplTransformer,
                                                                                                                                                                  eplCommands);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual SvgPathTranslator CreateSvgPathTranslator([NotNull] EplTransformer eplTransformer,
                                                                [NotNull] EplCommands eplCommands) => new SvgPathTranslator(eplTransformer,
                                                                                                                            eplCommands);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual SvgImageTranslator CreateSvgImageTranslator([NotNull] EplTransformer eplTransformer,
                                                                  [NotNull] EplCommands eplCommands) => new SvgImageTranslator(eplTransformer,
                                                                                                                               eplCommands);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual EplRenderer BuildUp(float sourceDpi,
                                       float destinationDpi,
                                       PrinterCodepage printerCodepage,
                                       int countryCode,
                                       ViewRotation viewRotation)
    {
      var svgUnitReader = this.CreateSvgUnitReader(sourceDpi);
      var eplTransformer = this.CreateEplTransformer(svgUnitReader);
      var viewMatrix = this.CreateViewMatrix(eplTransformer,
                                             sourceDpi,
                                             destinationDpi,
                                             viewRotation);
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
                                                             eplCommands);

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