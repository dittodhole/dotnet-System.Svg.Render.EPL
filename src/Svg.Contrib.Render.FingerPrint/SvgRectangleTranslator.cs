using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class SvgRectangleTranslator : SvgElementTranslatorBase<FingerPrintContainer, SvgRectangle>
  {
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintCommands" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitReader" /> is <see langword="null" />.</exception>
    public SvgRectangleTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                  [NotNull] FingerPrintCommands fingerPrintCommands,
                                  [NotNull] SvgUnitReader svgUnitReader)
    {
      this.FingerPrintTransformer = fingerPrintTransformer ?? throw new ArgumentNullException(nameof(fingerPrintTransformer));
      this.FingerPrintCommands = fingerPrintCommands ?? throw new ArgumentNullException(nameof(fingerPrintCommands));
      this.SvgUnitReader = svgUnitReader ?? throw new ArgumentNullException(nameof(svgUnitReader));
    }

    [NotNull]
    private SvgUnitReader SvgUnitReader { get; }

    [NotNull]
    private FingerPrintCommands FingerPrintCommands { get; }

    [NotNull]
    private FingerPrintTransformer FingerPrintTransformer { get; }

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintContainer" /> is <see langword="null" />.</exception>
    public override void Translate(SvgRectangle svgRectangle,
                                   Matrix sourceMatrix,
                                   Matrix viewMatrix,
                                   FingerPrintContainer fingerPrintContainer)
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
      if (fingerPrintContainer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintContainer));
      }

      if (svgRectangle.Fill != SvgPaintServer.None
          && (svgRectangle.Fill as SvgColourServer)?.Colour != Color.White)
      {
        this.TranslateFilledBox(svgRectangle,
                                sourceMatrix,
                                viewMatrix,
                                fingerPrintContainer);
      }
      else if (svgRectangle.Stroke != SvgPaintServer.None)
      {
        this.TranslateBox(svgRectangle,
                          sourceMatrix,
                          viewMatrix,
                          fingerPrintContainer);
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintContainer" /> is <see langword="null" />.</exception>
    protected virtual void TranslateFilledBox([NotNull] SvgRectangle svgRectangle,
                                              [NotNull] Matrix sourceMatrix,
                                              [NotNull] Matrix viewMatrix,
                                              [NotNull] FingerPrintContainer fingerPrintContainer)
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
      if (fingerPrintContainer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintContainer));
      }

      this.GetPosition(svgRectangle,
                       sourceMatrix,
                       viewMatrix,
                       out var horizontalStart,
                       out var verticalStart,
                       out var lineThickness,
                       out var horizontalEnd,
                       out var verticalEnd);

      var length = horizontalEnd - horizontalStart;
      var lineWeight = verticalEnd - verticalStart;

      fingerPrintContainer.Body.Add(this.FingerPrintCommands.Position(horizontalStart,
                                                                      verticalStart));
      fingerPrintContainer.Body.Add(this.FingerPrintCommands.Direction(Direction.LeftToRight));
      fingerPrintContainer.Body.Add(this.FingerPrintCommands.Align(Alignment.TopLeft));
      fingerPrintContainer.Body.Add(this.FingerPrintCommands.Line(length,
                                                                  lineWeight));
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintContainer" /> is <see langword="null" />.</exception>
    protected virtual void TranslateBox([NotNull] SvgRectangle svgRectangle,
                                        [NotNull] Matrix sourceMatrix,
                                        [NotNull] Matrix viewMatrix,
                                        [NotNull] FingerPrintContainer fingerPrintContainer)
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
      if (fingerPrintContainer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintContainer));
      }

      this.GetPosition(svgRectangle,
                       sourceMatrix,
                       viewMatrix,
                       out var horizontalStart,
                       out var verticalStart,
                       out var lineWeight,
                       out var horizontalEnd,
                       out var verticalEnd);

      var width = horizontalEnd - horizontalStart;
      var height = verticalEnd - verticalStart;

      fingerPrintContainer.Body.Add(this.FingerPrintCommands.Position(horizontalStart,
                                                                      verticalStart));
      fingerPrintContainer.Body.Add(this.FingerPrintCommands.Direction(Direction.LeftToRight));
      fingerPrintContainer.Body.Add(this.FingerPrintCommands.Align(Alignment.TopLeft));
      fingerPrintContainer.Body.Add(this.FingerPrintCommands.Box(width,
                                                                 height,
                                                                 lineWeight));
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

      this.FingerPrintTransformer.Transform(svgRectangle,
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
