using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;
using Svg.Pathing;

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class SvgPathTranslator : SvgElementTranslatorBase<SvgPath>
  {
    public SvgPathTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                             [NotNull] FingerPrintCommands fingerPrintCommands)
    {
      this.FingerPrintTransformer = fingerPrintTransformer;
      this.FingerPrintCommands = fingerPrintCommands;
    }

    [NotNull]
    protected FingerPrintTransformer FingerPrintTransformer { get; }

    [NotNull]
    protected FingerPrintCommands FingerPrintCommands { get; }

    public override void Translate([NotNull] SvgPath svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] FingerPrintContainer container)
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
                                                   [NotNull] FingerPrintContainer container)
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
      this.FingerPrintTransformer.Transform(svgLine,
                                            matrix,
                                            out startX,
                                            out startY,
                                            out endX,
                                            out endY,
                                            out strokeWidth);

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

      container.Body.Add(this.FingerPrintCommands.Direction(Direction.Direction1));
      container.Body.Add(this.FingerPrintCommands.Align(Alignment.TopLeft));
      container.Body.Add(this.FingerPrintCommands.Position(horizontalStart,
                                                           verticalStart));
      container.Body.Add(this.FingerPrintCommands.Line(length,
                                                       lineWeight));
    }
  }
}