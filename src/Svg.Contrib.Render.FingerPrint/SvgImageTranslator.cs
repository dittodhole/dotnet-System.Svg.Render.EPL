using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class SvgImageTranslator : SvgImageTranslatorBase<FingerPrintContainer>
  {
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintTransformer"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintCommands"/> is <see langword="null" />.</exception>
    public SvgImageTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                              [NotNull] FingerPrintCommands fingerPrintCommands)
      : base(fingerPrintTransformer)
    {
      if (fingerPrintTransformer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintTransformer));
      }
      if (fingerPrintCommands == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintCommands));
      }
      this.FingerPrintTransformer = fingerPrintTransformer;
      this.FingerPrintCommands = fingerPrintCommands;
    }

    [NotNull]
    protected FingerPrintCommands FingerPrintCommands { get; }

    [NotNull]
    protected FingerPrintTransformer FingerPrintTransformer { get; }

    /// <exception cref="ArgumentNullException"><paramref name="variableName"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="bitmap"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container"/> is <see langword="null" />.</exception>
    protected override void StoreGraphics([NotNull] string variableName,
                                          [NotNull] Bitmap bitmap,
                                          [NotNull] FingerPrintContainer container)
    {
      if (variableName == null)
      {
        throw new ArgumentNullException(nameof(variableName));
      }
      if (bitmap == null)
      {
        throw new ArgumentNullException(nameof(bitmap));
      }
      if (container == null)
      {
        throw new ArgumentNullException(nameof(container));
      }

      var pcxByteArray = this.FingerPrintTransformer.ConvertToPcx(bitmap);

      container.Header.Add(this.FingerPrintCommands.RemoveImage(variableName));
      container.Header.Add(this.FingerPrintCommands.ImageLoad(variableName,
                                                              pcxByteArray.Length));
      container.Header.Add(pcxByteArray);
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgElement"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container"/> is <see langword="null" />.</exception>
    protected override void GraphicDirectWrite([NotNull] SvgImage svgElement,
                                               [NotNull] Matrix sourceMatrix,
                                               [NotNull] Matrix viewMatrix,
                                               float sourceAlignmentWidth,
                                               float sourceAlignmentHeight,
                                               int horizontalStart,
                                               int verticalStart,
                                               [NotNull] FingerPrintContainer container)
    {
      if (svgElement == null)
      {
        throw new ArgumentNullException(nameof(svgElement));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }
      if (container == null)
      {
        throw new ArgumentNullException(nameof(container));
      }

      throw new NotImplementedException();

      using (var bitmap = this.FingerPrintTransformer.ConvertToBitmap(svgElement,
                                                                      sourceMatrix,
                                                                      viewMatrix,
                                                                      (int) sourceAlignmentWidth,
                                                                      (int) sourceAlignmentHeight))
      {
        if (bitmap == null)
        {
          return;
        }

        int numberOfBytesPerRow;
        var rawBinaryData = this.FingerPrintTransformer.GetRawBinaryData(bitmap,
                                                                         false,
                                                                         out numberOfBytesPerRow);

        container.Body.Add(this.FingerPrintCommands.Position(horizontalStart,
                                                             verticalStart));
        container.Body.Add(this.FingerPrintCommands.Direction(Direction.Direction3));
        container.Body.Add(this.FingerPrintCommands.Align(Alignment.TopLeft));
        container.Body.Add(this.FingerPrintCommands.NormalImage());
        container.Body.Add(this.FingerPrintCommands.PrintBuffer(rawBinaryData.Count()));
        container.Body.Add(rawBinaryData);
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgImage"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="variableName"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container"/> is <see langword="null" />.</exception>
    protected override void PrintGraphics([NotNull] SvgImage svgImage,
                                          [NotNull] Matrix sourceMatrix,
                                          [NotNull] Matrix viewMatrix,
                                          int horizontalStart,
                                          int verticalStart,
                                          int sector,
                                          [NotNull] string variableName,
                                          [NotNull] FingerPrintContainer container)
    {
      if (svgImage == null)
      {
        throw new ArgumentNullException(nameof(svgImage));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }
      if (variableName == null)
      {
        throw new ArgumentNullException(nameof(variableName));
      }
      if (container == null)
      {
        throw new ArgumentNullException(nameof(container));
      }

      Direction direction;
      if (sector % 2 == 0)
      {
        direction = Direction.Direction4;
      }
      else
      {
        direction = Direction.Direction3;
      }

      container.Body.Add(this.FingerPrintCommands.Position(horizontalStart,
                                                           verticalStart));
      container.Body.Add(this.FingerPrintCommands.Direction(direction));
      container.Body.Add(this.FingerPrintCommands.Align(Alignment.TopLeft));
      container.Body.Add(this.FingerPrintCommands.NormalImage());
      container.Body.Add(this.FingerPrintCommands.PrintImage(variableName));
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgImage"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix"/> is <see langword="null" />.</exception>
    [Pure]
    protected override void GetPosition([NotNull] SvgImage svgImage,
                                        [NotNull] Matrix sourceMatrix,
                                        [NotNull] Matrix viewMatrix,
                                        out float sourceAlignmentWidth,
                                        out float sourceAlignmentHeight,
                                        out int horizontalStart,
                                        out int verticalStart,
                                        out int sector)
    {
      if (svgImage == null)
      {
        throw new ArgumentNullException(nameof(svgImage));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }

      base.GetPosition(svgImage,
                       sourceMatrix,
                       viewMatrix,
                       out sourceAlignmentWidth,
                       out sourceAlignmentHeight,
                       out horizontalStart,
                       out verticalStart,
                       out sector);

      if (sector % 2 > 0)
      {
        horizontalStart += (int) sourceAlignmentHeight;
      }
    }
  }
}