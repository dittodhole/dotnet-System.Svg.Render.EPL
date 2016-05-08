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

    bool TryApplyMatrix(SvgUnit x,
                        SvgUnit y,
                        [NotNull] Matrix matrix,
                        out SvgUnit newX,
                        out SvgUnit newY);

    bool TryApplyMatrix(SvgUnit x1,
                        SvgUnit y1,
                        SvgUnit x2,
                        SvgUnit y2,
                        [NotNull] Matrix matrix,
                        out SvgUnit newX1,
                        out SvgUnit newY1,
                        out SvgUnit newX2,
                        out SvgUnit newY2);
  }
}