using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgLineTranslator : SvgElementTranslator<SvgLine>
  {
    public SvgLineTranslator([NotNull]SvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator) {}

    public override bool TryTranslate([NotNull] SvgLine instance,
                                      [NotNull] Matrix matrix,
                                      int targetDpi,
                                      out object translation)
    {
      SvgUnit newStartX;
      SvgUnit newStartY;
      SvgUnit newEndX;
      SvgUnit newEndY;
      if (!this.SvgUnitCalculator.TryApplyMatrix(instance.StartX,
                                                 instance.StartY,
                                                 instance.EndX,
                                                 instance.EndY,
                                                 matrix,
                                                 out newStartX,
                                                 out newStartY,
                                                 out newEndX,
                                                 out newEndY))
      {
#if DEBUG
        translation = $"; could not apply matrix: {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      int startX;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(newStartX,
                                                     targetDpi,
                                                     out startX))
      {
#if DEBUG
        translation = $"; could not get device points (startX): {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      int startY;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(newStartY,
                                                     targetDpi,
                                                     out startY))
      {
#if DEBUG
        translation = $"; could not get device points (startY): {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      int endX;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(newEndX,
                                                     targetDpi,
                                                     out endX))
      {
#if DEBUG
        translation = $"; could not get device points (endX): {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      int endY;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(newEndY,
                                                     targetDpi,
                                                     out endY))
      {
#if DEBUG
        translation = $"; could not get device points (endY): {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      int strokeWidth;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.StrokeWidth,
                                                     targetDpi,
                                                     out strokeWidth))
      {
#if DEBUG
        translation = $"; could not get device points (stroke): {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      if (startX > endX)
      {
        // no .. i won't go down two-variable-way - compiler galore
        var temp = endX;
        endX = startX;
        startX = temp;
      }

      if (startY > endY)
      {
        // no .. i won't go down two-variable-way - compiler galore
        var temp = endY;
        endY = startY;
        startY = temp;
      }

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

      return true;
    }

    private object TranslateHorizontalOrVerticalLine(int startX,
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

    private object TranslateDiagonal(int startX,
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