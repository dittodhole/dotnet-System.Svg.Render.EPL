using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;

namespace System.Svg.Render.EPL
{
  public class SvgUnitCalculator
  {
    // TODO add reading for different origin

    public int SourceDpi { get; set; } = 72;
    public SvgUnitType UserUnitTypeSubstitution { get; set; } = SvgUnitType.Pixel;

    public SvgUnit Add(SvgUnit svgUnit1,
                       SvgUnit svgUnit2)
    {
      var svgUnitType = this.CheckSvgUnitType(svgUnit1,
                                              svgUnit2);

      var val1 = this.GetValue(svgUnit1);
      var val2 = this.GetValue(svgUnit2);

      var result = new SvgUnit(svgUnitType,
                               val1 + val2);

      return result;
    }

    public SvgUnit Substract(SvgUnit svgUnit1,
                             SvgUnit svgUnit2)
    {
      var svgUnitType = this.CheckSvgUnitType(svgUnit1,
                                              svgUnit2);

      var val1 = this.GetValue(svgUnit1);
      var val2 = this.GetValue(svgUnit2);

      var result = new SvgUnit(svgUnitType,
                               val1 - val2);

      return result;
    }

    public SvgUnitType CheckSvgUnitType(SvgUnit svgUnit1,
                                        SvgUnit svgUnit2)
    {
      if (svgUnit1.Type != svgUnit2.Type)
      {
        // TODO add documentation
        throw new ArgumentException($"{nameof(svgUnit2)}'s {nameof(SvgUnit.Type)} ({svgUnit2.Type}) does not equal {nameof(svgUnit1)}'s {nameof(SvgUnit.Type)} ({svgUnit1.Type})");
      }

      return svgUnit1.Type;
    }

    public bool IsValueZero(SvgUnit svgUnit)
    {
      // TODO find a good TOLERANCE
      return Math.Abs(svgUnit.Value) < 0.5f;
    }

    public float GetValue(SvgUnit svgUnit)
    {
      var result = svgUnit.Value;

      return result;
    }

    public int GetDevicePoints(SvgUnit svgUnit,
                               int targetDpi)
    {
      var value = this.GetValue(svgUnit);
      var svgUnitType = svgUnit.Type;

      var result = this.GetDevicePoints(value,
                                        svgUnitType,
                                        targetDpi);

      return result;
    }

    public int GetDevicePoints(float value,
                               SvgUnitType svgUnitType,
                               int targetDpi)
    {
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
        // TODO add documentation
        throw new NotImplementedException($"a conversion for {nameof(value)} {svgUnitType} is currently not implemented");
      }

      var devicePoints = (int) (pixels / this.SourceDpi * targetDpi);

      return devicePoints;
    }

    public enum Rotation
    {
      None,
      Rotate90,
      Rotate180,
      Rotate270
    }

    private Dictionary<Rotation, object> RotationTranslations { get; } = new Dictionary<Rotation, object>
                                                                         {
                                                                           {
                                                                             Rotation.None, 0
                                                                           },
                                                                           {
                                                                             Rotation.Rotate90, 1
                                                                           },
                                                                           {
                                                                             Rotation.Rotate180, 2
                                                                           },
                                                                           {
                                                                             Rotation.Rotate270, 3
                                                                           }
                                                                         };

    public bool TryApplyMatrixTransformation(Matrix matrix,
                                             ref PointF startPoint,
                                             out object rotationTranslation)
    {
      if (matrix == null)
      {
        // TODO add documentation
        throw new ArgumentNullException(nameof(matrix));
      }

      var endPoint = new PointF
                     {
                       X = startPoint.X + 10,
                       Y = startPoint.Y
                     };

      var points = new[]
                   {
                     startPoint,
                     endPoint
                   };
      matrix.TransformPoints(points);

      startPoint = points.ElementAt(0);
      endPoint = points.ElementAt(1);

      Rotation rotation;

      // TODO find a good tolerance
      if (Math.Abs(startPoint.Y - endPoint.Y) < 0.5f)
      {
        if (startPoint.X < endPoint.X)
        {
          rotation = Rotation.None;
        }
        else if (startPoint.X > endPoint.X)
        {
          rotation = Rotation.Rotate180;
        }
        else
        {
          // TODO woho, stop - HAMMER TIME - transformation results in a singularity?
          rotationTranslation = null;
          return false;
        }
      }
      else if (startPoint.Y > endPoint.Y)
      {
        rotation = Rotation.Rotate90;
      }
      else if (startPoint.Y < endPoint.Y)
      {
        rotation = Rotation.Rotate270;
      }
      else
      {
        // TODO woho, stop - HAMMER TIME - transformation results in a singularity?
        rotationTranslation = null;
        return false;
      }

      rotationTranslation = this.GetRotationTranslation(rotation);

      return true;
    }

    public object GetRotationTranslation(Rotation rotation)
    {
      return this.RotationTranslations[rotation];
    }
  }
}