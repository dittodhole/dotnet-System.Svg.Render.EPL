using System.Diagnostics;
using System.Drawing;

namespace System.Svg.Render.EPL
{
  public class SvgLineTranslator : SvgElementTranslator<SvgLine>
  {
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitCalculator" /> is <see langword="null" />.</exception>
    public SvgLineTranslator(SvgUnitCalculator svgUnitCalculator)
    {
      if (svgUnitCalculator == null)
      {
        throw new ArgumentNullException(nameof(svgUnitCalculator));
      }

      this.SvgUnitCalculator = svgUnitCalculator;
    }

    private SvgUnitCalculator SvgUnitCalculator { get; }

    public override object Translate(SvgLine instance,
                                     int targetDpi)
    {
      if (instance == null)
      {
        // TODO add logging
        return null;
      }

      int startX;
      try
      {
        startX = this.SvgUnitCalculator.GetDevicePoints(instance.StartX,
                                                        targetDpi);
      }
      catch (NotImplementedException notImplementedException)
      {
        // TODO add logging
        return null;
      }

      int startY;
      try
      {
        startY = this.SvgUnitCalculator.GetDevicePoints(instance.StartY,
                                                        targetDpi);
      }
      catch (NotImplementedException notImplementedException)
      {
        // TODO add logging
        return null;
      }

      int endX;
      try
      {
        endX = this.SvgUnitCalculator.GetDevicePoints(instance.EndX,
                                                      targetDpi);
      }
      catch (NotImplementedException notImplementedException)
      {
        // TODO add logging
        return null;
      }

      int endY;
      try
      {
        endY = this.SvgUnitCalculator.GetDevicePoints(instance.EndY,
                                                      targetDpi);
      }
      catch (NotImplementedException notImplementedException)
      {
        // TODO add logging
        return null;
      }
      int strokeWidth;
      try
      {
        strokeWidth = this.SvgUnitCalculator.GetDevicePoints(instance.StrokeWidth,
                                                             targetDpi);
      }
      catch (NotImplementedException notImplementedException)
      {
        // TODO add logging
        return null;
      }

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