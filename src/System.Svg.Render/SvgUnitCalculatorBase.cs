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

    public bool IsValueZero(SvgUnit svgUnit)
    {
      // TODO find a good TOLERANCE
      return Math.Abs(svgUnit.Value) < 0.5f;
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

    public void ApplyMatrixToDevicePoints(int x1,
                                          int y1,
                                          int x2,
                                          int y2,
                                          [NotNull] Matrix matrix,
                                          out int newX1,
                                          out int newY1,
                                          out int newX2,
                                          out int newY2)
    {
      var originalPoint1 = new Point(x1,
                                     x2);
      var originalPoint2 = new Point(x2,
                                     y2);

      var points = new[]
                   {
                     originalPoint1,
                     originalPoint2
                   };
      matrix.TransformPoints(points);

      {
        var transformedPoint1 = points[0];
        transformedPoint1 = this.AdaptPoint(transformedPoint1);
        newX1 = transformedPoint1.X;
        newY1 = transformedPoint1.Y;
      }

      {
        var transformedPoint2 = points[1];
        transformedPoint2 = this.AdaptPoint(transformedPoint2);
        newX2 = transformedPoint2.X;
        newY2 = transformedPoint2.Y;
      }
    }

    protected virtual Point AdaptPoint(Point point)
    {
      return point;
    }
  }
}