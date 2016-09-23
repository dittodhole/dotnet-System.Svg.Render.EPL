using JetBrains.Annotations;

// ReSharper disable VirtualMemberNeverOverriden.Global

namespace Svg.Contrib.Render
{
  [PublicAPI]
  public class SvgUnitReader
  {
    [Pure]
    public virtual float GetValue([NotNull] SvgElement svgElement,
                                  SvgUnit svgUnit)
    {
      var result = svgUnit.Value;

      return result;
    }
  }
}