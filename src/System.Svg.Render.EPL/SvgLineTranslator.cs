using System.Diagnostics;
using System.Drawing;

namespace System.Svg.Render.EPL
{
  public class SvgLineTranslator : SvgElementTranslator<SvgLine>
  {
    public SvgLineTranslator(SvgUnitCalculator svgUnitCalculator)
    {
      if (svgUnitCalculator == null)
      {
        // TODO add documentation
        throw new ArgumentNullException(nameof(svgUnitCalculator));
      }

      this.SvgUnitCalculator = svgUnitCalculator;
    }

    private SvgUnitCalculator SvgUnitCalculator { get; }

    public override object Translate(SvgLine instance)
    {
      // TODO fix if upper/lower bound are swapped
      // TODO implement device-specific values

      var startX = this.SvgUnitCalculator.GetValue(instance.StartX);
      var startY = this.SvgUnitCalculator.GetValue(instance.StartY);
      var endX = this.SvgUnitCalculator.GetValue(instance.EndX);
      var endY = this.SvgUnitCalculator.GetValue(instance.EndY);
      var strokeWidth = this.SvgUnitCalculator.GetValue(instance.StrokeWidth);

      string translation;
      if (startY == endY
          || startX == endX)
      {
        var strokeShouldBeWhite = (instance.Color as SvgColourServer)?.Colour == Color.White;
        if (startX < endX)
        {
          translation = this.TranslateHorizontalLine(startX,
                                                     startY,
                                                     endX,
                                                     strokeWidth,
                                                     strokeShouldBeWhite);
        }
        else if (startY < endY)
        {
          translation = this.TranslateVertialLine(startX,
                                                  startY,
                                                  endY,
                                                  strokeWidth,
                                                  strokeShouldBeWhite);
        }
        else
        {
          Trace.TraceError($@"Could not translate {nameof(SvgLine)}, as either ""{startX} < {endX}"" or ""{startY} < {endY}"" is not true.");
          return string.Empty;
        }
      }
      else if (startX <= endX
               && startY <= endY)
      {
        translation = this.TranslateDiagonal(startX,
                                             startY,
                                             endX,
                                             endY,
                                             strokeWidth);
      }
      else
      {
        Trace.TraceError($@"Could not translate {nameof(SvgLine)}, as either ""{startX} < {endX}"" or ""{startY} < {endY}"" is not true.");
        return string.Empty;
      }

      return translation;
    }

    public string TranslateHorizontalLine(int startX,
                                          int startY,
                                          int endX,
                                          int strokeWidth,
                                          bool strokeShouldBeWhite)
    {
      // horizontal
      var horizontalStart = startX;
      var verticalStart = startY;
      var horizontalLength = endX - startX;
      var verticalLength = strokeWidth;

      string command;
      if (strokeShouldBeWhite)
      {
        command = "LW";
      }
      else
      {
        command = "LO";
      }

      string result = $"{command}{horizontalStart},{verticalStart},{horizontalLength},{verticalLength}";

      return result;
    }

    public string TranslateVertialLine(int startX,
                                       int startY,
                                       int endY,
                                       int strokeWidth,
                                       bool strokeShouldBeWhite)
    {
      // vertical
      var horizontalStart = startX;
      var verticalStart = startY;
      var horizontalLength = strokeWidth;
      var verticalLength = endY - startY;

      string command;
      if (strokeShouldBeWhite)
      {
        command = "LW";
      }
      else
      {
        command = "LO";
      }

      string result = $"{command}{horizontalStart},{verticalStart},{horizontalLength},{verticalLength}";

      return result;
    }

    public string TranslateDiagonal(int startX,
                                    int startY,
                                    int endX,
                                    int endY,
                                    int strokeWidth)
    {
      // diagonal
      var command = "LS";

      var horizontalStart = startX;
      var verticalStart = startY;
      var horizontalLength = strokeWidth;
      var verticalLength = endX;
      var verticalEnd = endY;

      string result = $"{command}{horizontalStart},{verticalStart},{horizontalLength},{verticalLength},{verticalEnd}";

      return result;
    }
  }
}