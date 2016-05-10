using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Svg.Pathing;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgPathTranslator : SvgElementTranslatorBase<SvgPath>

  {
    public SvgPathTranslator([NotNull] ISvgUnitCalculator svgUnitCalculator,
                             [NotNull] SvgLineTranslator svgLineTranslator)
      : base(svgUnitCalculator)
    {
      this.SvgLineTranslator = svgLineTranslator;
    }

    [NotNull]
    private SvgLineTranslator SvgLineTranslator { get; }

    public override bool TryTranslate([NotNull] SvgPath instance,
                                      [NotNull] Matrix matrix,
                                      int targetDpi,
                                      out object translation)
    {
      // TODO translate C (curveto)
      // TODO translate S (smooth curveto)
      // TODO translate Q (quadratic bézier curve)
      // TODO translate T (smooth bézier curve)
      // TODO translate A (elliptical arc)
      // TODO translate Z (closepath)
      // TODO add test cases

      ICollection<object> translations = new LinkedList<object>();

      foreach (var svgLineSegment in instance.PathData.OfType<SvgLineSegment>())
      {
        var svgLine = new SvgLine
                      {
                        Color = instance.Color,
                        Stroke = instance.Stroke,
                        StrokeWidth = instance.StrokeWidth,
                        StartX = new SvgUnit(svgLineSegment.Start.X),
                        StartY = new SvgUnit(svgLineSegment.Start.Y),
                        EndX = new SvgUnit(svgLineSegment.End.X),
                        EndY = new SvgUnit(svgLineSegment.End.Y)
                      };
        if (!this.SvgLineTranslator.TryTranslate(svgLine,
                                                 matrix,
                                                 targetDpi,
                                                 out translation))
        {
          return false;
        }

        translations.Add(translation);
      }

      if (translations.Any())
      {
        translation = string.Join(Environment.NewLine,
                                  translations);
      }
      else
      {
        translation = null;
      }

      return true;
    }
  }
}