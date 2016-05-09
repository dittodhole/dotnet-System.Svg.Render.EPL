using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public interface ISvgUnitCalculator
  {
    bool TryAdd(SvgUnit svgUnit1,
                SvgUnit svgUnit2,
                out SvgUnit result);

    bool TryGetDevicePoints(SvgUnit svgUnit,
                            int targetDpi,
                            out int devicePoints);

    void ApplyMatrixToDevicePoints(int x,
                                   int y,
                                   [NotNull] Matrix matrix,
                                   out int newX,
                                   out int newY);
  }
}