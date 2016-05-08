using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgTextSpanTranslator : SvgTextTranslatorBase<SvgTextSpan>
  {
    public SvgTextSpanTranslator([NotNull] SvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator) {}
  }
}