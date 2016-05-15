using JetBrains.Annotations;

namespace System.Svg.Render.EPL.Tests
{
  public class EplTransformer : EPL.EplTransformer
  {
    public EplTransformer([NotNull] SvgUnitReader svgUnitReader,
                          PrintDirection printDirection)
      : base(svgUnitReader,
             printDirection,
             EPL.EplTransformer.DefaultLabelWidthInDevicePoints,
             EPL.EplTransformer.DefaultLabelHeightInDevicePoints) {}

    protected override float GetLineHeightFactor([NotNull] SvgTextBase svgTextBase)
    {
      return 1f;
    }
  }
}