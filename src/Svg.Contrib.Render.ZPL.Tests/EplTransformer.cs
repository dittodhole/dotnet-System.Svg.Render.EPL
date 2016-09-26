using System;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.ZPL.Tests
{
  public class ZplTransformer : ZPL.ZplTransformer
  {
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitReader" /> is <see langword="null" />.</exception>
    public ZplTransformer([NotNull] SvgUnitReader svgUnitReader)
      : base(svgUnitReader,
             ZPL.ZplTransformer.DefaultOutputWidth,
             ZPL.ZplTransformer.DefaultOutputHeight)
    {
      if (svgUnitReader == null)
      {
        throw new ArgumentNullException(nameof(svgUnitReader));
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgTextBase" /> is <see langword="null" />.</exception>
    protected override float GetLineHeightFactor([NotNull] SvgTextBase svgTextBase)
    {
      if (svgTextBase == null)
      {
        throw new ArgumentNullException(nameof(svgTextBase));
      }

      return 1f;
    }
  }
}
