using JetBrains.Annotations;

namespace System.Svg.Render
{
  public abstract class SvgTextTranslatorBase<T> : SvgElementTranslatorBase<T>
    where T : SvgTextBase
  {
    protected SvgTextTranslatorBase([NotNull] SvgUnitCalculatorBase svgUnitCalculator)
      : base(svgUnitCalculator) {}
  }
}