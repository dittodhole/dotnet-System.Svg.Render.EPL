using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgTextTranslator : SvgTextTranslatorBase<SvgText>
  {
    public SvgTextTranslator([NotNull] SvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator) {}
  }
}