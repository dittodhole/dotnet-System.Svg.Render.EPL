using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Svg.Pathing;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgPathTranslator : SvgElementTranslatorBase<SvgPath>
  {
    public SvgPathTranslator([NotNull] Transformer transformer,
                             [NotNull] EplCommands eplCommands)
    {
      this.Transformer = transformer;
      this.EplCommands = eplCommands;
    }

    [NotNull]
    private Transformer Transformer { get; }

    [NotNull]
    private EplCommands EplCommands { get; }

    [NotNull]
    public override IEnumerable<byte> Translate([NotNull] SvgPath instance,
                                                [NotNull] Matrix matrix)
    {
      // TODO translate C (curveto)
      // TODO translate S (smooth curveto)
      // TODO translate Q (quadratic bézier curve)
      // TODO translate T (smooth bézier curve)
      // TODO translate A (elliptical arc)
      // TODO translate Z (closepath)
      // TODO add test cases

      var result = instance.PathData.OfType<SvgLineSegment>()
                           .SelectMany(svgLineSegment => this.TranslateSvgLineSegment(instance,
                                                                                      svgLineSegment,
                                                                                      matrix));

      return result;
    }

    [NotNull]
    private IEnumerable<byte> TranslateSvgLineSegment([NotNull] SvgPath instance,
                                                      [NotNull] SvgLineSegment svgLineSegment,
                                                      [NotNull] Matrix matrix)
    {
      var svgLine = new SvgLine
                    {
                      Color = instance.Color,
                      Stroke = instance.Stroke,
                      StrokeWidth = instance.StrokeWidth,
                      StartX = svgLineSegment.Start.X,
                      StartY = svgLineSegment.Start.Y,
                      EndX = svgLineSegment.End.X,
                      EndY = svgLineSegment.End.Y
                    };

      float startX;
      float startY;
      float endX;
      float endY;
      float strokeWidth;

      this.Transformer.Transform(svgLine,
                                 matrix,
                                 out startX,
                                 out startY,
                                 out endX,
                                 out endY,
                                 out strokeWidth);

      var horizontalStart = (int) startX;
      var verticalStart = (int) startY;
      var horizontalLength = (int) (endX - startX);
      var verticalLength = (int) (endY - startY);

      var result = this.EplCommands.LineDrawBlack(horizontalStart,
                                                  verticalStart,
                                                  horizontalLength,
                                                  verticalLength);

      return result;
    }
  }
}