using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgLineTranslator : SvgElementTranslatorBase<SvgLine>
  {
    public SvgLineTranslator([NotNull] ISvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
    }

    [NotNull]
    private ISvgUnitCalculator SvgUnitCalculator { get; }

    public override bool TryTranslate([NotNull] SvgLine instance,
                                      [NotNull] Matrix matrix,
                                      int targetDpi,
                                      out object translation)
    {
      int startX;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.StartX,
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
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.StartY,
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
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.EndX,
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
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.EndY,
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

      this.SvgUnitCalculator.ApplyMatrixToDevicePoints(startX,
                                                       startY,
                                                       matrix,
                                                       out startX,
                                                       out startY);

      this.SvgUnitCalculator.ApplyMatrixToDevicePoints(endX,
                                                       endY,
                                                       matrix,
                                                       out endX,
                                                       out endY);

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