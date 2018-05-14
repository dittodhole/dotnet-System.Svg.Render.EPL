using System;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;
using Svg.Pathing;

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class SvgPathTranslator : SvgElementTranslatorBase<FingerPrintContainer, SvgPath>
  {
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintCommands" /> is <see langword="null" />.</exception>
    public SvgPathTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                             [NotNull] FingerPrintCommands fingerPrintCommands)
    {
      this.FingerPrintTransformer = fingerPrintTransformer ?? throw new ArgumentNullException(nameof(fingerPrintTransformer));
      this.FingerPrintCommands = fingerPrintCommands ?? throw new ArgumentNullException(nameof(fingerPrintCommands));
    }

    [NotNull]
    private FingerPrintTransformer FingerPrintTransformer { get; }

    [NotNull]
    private FingerPrintCommands FingerPrintCommands { get; }

    /// <exception cref="ArgumentNullException"><paramref name="svgPath" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintContainer" /> is <see langword="null" />.</exception>
    public override void Translate(SvgPath svgPath,
                                   Matrix sourceMatrix,
                                   Matrix viewMatrix,
                                   FingerPrintContainer fingerPrintContainer)
    {
      if (svgPath == null)
      {
        throw new ArgumentNullException(nameof(svgPath));
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

      // TODO translate C (curveto)
      // TODO translate S (smooth curveto)
      // TODO translate Q (quadratic bézier curve)
      // TODO translate T (smooth bézier curve)
      // TODO translate A (elliptical arc)
      // TODO translate Z (closepath)
      // TODO add test cases

      if (svgPath.PathData == null)
      {
        return;
      }

      foreach (var svgLineSegment in svgPath.PathData.OfType<SvgLineSegment>())
      {
        this.TranslateSvgLineSegment(svgPath,
                                     svgLineSegment,
                                     sourceMatrix,
                                     viewMatrix,
                                     fingerPrintContainer);
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgPath" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="svgLineSegment" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintContainer" /> is <see langword="null" />.</exception>
    protected virtual void TranslateSvgLineSegment([NotNull] SvgPath svgPath,
                                                   [NotNull] SvgLineSegment svgLineSegment,
                                                   [NotNull] Matrix sourceMatrix,
                                                   [NotNull] Matrix viewMatrix,
                                                   [NotNull] FingerPrintContainer fingerPrintContainer)
    {
      if (svgPath == null)
      {
        throw new ArgumentNullException(nameof(svgPath));
      }
      if (svgLineSegment == null)
      {
        throw new ArgumentNullException(nameof(svgLineSegment));
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

      var svgLine = new SvgLine
                    {
                      Color = svgPath.Color,
                      Stroke = svgPath.Stroke,
                      StrokeWidth = svgPath.StrokeWidth,
                      StartX = svgLineSegment.Start.X,
                      StartY = svgLineSegment.Start.Y,
                      EndX = svgLineSegment.End.X,
                      EndY = svgLineSegment.End.Y
                    };

      this.FingerPrintTransformer.Transform(svgLine,
                                            sourceMatrix,
                                            viewMatrix,
                                            out var startX,
                                            out var startY,
                                            out var endX,
                                            out var endY,
                                            out var strokeWidth);

      var horizontalStart = (int) startX;
      var verticalStart = (int) startY;
      var length = (int) (endX - startX);
      if (length == 0)
      {
        length = (int) strokeWidth;
      }

      var lineWeight = (int) (endY - startY);
      if (lineWeight == 0)
      {
        lineWeight = (int) strokeWidth;
      }

      fingerPrintContainer.Body.Add(this.FingerPrintCommands.Position(horizontalStart,
                                                                      verticalStart));
      fingerPrintContainer.Body.Add(this.FingerPrintCommands.Direction(Direction.LeftToRight));
      fingerPrintContainer.Body.Add(this.FingerPrintCommands.Align(Alignment.TopLeft));
      fingerPrintContainer.Body.Add(this.FingerPrintCommands.Line(length,
                                                                  lineWeight));
    }
  }
}
