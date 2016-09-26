using System;
using System.Drawing.Drawing2D;
using System.Text;
using JetBrains.Annotations;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class FingerPrintRenderer : RendererBase<FingerPrintContainer>
  {
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintCommands" /> is <see langword="null" />.</exception>
    public FingerPrintRenderer([NotNull] FingerPrintCommands fingerPrintCommands)
    {
      if (fingerPrintCommands == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintCommands));
      }
      this.FingerPrintCommands = fingerPrintCommands;
    }

    [NotNull]
    protected FingerPrintCommands FingerPrintCommands { get; }

    /// <exception cref="ArgumentNullException"><paramref name="svgDocument" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public override FingerPrintContainer GetTranslation([NotNull] SvgDocument svgDocument,
                                                        [NotNull] Matrix viewMatrix)
    {
      if (svgDocument == null)
      {
        throw new ArgumentNullException(nameof(svgDocument));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }

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

    /// <exception cref="ArgumentNullException"><paramref name="svgDocument" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintContainer" /> is <see langword="null" />.</exception>
    protected virtual void AddHeaderToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix sourceMatrix,
                                                  [NotNull] Matrix viewMatrix,
                                                  [NotNull] FingerPrintContainer fingerPrintContainer)
    {
      if (svgDocument == null)
      {
        throw new ArgumentNullException(nameof(svgDocument));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }
      if (fingerPrintContainer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintContainer));
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgDocument" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintContainer" /> is <see langword="null" />.</exception>
    protected virtual void AddBodyToTranslation([NotNull] SvgDocument svgDocument,
                                                [NotNull] Matrix sourceMatrix,
                                                [NotNull] Matrix viewMatrix,
                                                [NotNull] FingerPrintContainer fingerPrintContainer)
    {
      if (svgDocument == null)
      {
        throw new ArgumentNullException(nameof(svgDocument));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }
      if (fingerPrintContainer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintContainer));
      }

      fingerPrintContainer.Header.Add(this.FingerPrintCommands.ImmediateOn());
      fingerPrintContainer.Header.Add(this.FingerPrintCommands.SelectCharacterSet(CharacterSet.Utf8));
      fingerPrintContainer.Body.Add(this.FingerPrintCommands.VerbOff());
      this.TranslateSvgElementAndChildren(svgDocument,
                                          sourceMatrix,
                                          viewMatrix,
                                          fingerPrintContainer);
      fingerPrintContainer.Body.Add(this.FingerPrintCommands.InputOff());
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgDocument" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintContainer" /> is <see langword="null" />.</exception>
    protected virtual void AddFooterToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix sourceMatrix,
                                                  [NotNull] Matrix viewMatrix,
                                                  [NotNull] FingerPrintContainer fingerPrintContainer)
    {
      if (svgDocument == null)
      {
        throw new ArgumentNullException(nameof(svgDocument));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }
      if (fingerPrintContainer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintContainer));
      }

      fingerPrintContainer.Footer.Add(this.FingerPrintCommands.PrintFeed());
      fingerPrintContainer.Footer.Add(string.Empty);
    }

    [Pure]
    [NotNull]
    public virtual Encoding GetEncoding()
    {
      return Encoding.UTF8;
    }
  }
}
