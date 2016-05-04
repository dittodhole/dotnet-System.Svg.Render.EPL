using System.Drawing;
using System.Drawing.Drawing2D;
using Anotar.LibLog;

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

    protected SvgUnitCalculator SvgUnitCalculator { get; }

    public override object Translate(SvgLine instance,
                                     Matrix matrix,
                                     int targetDpi)
    {
      if (instance == null)
      {
        LogTo.Error($"{nameof(instance)} is null");
        return null;
      }
      if (matrix == null)
      {
        LogTo.Error($"{nameof(matrix)} is null");
      }

      int startX;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.StartX,
                                                     targetDpi,
                                                     out startX))
      {
        LogTo.Error($"could not convert {instance.StartX} to device points");
        return null;
      }

      int startY;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.StartY,
                                                     targetDpi,
                                                     out startY))
      {
        LogTo.Error($"could not convert {instance.StartY} to device points");
        return null;
      }

      int endX;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.EndX,
                                                     targetDpi,
                                                     out endX))
      {
        LogTo.Error($"could not convert {instance.EndX} to device points");
        return null;
      }

      int endY;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.EndY,
                                                     targetDpi,
                                                     out endY))
      {
        LogTo.Error($"could not convert {instance.EndY} to device points");
        return null;
      }

      int strokeWidth;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.StrokeWidth,
                                                     targetDpi,
                                                     out strokeWidth))
      {
        LogTo.Error($"could not convert {instance.StrokeWidth} to device points");
        return null;
      }

      if (startX > endX)
      {
        LogTo.Error($"invalid coordinates: {nameof(startX)} ({startX}) is greater than {nameof(endX)} ({endX})");
        return null;
      }
      if (startY > endY)
      {
        LogTo.Error($"invalid coordinates: {nameof(startY)} ({startY}) is greater than {nameof(endY)} ({endY})");
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

    protected virtual string TranslateHorizontalOrVerticalLine(int startX,
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

    protected virtual string TranslateDiagonal(int startX,
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