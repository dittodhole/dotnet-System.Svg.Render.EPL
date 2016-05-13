using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgLineTranslator : SvgElementTranslatorBase<SvgLine>
  {
    public SvgLineTranslator([NotNull] SvgUnitCalculator svgUnitCalculator,
                             [NotNull] EplCommands eplCommands)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
      this.EplCommands = eplCommands;
    }

    [NotNull]
    private SvgUnitCalculator SvgUnitCalculator { get; }

    [NotNull]
    private EplCommands EplCommands { get; }

    public override IEnumerable<byte> Translate([NotNull] SvgLine instance,
                                                [NotNull] Matrix matrix)
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

      IEnumerable<byte> result;

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

        if (strokeShouldBeWhite)
        {
          result = this.EplCommands.LineDrawWhite(horizontalStart,
                                                  verticalStart,
                                                  horizontalLength,
                                                  verticalLength);
        }
        else
        {
          result = this.EplCommands.LineDrawBlack(horizontalStart,
                                                  verticalStart,
                                                  horizontalLength,
                                                  verticalLength);
        }
      }
      else
      {
        var horizontalStart = (int) startX;
        var verticalStart = (int) startY;
        var horizontalLength = (int) strokeWidth;
        var verticalLength = (int) endX;
        var verticalEnd = (int) endY;

        result = this.EplCommands.LineDrawDiagonal(horizontalStart,
                                                   verticalStart,
                                                   horizontalLength,
                                                   verticalLength,
                                                   verticalEnd);
      }

      return result;
    }
  }
}