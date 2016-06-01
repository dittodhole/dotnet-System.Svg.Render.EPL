using System.Drawing.Drawing2D;
using System.Text;
using JetBrains.Annotations;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class FingerPrintRenderer : RendererBase<FingerPrintContainer>
  {
    public FingerPrintRenderer([NotNull] FingerPrintCommands fingerPrintCommands)
    {
      this.FingerPrintCommands = fingerPrintCommands;
    }

    [NotNull]
    protected FingerPrintCommands FingerPrintCommands { get; }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public override FingerPrintContainer GetTranslation([NotNull] SvgDocument svgDocument,
                                                        [NotNull] Matrix viewMatrix)
    {
      var sourceMatrix = new Matrix();
      var fingerPrintContainer = new FingerPrintContainer();
      this.AddBodyToTranslation(svgDocument,
                                sourceMatrix,
                                viewMatrix,
                                fingerPrintContainer);
      this.AddHeaderToTranslation(svgDocument,
                                  sourceMatrix,
                                  viewMatrix,
                                  fingerPrintContainer);
      this.AddFooterToTranslation(svgDocument,
                                  sourceMatrix,
                                  viewMatrix,
                                  fingerPrintContainer);

      return fingerPrintContainer;
    }

    protected virtual void AddHeaderToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix sourceMatrix,
                                                  [NotNull] Matrix viewMatrix,
                                                  [NotNull] FingerPrintContainer container) {}

    protected virtual void AddBodyToTranslation([NotNull] SvgDocument svgDocument,
                                                [NotNull] Matrix sourceMatrix,
                                                [NotNull] Matrix viewMatrix,
                                                [NotNull] FingerPrintContainer container)
    {
      container.Header.Add(this.FingerPrintCommands.ImmediateOn());
      container.Header.Add(this.FingerPrintCommands.SelectCharacterSet(CharacterSet.Utf8));
      container.Body.Add(this.FingerPrintCommands.VerbOff());
      this.TranslateSvgElementAndChildren(svgDocument,
                                          sourceMatrix,
                                          viewMatrix,
                                          container);
      container.Body.Add(this.FingerPrintCommands.InputOff());
    }

    protected virtual void AddFooterToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix sourceMatrix,
                                                  [NotNull] Matrix viewMatrix,
                                                  [NotNull] FingerPrintContainer container)
    {
      container.Footer.Add(this.FingerPrintCommands.PrintFeed());
      container.Footer.Add(string.Empty);
    }

    [Pure]
    [NotNull]
    [MustUseReturnValue]
    public virtual Encoding GetEncoding()
    {
      return Encoding.UTF8;
    }
  }
}