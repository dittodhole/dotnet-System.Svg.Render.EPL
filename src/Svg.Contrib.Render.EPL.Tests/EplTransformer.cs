using System;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.EPL.Tests
{
  public class EplTransformer : EPL.EplTransformer
  {
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitReader" /> is <see langword="null" />.</exception>
    public EplTransformer([NotNull] SvgUnitReader svgUnitReader)
      : base(svgUnitReader,
             EPL.EplTransformer.DefaultOutputWidth,
             EPL.EplTransformer.DefaultOutputHeight)
    {
      if (svgUnitReader == null)
      {
        throw new ArgumentNullException(nameof(svgUnitReader));
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgTextBase"/> is <see langword="null" />.</exception>
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
