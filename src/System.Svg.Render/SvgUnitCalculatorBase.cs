using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public class SvgUnitCalculatorBase : ISvgUnitCalculator
  {
    public int SourceDpi { get; set; } = 72;
    public SvgUnitType UserUnitTypeSubstitution { get; set; } = SvgUnitType.Pixel;

    public bool TryAdd(SvgUnit svgUnit1,
                       SvgUnit svgUnit2,
                       out SvgUnit result)
    {
      var svgUnitType = svgUnit1.Type;
      if (svgUnitType != svgUnit2.Type)
      {
        result = SvgUnit.None;
        return false;
      }

      var val1 = svgUnit1.Value;
      var val2 = svgUnit2.Value;
      var value = val1 + val2;

      result = new SvgUnit(svgUnitType,
                           value);

      return true;
    }

    public bool TryGetDevicePoints(SvgUnit svgUnit,
                                   int targetDpi,
                                   out int devicePoints)
    {
      var value = svgUnit.Value;
      var svgUnitType = svgUnit.Type;
      if (svgUnitType == SvgUnitType.User)
      {
        svgUnitType = this.UserUnitTypeSubstitution;
      }

      float? inches;
      if (svgUnitType == SvgUnitType.Inch)
      {
        inches = value;
      }
      else if (svgUnitType == SvgUnitType.Centimeter)
      {
        inches = value / 2.54f;
      }
      else if (svgUnitType == SvgUnitType.Millimeter)
      {
        inches = value / 10f / 2.54f;
      }
      else if (svgUnitType == SvgUnitType.Point)
      {
        inches = value / 72f;
      }
      else if (svgUnitType == SvgUnitType.Pica)
      {
        inches = value / 10f / 72f;
      }
      else
      {
        inches = null;
      }

      float pixels;
      if (svgUnitType == SvgUnitType.Pixel)
      {
        pixels = value;
      }
      else if (inches.HasValue)
      {
        pixels = inches.Value * this.SourceDpi;
      }
      else
      {
        devicePoints = 0;
        return false;
      }

      devicePoints = (int) (pixels / this.SourceDpi * targetDpi);

      return true;
    }

    public void ApplyMatrixToDevicePoints(int x,
                                          int y,
                                          [NotNull] Matrix matrix,
                                          out int newX,
                                          out int newY)
    {
      var originalPoint = new Point(x,
                                    y);

      var points = new[]
                   {
                     originalPoint
                   };
      matrix.TransformPoints(points);

      var transformedPoint = points[0];
      transformedPoint = this.AdaptPoint(transformedPoint);
      newX = transformedPoint.X;
      newY = transformedPoint.Y;
    }

    protected virtual Point AdaptPoint(Point point)
    {
      return point;
    }
  }
}