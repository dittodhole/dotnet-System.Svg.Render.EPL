using System;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;
using Svg.Pathing;

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public class SvgPathTranslator : SvgElementTranslatorBase<ZplContainer, SvgPath>
  {
    /// <exception cref="ArgumentNullException"><paramref name="zplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplCommands" /> is <see langword="null" />.</exception>
    public SvgPathTranslator([NotNull] ZplTransformer zplTransformer,
                             [NotNull] ZplCommands zplCommands)
    {
      this.ZplTransformer = zplTransformer ?? throw new ArgumentNullException(nameof(zplTransformer));
      this.ZplCommands = zplCommands ?? throw new ArgumentNullException(nameof(zplCommands));
    }

    [NotNull]
    private ZplTransformer ZplTransformer { get; }

    [NotNull]
    private ZplCommands ZplCommands { get; }

    /// <exception cref="ArgumentNullException"><paramref name="svgPath" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplContainer" /> is <see langword="null" />.</exception>
    public override void Translate(SvgPath svgPath,
                                   Matrix sourceMatrix,
                                   Matrix viewMatrix,
                                   ZplContainer zplContainer)
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
      if (zplContainer == null)
      {
        throw new ArgumentNullException(nameof(zplContainer));
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
                                     zplContainer);
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgPath" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="svgLineSegment" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplContainer" /> is <see langword="null" />.</exception>
    protected virtual void TranslateSvgLineSegment([NotNull] SvgPath svgPath,
                                                   [NotNull] SvgLineSegment svgLineSegment,
                                                   [NotNull] Matrix sourceMatrix,
                                                   [NotNull] Matrix viewMatrix,
                                                   [NotNull] ZplContainer zplContainer)
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
      if (zplContainer == null)
      {
        throw new ArgumentNullException(nameof(zplContainer));
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

      this.ZplTransformer.Transform(svgLine,
                                    sourceMatrix,
                                    viewMatrix,
                                    out var startX,
                                    out var startY,
                                    out var endX,
                                    out var endY,
                                    out var strokeWidth);

      var horizontalStart = (int) startX;
      var verticalStart = (int) endY;
      var width = (int) (endX - startX);
      var height = (int) (endY - startY);
      var thickness = (int) strokeWidth;

      zplContainer.Body.Add(this.ZplCommands.FieldTypeset(horizontalStart,
                                                          verticalStart));
      zplContainer.Body.Add(this.ZplCommands.GraphicBox(width,
                                                        height,
                                                        thickness,
                                                        LineColor.Black));
    }
  }
}
