using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class FingerPrintRenderer : RendererBase<FingerPrintContainer>
  {
    public FingerPrintRenderer([NotNull] Matrix viewMatrix,
                               [NotNull] FingerPrintCommands fingerPrintCommands)
    {
      this.ViewMatrix = viewMatrix;
      this.FingerPrintCommands = fingerPrintCommands;
    }

    [NotNull]
    protected FingerPrintCommands FingerPrintCommands { get; }

    [NotNull]
    protected Matrix ViewMatrix { get; }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual Matrix CreateParentMatrix() => new Matrix();

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public override FingerPrintContainer GetTranslation([NotNull] SvgDocument svgDocument)
    {
      var parentMatrix = this.CreateParentMatrix();
      var fingerPrintContainer = new FingerPrintContainer();
      this.AddBodyToTranslation(svgDocument,
                                parentMatrix,
                                fingerPrintContainer);
      this.AddHeaderToTranslation(svgDocument,
                                  parentMatrix,
                                  fingerPrintContainer);
      this.AddFooterToTranslation(svgDocument,
                                  parentMatrix,
                                  fingerPrintContainer);

      return fingerPrintContainer;
    }

    protected virtual void AddHeaderToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix parentMatrix,
                                                  [NotNull] FingerPrintContainer container) {}

    protected virtual void AddBodyToTranslation([NotNull] SvgDocument svgDocument,
                                                [NotNull] Matrix parentMatrix,
                                                [NotNull] FingerPrintContainer container)
    {
      container.Body.Add(this.FingerPrintCommands.Align(Alignment.BaseLineLeft));
      this.TranslateSvgElementAndChildren(svgDocument,
                                          parentMatrix,
                                          this.ViewMatrix,
                                          container);
    }

    protected virtual void AddFooterToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix parentMatrix,
                                                  [NotNull] FingerPrintContainer container)
    {
      container.Footer.Add(this.FingerPrintCommands.PrintFeed());
    }
  }
}