namespace System.Svg.Render.EPL
{
  public class SvgGroupTranslator : SvgElementTranslator<SvgGroup>
  {
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitCalculator" /> is <see langword="null" />.</exception>
    public SvgGroupTranslator(SvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator) {}
  }
}