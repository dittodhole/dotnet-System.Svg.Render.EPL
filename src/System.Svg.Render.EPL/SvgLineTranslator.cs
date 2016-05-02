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

      if (startX > endX)
      {
        Trace.TraceError($@"Could not translate {nameof(SvgLine)}, as either ""{startX} < {endX}"" is not true.");
        return null;
      }
      if (startY > endY)
      {
        Trace.TraceError($@"Could not translate {nameof(SvgLine)}, as either ""{startY} < {endY}"" is not true.");
        return null;
      }

      string translation;
      if (startY == endY
          || startX == endX)
      {
        var strokeShouldBeWhite = (instance.Color as SvgColourServer)?.Colour == Color.White;
        translation = this.TranslateHorizontalOrVerticalLine(startX,
                                                             startY,
                                                             endX,
                                                             endY,
                                                             strokeWidth,
                                                             strokeShouldBeWhite);
      }
      else
      {
        translation = this.TranslateDiagonal(startX,
                                             startY,
                                             endX,
                                             endY,
                                             strokeWidth);
      }

      return translation;
    }

    public string TranslateHorizontalOrVerticalLine(int startX,
                                                    int startY,
                                                    int endX,
                                                    int endY,
                                                    int strokeWidth,
                                                    bool strokeShouldBeWhite)
    {
      var horizontalStart = startX;
      var verticalStart = startY;
      var horizontalLength = endX - startX;
      if (horizontalLength == 0)
      {
        horizontalLength = strokeWidth;
      }
      var verticalLength = endY - startY;
      if (verticalLength == 0)
      {
        verticalLength = strokeWidth;
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