using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public interface ISvgUnitCalculator
  {
    void ApplyMatrix(float x,
                     float y,
                     [NotNull] Matrix matrix,
                     out float newX,
                     out float newY);

    float GetLengthOfVector(PointF vector);

    float GetValue(SvgUnit svgUnit);

    void ApplyMatrix(PointF vector,
                     [NotNull] Matrix matrix,
                     out PointF newVector);

    void ApplyMatrix(float length,
                     [NotNull] Matrix matrix,
                     out float newLength);
  }
}