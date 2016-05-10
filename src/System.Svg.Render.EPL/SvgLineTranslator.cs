using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgLineTranslator : SvgElementTranslatorBase<SvgLine>
  {
    public SvgLineTranslator([NotNull] SvgUnitCalculator svgUnitCalculator)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
    }

    [NotNull]
    private SvgUnitCalculator SvgUnitCalculator { get; }

    public override void Translate([NotNull] SvgLine instance,
                                   [NotNull] Matrix matrix,
                                   out object translation)
    {
      var startX = this.SvgUnitCalculator.GetValue(instance.StartX);
      var startY = this.SvgUnitCalculator.GetValue(instance.StartY);
      var endX = this.SvgUnitCalculator.GetValue(instance.EndX);
      var endY = this.SvgUnitCalculator.GetValue(instance.EndY);
      var strokeWidth = this.SvgUnitCalculator.GetValue(instance.StrokeWidth);

      this.SvgUnitCalculator.ApplyMatrix(startX,
                                         startY,
                                         matrix,
                                         out startX,
                                         out startY);

      this.SvgUnitCalculator.ApplyMatrix(endX,
                                         endY,
                                         matrix,
                                         out endX,
                                         out endY);

      this.SvgUnitCalculator.ApplyMatrix(strokeWidth,
                                         matrix,
                                         out strokeWidth);

      // TODO find a good TOLERANCE
      if (Math.Abs(startY - endY) < 0.5f
          || Math.Abs(startX - endX) < 0.5f)
      {
        var strokeShouldBeWhite = (instance.Stroke as SvgColourServer)?.Colour == Color.White;
        var horizontalStart = (int) startX;
        var verticalStart = (int) startY;
        var horizontalLength = (int) endX - (int) startX;
        if (horizontalLength == 0)
        {
          horizontalLength = (int) strokeWidth;
        }
        var verticalLength = (int) endY - (int) startY;
        if (verticalLength == 0)
        {
          verticalLength = (int) strokeWidth;
        }

        string command;
        if (strokeShouldBeWhite)
        {
          command = "LW";
        }
        else
        {
          command = "LO";
        }

        var result = $"{command}{horizontalStart},{verticalStart},{horizontalLength},{verticalLength}";

        translation = result;
      }
      else
      {
        var horizontalStart = (int) startX;
        var verticalStart = (int) startY;
        var horizontalLength = (int) strokeWidth;
        var verticalLength = (int) endX;
        var verticalEnd = (int) endY;

        var result = $"LS{horizontalStart},{verticalStart},{horizontalLength},{verticalLength},{verticalEnd}";

        translation = result;
      }
    }
  }
}