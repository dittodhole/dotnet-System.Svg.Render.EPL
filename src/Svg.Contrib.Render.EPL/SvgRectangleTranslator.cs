using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class SvgRectangleTranslator : SvgElementTranslatorBase<EplContainer, SvgRectangle>
  {
    /// <exception cref="ArgumentNullException"><paramref name="eplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplCommands" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitReader" /> is <see langword="null" />.</exception>
    public SvgRectangleTranslator([NotNull] EplTransformer eplTransformer,
                                  [NotNull] EplCommands eplCommands,
                                  [NotNull] SvgUnitReader svgUnitReader)
    {
      this.EplTransformer = eplTransformer ?? throw new ArgumentNullException(nameof(eplTransformer));
      this.EplCommands = eplCommands ?? throw new ArgumentNullException(nameof(eplCommands));
      this.SvgUnitReader = svgUnitReader ?? throw new ArgumentNullException(nameof(svgUnitReader));
    }

    [NotNull]
    private EplTransformer EplTransformer { get; }

    [NotNull]
    private EplCommands EplCommands { get; }

    [NotNull]
    private SvgUnitReader SvgUnitReader { get; }

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplContainer" /> is <see langword="null" />.</exception>
    public override void Translate(SvgRectangle svgRectangle,
                                   Matrix sourceMatrix,
                                   Matrix viewMatrix,
                                   EplContainer eplContainer)
    {
      if (svgRectangle == null)
      {
        throw new ArgumentNullException(nameof(svgRectangle));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }
      if (eplContainer == null)
      {
        throw new ArgumentNullException(nameof(eplContainer));
      }

      if (svgRectangle.Fill != SvgPaintServer.None
          && (svgRectangle.Fill as SvgColourServer)?.Colour != Color.White)
      {
        this.TranslateFilledBox(svgRectangle,
                                sourceMatrix,
                                viewMatrix,
                                eplContainer);
      }

      if (svgRectangle.Stroke != SvgPaintServer.None)
      {
        this.TranslateBox(svgRectangle,
                          sourceMatrix,
                          viewMatrix,
                          eplContainer);
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplContainer" /> is <see langword="null" />.</exception>
    protected virtual void TranslateFilledBox([NotNull] SvgRectangle svgRectangle,
                                              [NotNull] Matrix sourceMatrix,
                                              [NotNull] Matrix viewMatrix,
                                              [NotNull] EplContainer eplContainer)
    {
      if (svgRectangle == null)
      {
        throw new ArgumentNullException(nameof(svgRectangle));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }
      if (eplContainer == null)
      {
        throw new ArgumentNullException(nameof(eplContainer));
      }

      this.GetPosition(svgRectangle,
                       sourceMatrix,
                       viewMatrix,
                       out var horizontalStart,
                       out var verticalStart,
                       out var lineThickness,
                       out var horizontalEnd,
                       out var verticalEnd);

      var horizontalLength = horizontalEnd - horizontalStart;
      var verticalLength = verticalEnd - verticalStart;

      eplContainer.Body.Add(this.EplCommands.LineDrawBlack(horizontalStart,
                                                           verticalStart,
                                                           horizontalLength,
                                                           verticalLength));
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplContainer" /> is <see langword="null" />.</exception>
    protected virtual void TranslateBox([NotNull] SvgRectangle svgRectangle,
                                        [NotNull] Matrix sourceMatrix,
                                        [NotNull] Matrix viewMatrix,
                                        [NotNull] EplContainer eplContainer)
    {
      if (svgRectangle == null)
      {
        throw new ArgumentNullException(nameof(svgRectangle));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }
      if (eplContainer == null)
      {
        throw new ArgumentNullException(nameof(eplContainer));
      }

      this.GetPosition(svgRectangle,
                       sourceMatrix,
                       viewMatrix,
                       out var horizontalStart,
                       out var verticalStart,
                       out var lineThickness,
                       out var horizontalEnd,
                       out var verticalEnd);

      eplContainer.Body.Add(this.EplCommands.DrawBox(horizontalStart,
                                                     verticalStart,
                                                     lineThickness,
                                                     horizontalEnd,
                                                     verticalEnd));
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    [Pure]
    protected virtual void GetPosition([NotNull] SvgRectangle svgRectangle,
                                       [NotNull] Matrix sourceMatrix,
                                       [NotNull] Matrix viewMatrix,
                                       out int horizontalStart,
                                       out int verticalStart,
                                       out int lineThickness,
                                       out int horizontalEnd,
                                       out int verticalEnd)
    {
      if (svgRectangle == null)
      {
        throw new ArgumentNullException(nameof(svgRectangle));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }

      this.EplTransformer.Transform(svgRectangle,
                                    sourceMatrix,
                                    viewMatrix,
                                    out var startX,
                                    out var startY,
                                    out var endX,
                                    out var endY,
                                    out var strokeWidth);

      horizontalStart = (int) startX;
      verticalStart = (int) startY;
      lineThickness = (int) strokeWidth;
      horizontalEnd = (int) endX;
      verticalEnd = (int) endY;
    }
  }
}
