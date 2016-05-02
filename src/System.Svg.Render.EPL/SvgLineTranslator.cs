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

    public override object Translate(SvgLine instance,
                                     int targetDpi)
    {
      // TODO fix if upper/lower bound are swapped
      // TODO implement device-specific values

      var startX = this.SvgUnitCalculator.GetDevicePoints(instance.StartX,
                                                          targetDpi);
      var startY = this.SvgUnitCalculator.GetDevicePoints(instance.StartY,
                                                          targetDpi);
      var endX = this.SvgUnitCalculator.GetDevicePoints(instance.EndX,
                                                        targetDpi);
      var endY = this.SvgUnitCalculator.GetDevicePoints(instance.EndY,
                                                        targetDpi);
      var strokeWidth = this.SvgUnitCalculator.GetDevicePoints(instance.StrokeWidth,
                                                               targetDpi);

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
          translation = this.TranslateVerticalLine(startX,
                                                   startY,
                                                   endY,
                                                   strokeWidth,
                                                   strokeShouldBeWhite);
        }
        else
        {
          Trace.TraceError($@"Could not translate {nameof(SvgLine)}, as either ""{startX} < {endX}"" or ""{startY} < {endY}"" is not true.");
          return null;
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
        return null;
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

    public string TranslateVerticalLine(int startX,
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