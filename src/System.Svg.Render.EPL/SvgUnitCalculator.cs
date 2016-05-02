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

    public void CheckSvgUnitType(SvgUnit svgUnit1,
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