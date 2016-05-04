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
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.StartX,
                                                     targetDpi,
                                                     out startX))
      {
        // TODO add logging
        return null;
      }

      int startY;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.StartY,
                                                     targetDpi,
                                                     out startY))
      {
        // TODO add logging
        return null;
      }

      int endX;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.EndX,
                                                     targetDpi,
                                                     out endX))
      {
        // TODO add logging
        return null;
      }

      int endY;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.EndY,
                                                     targetDpi,
                                                     out endY))
      {
        // TODO add logging
        return null;
      }

      int strokeWidth;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.StrokeWidth,
                                                     targetDpi,
                                                     out strokeWidth))
      {
        // TODO add logging
        return null;
      }

      if (startX > endX)
      {
        // TODO add logging
        return null;
      }
      if (startY > endY)
      {
        // TODO add logging
        return null;
      }

      string translation;
      if (startY == endY
          || startX == endX)
      {
        var strokeShouldBeWhite = (instance.Stroke as SvgColourServer)?.Colour == Color.White;
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

      var result = $"{command}{horizontalStart},{verticalStart},{horizontalLength},{verticalLength}";

      return result;
    }

    public string TranslateDiagonal(int startX,
                                    int startY,
                                    int endX,
                                    int endY,
                                    int strokeWidth)
    {
      var horizontalStart = startX;
      var verticalStart = startY;
      var horizontalLength = strokeWidth;
      var verticalLength = endX;
      var verticalEnd = endY;

      var result = $"LS{horizontalStart},{verticalStart},{horizontalLength},{verticalLength},{verticalEnd}";

      return result;
    }
  }
}