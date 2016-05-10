using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public class SvgUnitCalculatorBase : ISvgUnitCalculator
  {
    public void ApplyMatrix(float x,
                            float y,
                            [NotNull] Matrix matrix,
                            out float newX,
                            out float newY)
    {
      var originalPoint = new PointF(x,
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

    protected virtual PointF AdaptPoint(PointF point)
    {
      return point;
    }

    public float GetLengthOfVector(PointF vector)
    {
      var result = Math.Sqrt(Math.Pow(vector.X,
                                      2) + Math.Pow(vector.Y,
                                                    2));

      return (int) result;
    }

    public float GetValue(SvgUnit svgUnit)
    {
      // TODO we asome that we are working with pixels all the way
      var result = svgUnit.Value;

      return result;
    }

    public void ApplyMatrix(PointF vector,
                            Matrix matrix,
                            out PointF newVector)
    {
      var vectors = new[]
                    {
                      vector
                    };

      matrix.TransformVectors(vectors);

      newVector = vectors[0];
    }

    public void ApplyMatrix(float length,
                            Matrix matrix,
                            out float newLength)
    {
      var vector = new PointF(length,
                              0f);

      this.ApplyMatrix(vector,
                       matrix,
                       out vector);

      newLength = this.GetLengthOfVector(vector);
    }
  }
}