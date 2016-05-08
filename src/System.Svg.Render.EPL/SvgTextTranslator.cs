using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgTextTranslator : SvgTextBaseTranslator<SvgText>
  {
    public SvgTextTranslator([NotNull] SvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator) {}
  }
}