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

    public bool TryApplyMatrix(SvgUnit x,
                               SvgUnit y,
                               [NotNull] Matrix matrix,
                               out SvgUnit newX,
                               out SvgUnit newY)
    {
      var typeX = x.Type;
      var typeY = y.Type;
      if (typeX != typeY)
      {
        newX = SvgUnit.None;
        newY = SvgUnit.None;
        return false;
      }

      var originalX = x.Value;
      var originalY = y.Value;
      var originalPoint = new PointF(originalX,
                                     originalY);

      var points = new[]
                   {
                     originalPoint
                   };
      matrix.TransformPoints(points);

      var transformedPoint = points[0];
      var transformedX = transformedPoint.X;
      var transformedY = transformedPoint.Y;

      newX = new SvgUnit(typeX,
                         transformedX);
      newY = new SvgUnit(typeY,
                         transformedY);

      return true;
    }

    public bool TryApplyMatrix(SvgUnit x1,
                               SvgUnit y1,
                               SvgUnit x2,
                               SvgUnit y2,
                               [NotNull] Matrix matrix,
                               out SvgUnit newX1,
                               out SvgUnit newY1,
                               out SvgUnit newX2,
                               out SvgUnit newY2)
    {
      var typeX1 = x1.Type;
      var typeY1 = y1.Type;
      if (typeX1 != typeY1)
      {
        newX1 = SvgUnit.None;
        newY1 = SvgUnit.None;
        newX2 = SvgUnit.None;
        newY2 = SvgUnit.None;
        return false;
      }

      var typeX2 = x2.Type;
      var typeY2 = y2.Type;
      if (typeX2 != typeY2)
      {
        newX1 = SvgUnit.None;
        newY1 = SvgUnit.None;
        newX2 = SvgUnit.None;
        newY2 = SvgUnit.None;
        return false;
      }

      var originalPoint1 = new PointF(x1.Value,
                                      y1.Value);
      var originalPoint2 = new PointF(x2.Value,
                                      y2.Value);

      var points = new[]
                   {
                     originalPoint1,
                     originalPoint2
                   };
      matrix.TransformPoints(points);

      {
        var transformedPoint1 = points[0];
        var transformedX1 = transformedPoint1.X;
        var transformedY1 = transformedPoint1.Y;

        newX1 = new SvgUnit(typeX1,
                            transformedX1);
        newY1 = new SvgUnit(typeY1,
                            transformedY1);
      }

      {
        var transformedPoint2 = points[1];
        var transformedX2 = transformedPoint2.X;
        var transformedY2 = transformedPoint2.Y;

        newX2 = new SvgUnit(typeX2,
                            transformedX2);
        newY2 = new SvgUnit(typeY2,
                            transformedY2);
      }

      return true;
    }
  }
}