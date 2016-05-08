using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgTextSpanTranslator : SvgTextBaseTranslator<SvgTextSpan>
  {
    public SvgTextSpanTranslator([NotNull] SvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator) {}
  }
}