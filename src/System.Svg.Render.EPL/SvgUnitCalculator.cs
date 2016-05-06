using System.Drawing;
using System.Drawing.Drawing2D;
using System.Svg.Transforms;
using Anotar.LibLog;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgUnitCalculator
  {
    public int SourceDpi { get; set; } = 72;
    public SvgUnitType UserUnitTypeSubstitution { get; set; } = SvgUnitType.Pixel;

    /// <exception cref="ArgumentException">If <see cref="SvgUnitType" /> of <paramref name="svgUnit1" /> and <paramref name="svgUnit2" /> are not matching.</exception>
    public virtual SvgUnit Add(SvgUnit svgUnit1,
                               SvgUnit svgUnit2)
    {
      var svgUnitType = svgUnit1.Type;
      if (svgUnitType != svgUnit2.Type)
      {
        throw new ArgumentException($"{nameof(svgUnit1)} ({svgUnit1}) and {nameof(svgUnit2)} ({svgUnit2}) are not of same type");
      }

      var val1 = svgUnit1.Value;
      var val2 = svgUnit2.Value;
      var value = val1 + val2;

      var result = new SvgUnit(svgUnitType,
                               value);

      return result;
    }

    /// <exception cref="ArgumentException">If <see cref="SvgUnitType" /> of <paramref name="svgUnit1" /> and <paramref name="svgUnit2" /> are not matching.</exception>
    public virtual SvgUnit Substract(SvgUnit svgUnit1,
                                     SvgUnit svgUnit2)
    {
      var svgUnitType = svgUnit1.Type;
      if (svgUnitType != svgUnit2.Type)
      {
        throw new ArgumentException($"{nameof(svgUnit1)} ({svgUnit1}) and {nameof(svgUnit2)} ({svgUnit2}) are not of same type");
      }

      var val1 = svgUnit1.Value;
      var val2 = svgUnit2.Value;
      var value = val1 - val2;

      var result = new SvgUnit(svgUnitType,
                               value);

      return result;
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
      var result = this.TryGetDevicePoints(svgUnit.Value,
                                           svgUnit.Type,
                                           targetDpi,
                                           out devicePoints);

      return result;
    }

    public virtual bool TryGetDevicePoints(float value,
                                           SvgUnitType svgUnitType,
                                           int targetDpi,
                                           out int devicePoints)
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
        devicePoints = 0;
        return false;
      }

      devicePoints = (int) (pixels / this.SourceDpi * targetDpi);

      return true;
    }

    private enum Rotation
    {
      None,
      Rotate90,
      Rotate180,
      Rotate270
    }

    public bool TryGetRotationTranslation([NotNull] Matrix matrix,
                                          out object rotationTranslation)
    {
      // TODO check with real scenarios

      var startPoint = new PointF(0f,
                                  0f);
      var endPoint = new PointF(10f,
                                0f);

      var points = new[]
                   {
                     startPoint,
                     endPoint
                   };

      matrix.TransformPoints(points);

      startPoint = points[0];
      endPoint = points[1];

      Rotation rotation;

      // TODO find a good TOLERANCE
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
          LogTo.Error($"HAMMER TIME - singularity detected ({startPoint.X}/{startPoint.Y}, {endPoint.X}/{endPoint.Y})");
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
        LogTo.Error($"HAMMER TIME - singularity detected ({startPoint.X}/{startPoint.Y}, {endPoint.X}/{endPoint.Y})");
        rotationTranslation = null;
        return false;
      }

      if (this.TryGetRotationTranslation(rotation,
                                         out rotationTranslation))
      {
        return true;
      }

      return false;
    }

    private bool TryGetRotationTranslation(Rotation rotation,
                                           out object rotationTranslation)
    {
      switch (rotation)
      {
        case Rotation.None:
          rotationTranslation = "0";
          return true;
        case Rotation.Rotate90:
          rotationTranslation = "1";
          return true;
        case Rotation.Rotate180:
          rotationTranslation = "2";
          return true;
        case Rotation.Rotate270:
          rotationTranslation = "3";
          return true;
        default:
          rotationTranslation = null;
          return false;
      }
    }

    public Matrix MultiplyTransformationsIntoNewMatrix([NotNull] ISvgTransformable svgTransformable,
                                                       [NotNull] Matrix matrix)
    {
      var result = default(Matrix);
      foreach (var transformation in svgTransformable.Transforms)
      {
        var transformationType = transformation.GetType();
        if (!this.IsTransformationAllowed(svgTransformable,
                                          transformationType))
        {
          LogTo.Error($"transformation {transformationType} is not allowed");
          continue;
        }

        var matrixToMultiply = transformation.Matrix;
        if (matrixToMultiply == null)
        {
          LogTo.Error($"{nameof(transformation.Matrix)} is null");
          continue;
        }

        if (result == null)
        {
          result = matrix.Clone();
        }

        result.Multiply(matrixToMultiply,
                        MatrixOrder.Append);
      }

      return result ?? matrix;
    }

    private bool IsTransformationAllowed([NotNull] ISvgTransformable svgTransformable,
                                         [NotNull] Type type)
    {
      if (type == typeof(SvgMatrix))
      {
        return true;
      }
      if (type == typeof(SvgRotate))
      {
        return true;
      }
      if (type == typeof(SvgScale))
      {
        return true;
      }
      if (type == typeof(SvgTranslate))
      {
        return true;
      }

      return false;
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
        LogTo.Error($"types do not match: {typeX} - {typeY}");
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

    public virtual bool TryApplyMatrix(SvgUnit x1,
                                       SvgUnit y1,
                                       SvgUnit x2,
                                       SvgUnit y2,
                                       Matrix matrix,
                                       out SvgUnit newX1,
                                       out SvgUnit newY1,
                                       out SvgUnit newX2,
                                       out SvgUnit newY2)
    {
      if (matrix == null)
      {
        LogTo.Error($"{nameof(matrix)} is null");
        newX1 = SvgUnit.None;
        newY1 = SvgUnit.None;
        newX2 = SvgUnit.None;
        newY2 = SvgUnit.None;
        return false;
      }

      var typeX1 = x1.Type;
      var typeY1 = y1.Type;
      if (typeX1 != typeY1)
      {
        LogTo.Error($"types do not match: {nameof(typeX1)} ({typeX1}) - {nameof(typeY1)} ({typeY1})");
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
        LogTo.Error($"types do not match: {nameof(typeX2)} ({typeX2}) - {nameof(typeY2)} ({typeY2})");
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