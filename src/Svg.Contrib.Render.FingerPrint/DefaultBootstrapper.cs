using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.FingerPrint
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
    protected virtual FingerPrintTransformer CreateFingerPrintTransformer([NotNull] SvgUnitReader svgUnitReader) => new FingerPrintTransformer(svgUnitReader);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual Matrix CreateViewMatrix([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                              float sourceDpi,
                                              float destinationDpi,
                                              ViewRotation viewRotation) => fingerPrintTransformer.CreateViewMatrix(sourceDpi,
                                                                                                                    destinationDpi,
                                                                                                                    viewRotation);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual FingerPrintRenderer CreateFingerPrintRenderer([NotNull] Matrix viewMatrix,
                                                                    [NotNull] FingerPrintCommands fingerPrintCommands) => new FingerPrintRenderer(viewMatrix,
                                                                                                                                                  fingerPrintCommands);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual FingerPrintCommands CreateFingerPrintCommands() => new FingerPrintCommands();

    //[NotNull]
    //[Pure]
    //[MustUseReturnValue]
    //protected virtual SvgLineTranslator CreateSvgLineTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
    //                                                            [NotNull] FingerPrintCommands fingerPrintCommands) => new SvgLineTranslator(fingerPrintTransformer,
    //                                                                                                                        fingerPrintCommands);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual SvgRectangleTranslator CreateSvgRectangleTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                          [NotNull] FingerPrintCommands fingerPrintCommands,
                                                                          [NotNull] SvgUnitReader svgUnitReader) => new SvgRectangleTranslator(fingerPrintTransformer,
                                                                                                                                               fingerPrintCommands,
                                                                                                                                               svgUnitReader);

    //[NotNull]
    //[Pure]
    //[MustUseReturnValue]
    //protected virtual SvgTextBaseTranslator<SvgText> CreateSvgTextTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
    //                                                                         [NotNull] FingerPrintCommands fingerPrintCommands) => new SvgTextBaseTranslator<SvgText>(fingerPrintTransformer,
    //                                                                                                                                                  fingerPrintCommands);

    //[NotNull]
    //[Pure]
    //[MustUseReturnValue]
    //protected virtual SvgTextBaseTranslator<SvgTextSpan> CreateSvgTextSpanTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
    //                                                                                 [NotNull] FingerPrintCommands fingerPrintCommands) => new SvgTextBaseTranslator<SvgTextSpan>(fingerPrintTransformer,
    //                                                                                                                                                              fingerPrintCommands);

    //[NotNull]
    //[Pure]
    //[MustUseReturnValue]
    //protected virtual SvgPathTranslator CreateSvgPathTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
    //                                                            [NotNull] FingerPrintCommands fingerPrintCommands) => new SvgPathTranslator(fingerPrintTransformer,
    //                                                                                                                        fingerPrintCommands);

    //[NotNull]
    //[Pure]
    //[MustUseReturnValue]
    //protected virtual SvgImageTranslator CreateSvgImageTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
    //                                                              [NotNull] FingerPrintCommands fingerPrintCommands) => new SvgImageTranslator(fingerPrintTransformer,
    //                                                                                                                           fingerPrintCommands);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual FingerPrintRenderer BuildUp(float sourceDpi,
                                               float destinationDpi,
                                               ViewRotation viewRotation = ViewRotation.Normal)
    {
      var svgUnitReader = this.CreateSvgUnitReader(sourceDpi);
      var fingerPrintTransformer = this.CreateFingerPrintTransformer(svgUnitReader);
      var viewMatrix = this.CreateViewMatrix(fingerPrintTransformer,
                                             sourceDpi,
                                             destinationDpi,
                                             viewRotation);
      var fingerPrintCommands = this.CreateFingerPrintCommands();
      var fingerPrintRenderer = this.CreateFingerPrintRenderer(viewMatrix,
                                                               fingerPrintCommands);
      //var svgLineTranslator = this.CreateSvgLineTranslator(fingerPrintTransformer,
      //                                                     fingerPrintCommands);
      var svgRectangleTranslator = this.CreateSvgRectangleTranslator(fingerPrintTransformer,
                                                                     fingerPrintCommands,
                                                                     svgUnitReader);
      //var svgTextTranslator = this.CreateSvgTextTranslator(fingerPrintTransformer,
      //                                                     fingerPrintCommands);
      //var svgTextSpanTranslator = this.CreateSvgTextSpanTranslator(fingerPrintTransformer,
      //                                                             fingerPrintCommands);
      //var svgPathTranslator = this.CreateSvgPathTranslator(fingerPrintTransformer,
      //                                                     fingerPrintCommands);
      //var svgImageTranslator = this.CreateSvgImageTranslator(fingerPrintTransformer,
      //                                                       fingerPrintCommands);

      //fingerPrintRenderer.RegisterTranslator(svgLineTranslator);
      fingerPrintRenderer.RegisterTranslator(svgRectangleTranslator);
      //fingerPrintRenderer.RegisterTranslator(svgTextTranslator);
      //fingerPrintRenderer.RegisterTranslator(svgTextSpanTranslator);
      //fingerPrintRenderer.RegisterTranslator(svgPathTranslator);
      //fingerPrintRenderer.RegisterTranslator(svgImageTranslator);

      return fingerPrintRenderer;
    }
  }
}