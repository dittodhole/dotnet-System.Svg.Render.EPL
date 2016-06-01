using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable VirtualMemberNeverOverriden.Global

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public class DefaultBootstrapper
  {
    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual SvgUnitReader CreateSvgUnitReader() => new SvgUnitReader();

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual ZplTransformer CreateZplTransformer()
    {
      var svgUnitReader = this.CreateSvgUnitReader();
      var zplTransformer = this.CreateZplTransformer(svgUnitReader);

      return zplTransformer;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual ZplTransformer CreateZplTransformer([NotNull] SvgUnitReader svgUnitReader) => new ZplTransformer(svgUnitReader);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual Matrix CreateViewMatrix(float sourceDpi,
                                           float destinationDpi,
                                           ViewRotation viewRotation = ViewRotation.Normal)
    {
      var magnificationFactor = destinationDpi / sourceDpi;

      var zplTransformer = this.CreateZplTransformer();
      var viewMatrix = zplTransformer.CreateViewMatrix(magnificationFactor,
                                                       viewRotation);

      return viewMatrix;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual ZplRenderer CreateZplRenderer([NotNull] ZplCommands zplCommands,
                                                    CharacterSet characterSet = CharacterSet.ZebraCodePage850) => new ZplRenderer(zplCommands,
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

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual SvgImageTranslator CreateSvgImageTranslator([NotNull] ZplTransformer zplTransformer,
                                                                  [NotNull] ZplCommands zplCommands) => new SvgImageTranslator(zplTransformer,
                                                                                                                               zplCommands);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual ZplRenderer CreateZplRenderer([NotNull] ZplTransformer zplTransformer,
                                                 CharacterSet characterSet = CharacterSet.ZebraCodePage850)
    {
      var svgUnitReader = this.CreateSvgUnitReader();
      var zplCommands = this.CreateZplCommands();
      var zplRenderer = this.CreateZplRenderer(zplCommands,
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
      var svgImageTranslator = this.CreateSvgImageTranslator(zplTransformer,
                                                             zplCommands);

      zplRenderer.RegisterTranslator(svgLineTranslator);
      zplRenderer.RegisterTranslator(svgRectangleTranslator);
      zplRenderer.RegisterTranslator(svgTextTranslator);
      zplRenderer.RegisterTranslator(svgTextSpanTranslator);
      zplRenderer.RegisterTranslator(svgPathTranslator);
      zplRenderer.RegisterTranslator(svgImageTranslator);

      return zplRenderer;
    }
  }
}