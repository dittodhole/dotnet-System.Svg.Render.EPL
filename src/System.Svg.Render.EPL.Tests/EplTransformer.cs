using JetBrains.Annotations;

namespace System.Svg.Render.EPL.Tests
{
  public class EplTransformer : EPL.EplTransformer
  {
    public EplTransformer([NotNull] SvgUnitReader svgUnitReader)
      : base(svgUnitReader,
             EPL.EplTransformer.DefaultLabelWidthInDevicePoints,
             EPL.EplTransformer.DefaultLabelHeightInDevicePoints) {}

    protected override float GetLineHeightFactor([NotNull] SvgTextBase svgTextBase) => 1f;
  }
}