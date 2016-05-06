using System.Drawing;
using System.Drawing.Drawing2D;
using System.Svg.Transforms;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgUnitCalculator
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
        rotationTranslation = null;
        return false;
      }

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
      }

      rotationTranslation = null;
      return false;
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
          continue;
        }

        var matrixToMultiply = transformation.Matrix;
        if (matrixToMultiply == null)
        {
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

    public bool TryGetFontTranslation([NotNull] SvgTextBase svgTextBase,
                                      [NotNull] Matrix matrix,
                                      int targetDpi,
                                      out object translation)
    {
      int fontSize;
      if (!this.TryGetDevicePoints(svgTextBase.FontSize,
                                   targetDpi,
                                   out fontSize))
      {
        translation = null;
        return false;
      }

      object fontSelection;
      object multiplier;
      if (!this.TryGetFontSelection(fontSize,
                                    targetDpi,
                                    out fontSelection,
                                    out multiplier))
      {
        translation = null;
        return false;
      }

      translation = $"{fontSelection},{multiplier},{multiplier}";

      return true;
    }

    private class FontDefinition
    {
      public int Width { get; set; }
      public int Height { get; set; }
      public string Font { get; set; }
    }

    private bool TryGetFontSelection(int fontSize,
                                     int targetDpi,
                                     out object fontSelection,
                                     out object multiplier)
    {
      // VALUE    203dpi        300dpi
      // ==================================
      //  1       20.3cpi       25cpi
      //          6pts          4pts
      //          8x12 dots     12x20 dots
      //          1:1.5         1:1.66
      // ==================================
      //  2       16.9cpi       18.75cpi
      //          7pts          6pts
      //          10x16 dots    16x28 dots
      //          1:1.6         1:1.75
      // ==================================
      //  3       14.5cpi       15cpi
      //          10pts         8pts
      //          12x20 dots    20x36 dots
      //          1:1.66        1:1.8
      // ==================================
      //  4       12.7cpi       12.5cpi
      //          12pts         10pts
      //          14x24 dots    24x44 dots
      //          1:1.71        1:1.83
      // ==================================
      //  5       5.6cpi        6.25cpi
      //          24pts         21pts
      //          32x48 dots    48x80 dots
      //          1:1.5         1:1.6
      // ==================================
      // horizontal multiplier: Accepted Values: 1–6, 8
      // vertical multiplier: Accepted Values: 1–9

      FontDefinition[] fontDefinitions;
      if (targetDpi == 203)
      {
        fontDefinitions = new[]
                          {
                            new FontDefinition
                            {
                              Width = 8,
                              Height = 12,
                              Font = "1"
                            },
                            new FontDefinition
                            {
                              Width = 10,
                              Height = 16,
                              Font = "2"
                            },
                            new FontDefinition
                            {
                              Width = 12,
                              Height = 20,
                              Font = "3"
                            },
                            new FontDefinition
                            {
                              Width = 14,
                              Height = 24,
                              Font = "4"
                            },
                            new FontDefinition
                            {
                              Width = 32,
                              Height = 48,
                              Font = "5"
                            }
                          };
      }
      else if (targetDpi == 300)
      {
        fontDefinitions = new[]
                          {
                            new FontDefinition
                            {
                              Width = 12,
                              Height = 20,
                              Font = "1"
                            },
                            new FontDefinition
                            {
                              Width = 16,
                              Height = 28,
                              Font = "2"
                            },
                            new FontDefinition
                            {
                              Width = 20,
                              Height = 36,
                              Font = "3"
                            },
                            new FontDefinition
                            {
                              Width = 24,
                              Height = 44,
                              Font = "4"
                            },
                            new FontDefinition
                            {
                              Width = 48,
                              Height = 80,
                              Font = "5"
                            }
                          };
      }
      else
      {
        fontSelection = null;
        multiplier = null;
        return false;
      }

      // TODO maybe ... :angel: ... optimize performance ... :athletic_shoe:

      fontSelection = null;
      multiplier = null;

      foreach (var possibleMultiplier in new[]
                                         {
                                           1,
                                           2,
                                           3,
                                           4,
                                           5,
                                           6,
                                           8
                                         })
      {
        multiplier = possibleMultiplier;

        foreach (var fontDefinition in fontDefinitions)
        {
          if (fontSelection == null)
          {
            fontSelection = fontDefinition.Font;
          }

          var height = fontDefinition.Height * possibleMultiplier;
          if (height > fontSize)
          {
            return true;
          }

          fontSelection = fontDefinition.Font;
        }
      }

      return false;
    }
  }
}