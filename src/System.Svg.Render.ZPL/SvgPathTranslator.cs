using System.Drawing.Drawing2D;
using System.Linq;
using System.Svg.Pathing;
using JetBrains.Annotations;

namespace System.Svg.Render.ZPL
{
  [PublicAPI]
  public class SvgPathTranslator : SvgElementTranslatorBase<SvgPath>
  {
    public SvgPathTranslator([NotNull] ZplTransformer zplTransformer,
                             [NotNull] ZplCommands zplCommands)
    {
      this.ZplTransformer = zplTransformer;
      this.ZplCommands = zplCommands;
    }

    [NotNull]
    protected ZplTransformer ZplTransformer { get; }

    [NotNull]
    protected ZplCommands ZplCommands { get; }

    public override void Translate([NotNull] SvgPath svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] ZplStream container)
    {
      // TODO translate C (curveto)
      // TODO translate S (smooth curveto)
      // TODO translate Q (quadratic bézier curve)
      // TODO translate T (smooth bézier curve)
      // TODO translate A (elliptical arc)
      // TODO translate Z (closepath)
      // TODO add test cases

      if (svgElement.PathData == null)
      {
        return;
      }

      // ReSharper disable ExceptionNotDocumentedOptional
      foreach (var svgLineSegment in svgElement.PathData.OfType<SvgLineSegment>())
      // ReSharper restore ExceptionNotDocumentedOptional
      {
        var eplStream = this.TranslateSvgLineSegment(svgElement,
                                                     svgLineSegment,
                                                     matrix);
        if (eplStream.Any())
        {
          container.Add(eplStream);
        }
      }
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual ZplStream TranslateSvgLineSegment([NotNull] SvgPath instance,
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
      this.ZplTransformer.Transform(svgLine,
                                    matrix,
                                    out startX,
                                    out startY,
                                    out endX,
                                    out endY,
                                    out strokeWidth);

      var horizontalStart = (int) startX;
      var verticalStart = (int) startY;
      var width = (int) Math.Abs(endX - startX);
      var height = (int) Math.Abs(endY - startY);
      var thickness = (int) strokeWidth;

      var zplStream = this.ZplCommands.GraphicBox(horizontalStart,
                                                  verticalStart,
                                                  width,
                                                  height,
                                                  thickness,
                                                  LineColor.Black);
      return zplStream;
    }
  }
}