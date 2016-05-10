using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgRectangleTranslator : SvgElementTranslatorBase<SvgRectangle>
  {
    public SvgRectangleTranslator([NotNull] SvgUnitCalculator svgUnitCalculator,
                                  [NotNull] SvgLineTranslator svgLineTranslator)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
      this.SvgLineTranslator = svgLineTranslator;
    }

    [NotNull]
    private SvgUnitCalculator SvgUnitCalculator { get; }

    [NotNull]
    private SvgLineTranslator SvgLineTranslator { get; }

    public override void Translate([NotNull] SvgRectangle instance,
                                   [NotNull] Matrix matrix,
                                   out object translation)
    {
      if (instance.Fill != SvgPaintServer.None
          && (instance.Fill as SvgColourServer)?.Colour != Color.White)
      {
        this.TranslateFilledBox(instance,
                                matrix,
                                out translation);
      }
      else if (instance.Stroke != SvgPaintServer.None)
      {
        this.TranslateBox(instance,
                          matrix,
                          out translation);
      }
      else
      {
#if DEBUG
        translation = $"; could not translate {nameof(SvgRectangle)}: {instance.GetXML()}";
#else
        translation = null;
#endif
      }
    }

    private void TranslateFilledBox([NotNull] SvgRectangle instance,
                                    [NotNull] Matrix matrix,
                                    out object translation)
    {
      var endX = this.SvgUnitCalculator.GetValue(instance.X) + this.SvgUnitCalculator.GetValue(instance.Width);
      var startY = this.SvgUnitCalculator.GetValue(instance.Y) + this.SvgUnitCalculator.GetValue(instance.Height);

      var svgLine = new SvgLine
                    {
                      StartX = instance.X,
                      StartY = startY,
                      EndX = endX,
                      EndY = startY,
                      StrokeWidth = instance.Height
                    };

      this.SvgLineTranslator.Translate(svgLine,
                                       matrix,
                                       out translation);
    }

    private void TranslateBox([NotNull] SvgRectangle instance,
                              [NotNull] Matrix matrix,
                              out object translation)
    {
      var horizontalStart = this.SvgUnitCalculator.GetValue(instance.X);
      var verticalStart = this.SvgUnitCalculator.GetValue(instance.Y);
      var horizontalEnd = this.SvgUnitCalculator.GetValue(instance.X) + this.SvgUnitCalculator.GetValue(instance.Width);
      var verticalEnd = this.SvgUnitCalculator.GetValue(instance.Y) + this.SvgUnitCalculator.GetValue(instance.Height);
      var lineThickness = this.SvgUnitCalculator.GetValue(instance.StrokeWidth);

      horizontalStart -= lineThickness / 2f;
      verticalStart -= lineThickness / 2f;
      horizontalEnd += lineThickness / 2f;
      verticalEnd += lineThickness / 2f;

      this.SvgUnitCalculator.ApplyMatrix(horizontalStart,
                                         verticalStart,
                                         matrix,
                                         out horizontalStart,
                                         out verticalStart);

      this.SvgUnitCalculator.ApplyMatrix(horizontalEnd,
                                         verticalEnd,
                                         matrix,
                                         out horizontalEnd,
                                         out verticalEnd);

      this.SvgUnitCalculator.ApplyMatrix(lineThickness,
                                         matrix,
                                         out lineThickness);

      translation = $"X{(int) horizontalStart},{(int) verticalStart},{(int) lineThickness},{(int) horizontalEnd},{(int) verticalEnd}";
    }
  }
}