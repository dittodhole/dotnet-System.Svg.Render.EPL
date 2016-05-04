using System.Drawing.Drawing2D;

namespace System.Svg.Render.EPL
{
  public class SvgGroupTranslator : SvgElementTranslator<SvgGroup>
  {
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitCalculator" /> is <see langword="null" />.</exception>
    public SvgGroupTranslator(SvgUnitCalculator svgUnitCalculator)
    {
      if (svgUnitCalculator == null)
      {
        throw new ArgumentNullException(nameof(svgUnitCalculator));
      }

      this.SvgUnitCalculator = svgUnitCalculator;
    }

    private SvgUnitCalculator SvgUnitCalculator { get; }

    public override object Translate(SvgGroup instance,
                                     Matrix matrix,
                                     int targetDpi)
    {
      var newMatrix = matrix.Clone();
      this.SvgUnitCalculator.ApplyTransformationsToMatrix(instance,
                                                          newMatrix);

      return null;
    }
  }
}