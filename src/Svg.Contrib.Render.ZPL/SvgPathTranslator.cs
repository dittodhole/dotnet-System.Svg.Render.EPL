using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;
using Svg.Pathing;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public class SvgPathTranslator : SvgElementTranslatorBase<ZplContainer, SvgPath>
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
                                   [NotNull] ZplContainer container)
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
        this.TranslateSvgLineSegment(svgElement,
                                     svgLineSegment,
                                     matrix,
                                     container);
      }
    }

    protected virtual void TranslateSvgLineSegment([NotNull] SvgPath instance,
                                                   [NotNull] SvgLineSegment svgLineSegment,
                                                   [NotNull] Matrix matrix,
                                                   [NotNull] ZplContainer container)
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
      var verticalStart = (int) endY;
      var width = (int) (endX - startX);
      var height = (int) (endY - startY);
      var thickness = (int) strokeWidth;

      container.Body.Add(this.ZplCommands.FieldTypeset(horizontalStart,
                                                       verticalStart));
      container.Body.Add(this.ZplCommands.GraphicBox(width,
                                                     height,
                                                     thickness,
                                                     LineColor.Black));
    }
  }
}