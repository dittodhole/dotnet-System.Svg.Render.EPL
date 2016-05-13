using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public class Transformer
  {
    public Transformer([NotNull] ISvgUnitCalculator svgUnitCalculator)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
    }

    [NotNull]
    private ISvgUnitCalculator SvgUnitCalculator { get; }

    public void Transform([NotNull] SvgLine svgLine,
                          [NotNull] Matrix matrix,
                          out float startX,
                          out float startY,
                          out float endX,
                          out float endY,
                          out float strokeWidth)
    {
      startX = this.SvgUnitCalculator.GetValue(svgLine.StartX);
      startY = this.SvgUnitCalculator.GetValue(svgLine.StartY);
      endX = this.SvgUnitCalculator.GetValue(svgLine.EndX);
      endY = this.SvgUnitCalculator.GetValue(svgLine.EndY);
      strokeWidth = this.SvgUnitCalculator.GetValue(svgLine.StrokeWidth);

      this.SvgUnitCalculator.ApplyMatrix(startX,
                                         startY,
                                         matrix,
                                         out startX,
                                         out startY);

      this.SvgUnitCalculator.ApplyMatrix(endX,
                                         endY,
                                         matrix,
                                         out endX,
                                         out endY);

      this.SvgUnitCalculator.ApplyMatrix(strokeWidth,
                                         matrix,
                                         out strokeWidth);
    }
  }
}