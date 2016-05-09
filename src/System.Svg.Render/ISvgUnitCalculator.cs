using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public interface ISvgUnitCalculator
  {
    bool TryAdd(SvgUnit svgUnit1,
                SvgUnit svgUnit2,
                out SvgUnit result);

    bool IsValueZero(SvgUnit svgUnit);

    bool TryGetDevicePoints(SvgUnit svgUnit,
                            int targetDpi,
                            out int devicePoints);

    void ApplyMatrixToDevicePoints(int x,
                                   int y,
                                   [NotNull] Matrix matrix,
                                   out int newX,
                                   out int newY);

    void ApplyMatrixToDevicePoints(int x1,
                                   int y1,
                                   int x2,
                                   int y2,
                                   [NotNull] Matrix matrix,
                                   out int newX1,
                                   out int newY1,
                                   out int newX2,
                                   out int newY2);
  }
}