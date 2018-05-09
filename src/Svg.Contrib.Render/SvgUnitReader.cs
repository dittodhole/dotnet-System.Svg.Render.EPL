using System;
using JetBrains.Annotations;

namespace Svg.Contrib.Render
{
  [PublicAPI]
  public class SvgUnitReader
  {
    /// <exception cref="ArgumentNullException"><paramref name="svgElement" /> is <see langword="null" />.</exception>
    [Pure]
    public virtual float GetValue([NotNull] SvgElement svgElement,
                                  SvgUnit svgUnit)
    {
      if (svgElement == null)
      {
        throw new ArgumentNullException(nameof(svgElement));
      }

      var result = svgUnit.Value;

      return result;
    }
  }
}
