using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public class SvgImageTranslator : SvgImageTranslatorBase<ZplContainer>
  {
    /// <exception cref="ArgumentNullException"><paramref name="zplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplCommands" /> is <see langword="null" />.</exception>
    public SvgImageTranslator([NotNull] ZplTransformer zplTransformer,
                              [NotNull] ZplCommands zplCommands)
      : base(zplTransformer)
    {
      this.ZplTransformer = zplTransformer ?? throw new ArgumentNullException(nameof(zplTransformer));
      this.ZplCommands = zplCommands ?? throw new ArgumentNullException(nameof(zplCommands));
    }

    [NotNull]
    private ZplTransformer ZplTransformer { get; }

    [NotNull]
    private ZplCommands ZplCommands { get; }

    /// <exception cref="ArgumentNullException"><paramref name="svgImage" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="variableName" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="bitmap" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplContainer" /> is <see langword="null" />.</exception>
    protected override void StoreGraphics(SvgImage svgImage,
                                          string variableName,
                                          Bitmap bitmap,
                                          ZplContainer zplContainer)
    {
      if (svgImage == null)
      {
        throw new ArgumentNullException(nameof(svgImage));
      }
      if (variableName == null)
      {
        throw new ArgumentNullException(nameof(variableName));
      }
      if (bitmap == null)
      {
        throw new ArgumentNullException(nameof(bitmap));
      }
      if (zplContainer == null)
      {
        throw new ArgumentNullException(nameof(zplContainer));
      }

      var rawBinaryData = this.ZplTransformer.GetRawBinaryData(bitmap,
                                                               false,
                                                               out var numberOfBytesPerRow);

      zplContainer.Header.Add(this.ZplCommands.DownloadGraphics(variableName,
                                                                rawBinaryData,
                                                                numberOfBytesPerRow));
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgImage" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplContainer" /> is <see langword="null" />.</exception>
    protected override void GraphicDirectWrite(SvgImage svgImage,
                                               Matrix sourceMatrix,
                                               Matrix viewMatrix,
                                               float sourceAlignmentWidth,
                                               float sourceAlignmentHeight,
                                               int horizontalStart,
                                               int verticalStart,
                                               ZplContainer zplContainer)
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
      if (zplContainer == null)
      {
        throw new ArgumentNullException(nameof(zplContainer));
      }

      using (var bitmap = this.ZplTransformer.ConvertToBitmap(svgImage,
                                                              sourceMatrix,
                                                              viewMatrix,
                                                              (int) sourceAlignmentWidth,
                                                              (int) sourceAlignmentHeight))
      {
        if (bitmap == null)
        {
          return;
        }

        var rawBinaryData = this.ZplTransformer.GetRawBinaryData(bitmap,
                                                                 false,
                                                                 out var numberOfBytesPerRow);

        zplContainer.Body.Add(this.ZplCommands.FieldTypeset(horizontalStart,
                                                            verticalStart));
        zplContainer.Body.Add(this.ZplCommands.GraphicField(rawBinaryData,
                                                            numberOfBytesPerRow));
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgImage" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="variableName" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplContainer" /> is <see langword="null" />.</exception>
    protected override void PrintGraphics(SvgImage svgImage,
                                          Matrix sourceMatrix,
                                          Matrix viewMatrix,
                                          int horizontalStart,
                                          int verticalStart,
                                          int sector,
                                          string variableName,
                                          ZplContainer zplContainer)
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
      if (zplContainer == null)
      {
        throw new ArgumentNullException(nameof(zplContainer));
      }

      zplContainer.Body.Add(this.ZplCommands.FieldTypeset(horizontalStart,
                                                          verticalStart));
      zplContainer.Body.Add(this.ZplCommands.RecallGraphic(variableName));
    }
  }
}
