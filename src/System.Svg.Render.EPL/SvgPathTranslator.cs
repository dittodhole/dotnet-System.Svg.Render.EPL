using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Svg.Pathing;
using System.Text;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgPathTranslator : SvgElementTranslatorWithEncoding<SvgPath>
  {
    public SvgPathTranslator([NotNull] SvgLineTranslator svgLineTranslator,
                             [NotNull] Encoding encoding)
      : base(encoding)
    {
      this.SvgLineTranslator = svgLineTranslator;
    }

    [NotNull]
    private SvgLineTranslator SvgLineTranslator { get; }

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
                           .Select(svgLineSegment => new SvgLine
                                                     {
                                                       Color = instance.Color,
                                                       Stroke = instance.Stroke,
                                                       StrokeWidth = instance.StrokeWidth,
                                                       StartX = new SvgUnit(svgLineSegment.Start.X),
                                                       StartY = new SvgUnit(svgLineSegment.Start.Y),
                                                       EndX = new SvgUnit(svgLineSegment.End.X),
                                                       EndY = new SvgUnit(svgLineSegment.End.Y)
                                                     })
                           .SelectMany(svgLine => this.SvgLineTranslator.Translate(svgLine,
                                                                                   matrix));

      return result;
    }
  }
}