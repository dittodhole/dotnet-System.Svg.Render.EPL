using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.ZPL
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
    protected virtual ZplTransformer CreateZplTransformer([NotNull] SvgUnitReader svgUnitReader) => new ZplTransformer(svgUnitReader);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual Matrix CreateViewMatrix([NotNull] ZplTransformer zplTransformer,
                                              float sourceDpi,
                                              float destinationDpi,
                                              ViewRotation viewRotation) => zplTransformer.CreateViewMatrix(sourceDpi,
                                                                                                            destinationDpi,
                                                                                                            viewRotation);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual ZplRenderer CreateZplRenderer([NotNull] Matrix viewMatrix,
                                                    [NotNull] ZplCommands zplCommands,
                                                    CharacterSet characterSet) => new ZplRenderer(viewMatrix,
                                                                                                  zplCommands,
                                                                                                  characterSet);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual ZplCommands CreateZplCommands() => new ZplCommands();

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual SvgLineTranslator CreateSvgLineTranslator([NotNull] ZplTransformer zplTransformer,
                                                                [NotNull] ZplCommands zplCommands) => new SvgLineTranslator(zplTransformer,
                                                                                                                            zplCommands);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual SvgRectangleTranslator CreateSvgRectangleTranslator([NotNull] ZplTransformer zplTransformer,
                                                                          [NotNull] ZplCommands zplCommands,
                                                                          [NotNull] SvgUnitReader svgUnitReader) => new SvgRectangleTranslator(zplTransformer,
                                                                                                                                               zplCommands,
                                                                                                                                               svgUnitReader);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual SvgTextBaseTranslator<SvgText> CreateSvgTextTranslator([NotNull] ZplTransformer zplTransformer,
                                                                             [NotNull] ZplCommands zplCommands) => new SvgTextBaseTranslator<SvgText>(zplTransformer,
                                                                                                                                                      zplCommands);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual SvgTextBaseTranslator<SvgTextSpan> CreateSvgTextSpanTranslator([NotNull] ZplTransformer zplTransformer,
                                                                                     [NotNull] ZplCommands zplCommands) => new SvgTextBaseTranslator<SvgTextSpan>(zplTransformer,
                                                                                                                                                                  zplCommands);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual SvgPathTranslator CreateSvgPathTranslator([NotNull] ZplTransformer zplTransformer,
                                                                [NotNull] ZplCommands zplCommands) => new SvgPathTranslator(zplTransformer,
                                                                                                                            zplCommands);

    //[NotNull]
    //[Pure]
    //[MustUseReturnValue]
    //protected virtual SvgImageTranslator CreateSvgImageTranslator([NotNull] ZplTransformer zplTransformer,
    //                                                              [NotNull] ZplCommands zplCommands) => new SvgImageTranslator(zplTransformer,
    //                                                                                                                           zplCommands);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual ZplRenderer BuildUp(float sourceDpi,
                                       float destinationDpi,
                                       CharacterSet characterSet = CharacterSet.ZebraCodePage850,
                                       ViewRotation viewRotation = ViewRotation.Normal)
    {
      var svgUnitReader = this.CreateSvgUnitReader(sourceDpi);
      var zplTransformer = this.CreateZplTransformer(svgUnitReader);
      var viewMatrix = this.CreateViewMatrix(zplTransformer,
                                             sourceDpi,
                                             destinationDpi,
                                             viewRotation);
      var zplCommands = this.CreateZplCommands();
      var zplRenderer = this.CreateZplRenderer(viewMatrix,
                                               zplCommands,
                                               characterSet);
      var svgLineTranslator = this.CreateSvgLineTranslator(zplTransformer,
                                                           zplCommands);
      var svgRectangleTranslator = this.CreateSvgRectangleTranslator(zplTransformer,
                                                                     zplCommands,
                                                                     svgUnitReader);
      var svgTextTranslator = this.CreateSvgTextTranslator(zplTransformer,
                                                           zplCommands);
      var svgTextSpanTranslator = this.CreateSvgTextSpanTranslator(zplTransformer,
                                                                   zplCommands);
      var svgPathTranslator = this.CreateSvgPathTranslator(zplTransformer,
                                                           zplCommands);
      //var svgImageTranslator = this.CreateSvgImageTranslator(eplTransformer,
      //                                                       eplCommands);

      zplRenderer.RegisterTranslator(svgLineTranslator);
      zplRenderer.RegisterTranslator(svgRectangleTranslator);
      zplRenderer.RegisterTranslator(svgTextTranslator);
      zplRenderer.RegisterTranslator(svgTextSpanTranslator);
      zplRenderer.RegisterTranslator(svgPathTranslator);
      //zplRenderer.RegisterTranslator(svgImageTranslator);

      return zplRenderer;
    }
  }
}