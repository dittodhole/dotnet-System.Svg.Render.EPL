using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;
using Svg.Pathing;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class SvgPathTranslator : SvgElementTranslatorBase<EplContainer, SvgPath>
  {
    public SvgPathTranslator([NotNull] EplTransformer eplTransformer,
                             [NotNull] EplCommands eplCommands)
    {
      this.EplTransformer = eplTransformer;
      this.EplCommands = eplCommands;
    }

    [NotNull]
    protected EplTransformer EplTransformer { get; }

    [NotNull]
    protected EplCommands EplCommands { get; }

    public override void Translate([NotNull] SvgPath svgElement,
                                   [NotNull] Matrix sourceMatrix,
                                   [NotNull] Matrix viewMatrix,
                                   [NotNull] EplContainer container)
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
                                     sourceMatrix,
                                     viewMatrix,
                                     container);
      }
    }

    protected virtual void TranslateSvgLineSegment([NotNull] SvgPath instance,
                                                   [NotNull] SvgLineSegment svgLineSegment,
                                                   [NotNull] Matrix sourceMatrix,
                                                   [NotNull] Matrix viewMatrix,
                                                   [NotNull] EplContainer container)
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
      this.EplTransformer.Transform(svgLine,
                                    sourceMatrix,
                                    viewMatrix,
                                    out startX,
                                    out startY,
                                    out endX,
                                    out endY,
                                    out strokeWidth);

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

      container.Body.Add(this.EplCommands.LineDrawBlack(horizontalStart,
                                                        verticalStart,
                                                        horizontalLength,
                                                        verticalLength));
    }
  }
}