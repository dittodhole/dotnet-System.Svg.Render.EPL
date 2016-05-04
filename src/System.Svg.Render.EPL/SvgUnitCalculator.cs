namespace System.Svg.Render.EPL
{
  public class SvgUnitCalculator
  {
    // TODO add reading for different origin

    public int SourceDpi { get; set; } = 72;
    public SvgUnitType UserUnitTypeSubstitution { get; set; } = SvgUnitType.Pixel;

    public SvgUnit Add(SvgUnit svgUnit1,
                       SvgUnit svgUnit2)
    {
      var svgUnitType = this.CheckSvgUnitType(svgUnit1,
                                              svgUnit2);

      var val1 = this.GetValue(svgUnit1);
      var val2 = this.GetValue(svgUnit2);

      var result = new SvgUnit(svgUnitType,
                               val1 + val2);

      return result;
    }

    public SvgUnit Substract(SvgUnit svgUnit1,
                             SvgUnit svgUnit2)
    {
      var svgUnitType = this.CheckSvgUnitType(svgUnit1,
                                              svgUnit2);

      var val1 = this.GetValue(svgUnit1);
      var val2 = this.GetValue(svgUnit2);

      var result = new SvgUnit(svgUnitType,
                               val1 - val2);

      return result;
    }

    public SvgUnitType CheckSvgUnitType(SvgUnit svgUnit1,
                                        SvgUnit svgUnit2)
    {
      if (svgUnit1.Type != svgUnit2.Type)
      {
        // TODO add documentation
        throw new ArgumentException($"{nameof(svgUnit2)}'s {nameof(SvgUnit.Type)} ({svgUnit2.Type}) does not equal {nameof(svgUnit1)}'s {nameof(SvgUnit.Type)} ({svgUnit1.Type})");
      }

      return svgUnit1.Type;
    }

    public bool IsValueZero(SvgUnit svgUnit)
    {
      // TODO find a good TOLERANCE
      return Math.Abs(svgUnit.Value) < 0.5f;
    }

    public float GetValue(SvgUnit svgUnit)
    {
      var result = svgUnit.Value;

      return result;
    }

    public int GetDevicePoints(SvgUnit svgUnit,
                               int targetDpi)
    {
      var type = svgUnit.Type;
      if (type == SvgUnitType.User)
      {
        type = this.UserUnitTypeSubstitution;
      }

      float? inches;
      if (type == SvgUnitType.Inch)
      {
        inches = svgUnit.Value;
      }
      else if (type == SvgUnitType.Centimeter)
      {
        inches = svgUnit.Value / 2.54f;
      }
      else if (type == SvgUnitType.Millimeter)
      {
        inches = svgUnit.Value / 10f / 2.54f;
      }
      else if (type == SvgUnitType.Point)
      {
        inches = svgUnit.Value / 72f;
      }
      else if (type == SvgUnitType.Pica)
      {
        inches = svgUnit.Value / 10f / 72f;
      }
      else
      {
        inches = null;
      }

      float pixels;
      if (type == SvgUnitType.Pixel)
      {
        pixels = svgUnit.Value;
      }
      else if (inches.HasValue)
      {
        pixels = inches.Value * this.SourceDpi;
      }
      else
      {
        // TODO add documentation
        throw new NotImplementedException($"a conversion for {nameof(svgUnit)}'s {nameof(SvgUnit.Type)} value set is currently not implemented: {type}");
      }

      var devicePoints = (int) (pixels / this.SourceDpi * targetDpi);

      return devicePoints;
    }
  }
}