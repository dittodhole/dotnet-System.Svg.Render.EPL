using JetBrains.Annotations;

namespace System.Svg.Render
{
  public class SvgUnitReader
  {
    public virtual float GetValue([NotNull] SvgElement svgElement,
                                  SvgUnit svgUnit)
    {
      var result = svgUnit.Value;

      return result;
    }
  }
}