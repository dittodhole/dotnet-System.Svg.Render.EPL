using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
// ReSharper disable VirtualMemberNeverOverriden.Global

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class SvgImageTranslator : SvgImageTranslatorBase<EplContainer>
  {
    /// <exception cref="ArgumentNullException"><paramref name="eplTransformer"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplCommands"/> is <see langword="null" />.</exception>
    public SvgImageTranslator([NotNull] EplTransformer eplTransformer,
                              [NotNull] EplCommands eplCommands)
      : base(eplTransformer)
    {
      if (eplTransformer == null)
      {
        throw new ArgumentNullException(nameof(eplTransformer));
      }
      if (eplCommands == null)
      {
        throw new ArgumentNullException(nameof(eplCommands));
      }
      this.EplTransformer = eplTransformer;
      this.EplCommands = eplCommands;
    }

    [NotNull]
    protected EplTransformer EplTransformer { get; }

    [NotNull]
    protected EplCommands EplCommands { get; }

    /// <exception cref="ArgumentNullException"><paramref name="variableName"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="bitmap"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container"/> is <see langword="null" />.</exception>
    protected override void StoreGraphics([NotNull] string variableName,
                                          [NotNull] Bitmap bitmap,
                                          [NotNull] EplContainer container)
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

      var pcxByteArray = this.EplTransformer.ConvertToPcx(bitmap);

      container.Header.Add(this.EplCommands.DeleteGraphics(variableName));
      container.Header.Add(this.EplCommands.DeleteGraphics(variableName));
      container.Header.Add(this.EplCommands.StoreGraphics(variableName,
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
                                               [NotNull] EplContainer container)
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

      using (var bitmap = this.EplTransformer.ConvertToBitmap(svgElement,
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
        var rawBinaryData = this.EplTransformer.GetRawBinaryData(bitmap,
                                                                 true,
                                                                 out numberOfBytesPerRow);
        var rows = bitmap.Height;

        container.Body.Add(this.EplCommands.GraphicDirectWrite(horizontalStart,
                                                               verticalStart,
                                                               numberOfBytesPerRow,
                                                               rows));
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
                                          [NotNull] EplContainer container)
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

      container.Body.Add(this.EplCommands.PrintGraphics(horizontalStart,
                                                        verticalStart,
                                                        variableName));
    }
  }
}