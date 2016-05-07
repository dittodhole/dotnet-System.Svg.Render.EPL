namespace System.Svg.Render.Tests
{
  public class SvgUnitCalculator : SvgUnitCalculatorBase
  {
    protected override bool IsTransformationAllowed(ISvgTransformable svgTransformable,
                                                    Type type)
    {
      return false;
    }
  }
}