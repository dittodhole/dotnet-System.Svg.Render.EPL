using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public abstract class SvgElementTranslator<T> : SvgElementTranslatorBase<T>
    where T : SvgElement
  {
    protected SvgElementTranslator([NotNull] SvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
    }

    [NotNull]
    protected SvgUnitCalculator SvgUnitCalculator { get; }
  }
}