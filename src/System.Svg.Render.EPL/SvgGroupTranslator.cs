using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgGroupTranslator : SvgElementTranslatorBase<SvgGroup>
  {
    public SvgGroupTranslator([NotNull] ISvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator) {}
  }
}