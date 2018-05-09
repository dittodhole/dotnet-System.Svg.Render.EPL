using System;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;
using Svg.Pathing;

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class SvgPathTranslator : SvgElementTranslatorBase<EplContainer, SvgPath>
  {
    /// <exception cref="ArgumentNullException"><paramref name="eplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplCommands" /> is <see langword="null" />.</exception>
    public SvgPathTranslator([NotNull] EplTransformer eplTransformer,
                             [NotNull] EplCommands eplCommands)
    {
      this.EplTransformer = eplTransformer ?? throw new ArgumentNullException(nameof(eplTransformer));
      this.EplCommands = eplCommands ?? throw new ArgumentNullException(nameof(eplCommands));
    }

    [NotNull]
    private EplTransformer EplTransformer { get; }

    [NotNull]
    private EplCommands EplCommands { get; }

    /// <exception cref="ArgumentNullException"><paramref name="svgPath" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplContainer" /> is <see langword="null" />.</exception>
    public override void Translate(SvgPath svgPath,
                                   Matrix sourceMatrix,
                                   Matrix viewMatrix,
                                   EplContainer eplContainer)
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
      if (eplContainer == null)
      {
        throw new ArgumentNullException(nameof(eplContainer));
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
                                     eplContainer);
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgPath" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="svgLineSegment" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplContainer" /> is <see langword="null" />.</exception>
    protected virtual void TranslateSvgLineSegment([NotNull] SvgPath svgPath,
                                                   [NotNull] SvgLineSegment svgLineSegment,
                                                   [NotNull] Matrix sourceMatrix,
                                                   [NotNull] Matrix viewMatrix,
                                                   [NotNull] EplContainer eplContainer)
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
      if (eplContainer == null)
      {
        throw new ArgumentNullException(nameof(eplContainer));
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

      this.EplTransformer.Transform(svgLine,
                                    sourceMatrix,
                                    viewMatrix,
                                    out var startX,
                                    out var startY,
                                    out var endX,
                                    out var endY,
                                    out var strokeWidth);

      var horizontalStart = (int) startX;
      var verticalStart = (int) startY;
      var horizontalLength = (int) (endX - startX);
      if (horizontalLength == 0)
      {
        horizontalLength = (int) strokeWidth;
      }

      var verticalLength = (int) (endY - startY);
      if (verticalLength == 0)
      {
        verticalLength = (int) strokeWidth;
      }

      eplContainer.Body.Add(this.EplCommands.LineDrawBlack(horizontalStart,
                                                           verticalStart,
                                                           horizontalLength,
                                                           verticalLength));
    }
  }
}
