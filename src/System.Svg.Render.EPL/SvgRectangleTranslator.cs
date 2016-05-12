using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgRectangleTranslator : SvgElementTranslatorWithEncoding<SvgRectangle>
  {
    public SvgRectangleTranslator([NotNull] SvgUnitCalculator svgUnitCalculator,
                                  [NotNull] SvgLineTranslator svgLineTranslator,
                                  [NotNull] Encoding encoding)
      : base(encoding)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
      this.SvgLineTranslator = svgLineTranslator;
    }

    [NotNull]
    private SvgUnitCalculator SvgUnitCalculator { get; }

    [NotNull]
    private SvgLineTranslator SvgLineTranslator { get; }

    public override IEnumerable<byte> Translate([NotNull] SvgRectangle instance,
                                                [NotNull] Matrix matrix)
    {
      if (instance.Fill != SvgPaintServer.None
          && (instance.Fill as SvgColourServer)?.Colour != Color.White)
      {
        return this.TranslateFilledBox(instance,
                                       matrix);
      }
      if (instance.Stroke != SvgPaintServer.None)
      {
        return this.TranslateBox(instance,
                                 matrix);
      }

      return Enumerable.Empty<byte>();
    }

    private IEnumerable<byte> TranslateFilledBox([NotNull] SvgRectangle instance,
                                                 [NotNull] Matrix matrix)
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

      var result = this.SvgLineTranslator.Translate(svgLine,
                                                    matrix);

      return result;
    }

    private IEnumerable<byte> TranslateBox([NotNull] SvgRectangle instance,
                                           [NotNull] Matrix matrix)
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

      var translation = $"X{(int) horizontalStart},{(int) verticalStart},{(int) lineThickness},{(int) horizontalEnd},{(int) verticalEnd}";
      var result = this.GetBytes(translation);

      return result;
    }
  }
}