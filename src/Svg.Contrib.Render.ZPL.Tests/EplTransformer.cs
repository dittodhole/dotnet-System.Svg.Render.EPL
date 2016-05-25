using JetBrains.Annotations;

namespace Svg.Contrib.Render.ZPL.Tests
{
  public class ZplTransformer : ZPL.ZplTransformer
  {
    public ZplTransformer([NotNull] SvgUnitReader svgUnitReader)
      : base(svgUnitReader,
             ZPL.ZplTransformer.DefaultOutputWidth,
             ZPL.ZplTransformer.DefaultOutputHeight) {}

    protected override float GetLineHeightFactor([NotNull] SvgTextBase svgTextBase) => 1f;
  }
}