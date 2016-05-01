namespace System.Svg.Render.EPL
{
  public class SvgUnitCalculator
  {
    public SvgUnit Add(SvgUnit svgUnit1,
                       SvgUnit svgUnit2)
    {
      this.CheckSvgUnitType(svgUnit1,
                            svgUnit2);

      var result = new SvgUnit(svgUnit1.Type,
                               svgUnit1.Value + svgUnit2.Value);

      return result;
    }

    public SvgUnit Substract(SvgUnit svgUnit1,
                             SvgUnit svgUnit2)
    {
      this.CheckSvgUnitType(svgUnit1,
                            svgUnit2);

      var result = new SvgUnit(svgUnit1.Type,
                               svgUnit1.Value - svgUnit2.Value);

      return result;
    }

    private void CheckSvgUnitType(SvgUnit svgUnit1,
                                  SvgUnit svgUnit2)
    {
      if (svgUnit1.Type != svgUnit2.Type)
      {
        // TODO add documentation
        throw new ArgumentException($"{nameof(svgUnit2)}'s {nameof(SvgUnit.Type)} ({svgUnit2.Type}) does not equal {nameof(svgUnit1)}'s {nameof(SvgUnit.Type)} ({svgUnit1.Type})");
      }
    }

    public int GetValue(SvgUnit svgUnit)
    {
      // TODO implement device-specific getting of .Value

      var result = (int) svgUnit.Value;

      return result;
    }
  }
}