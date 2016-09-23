using System;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString

namespace Svg.Contrib.Render.EPL.Demo
{
  [PublicAPI]
  public class EplTransformer : EPL.EplTransformer
  {
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitReader" /> is <see langword="null" />.</exception>
    public EplTransformer([NotNull] SvgUnitReader svgUnitReader)
      : base(svgUnitReader)
    {
      if (svgUnitReader == null)
      {
        throw new ArgumentNullException(nameof(svgUnitReader));
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgTextBase" /> is <see langword="null" />.</exception>
    [Pure]
    public override void GetFontSelection([NotNull] SvgTextBase svgTextBase,
                                          float fontSize,
                                          out int fontSelection,
                                          out int horizontalMultiplier,
                                          out int verticalMultiplier)
    {
      if (svgTextBase == null)
      {
        throw new ArgumentNullException(nameof(svgTextBase));
      }

      if (svgTextBase.ID == "tspan5668")
      {
        fontSelection = 1;
        horizontalMultiplier = 1;
        verticalMultiplier = 1;
      }
      else if (svgTextBase.ID == "tspan5670")
      {
        fontSelection = 1;
        horizontalMultiplier = 1;
        verticalMultiplier = 1;
      }
      else if (svgTextBase.ID == "tspan5676")
      {
        fontSelection = 1;
        horizontalMultiplier = 1;
        verticalMultiplier = 1;
      }
      else if (svgTextBase.ID == "tspan5682")
      {
        fontSelection = 1;
        horizontalMultiplier = 1;
        verticalMultiplier = 1;
      }
      else if (svgTextBase.ID == "tspan3131")
      {
        fontSelection = 2;
        horizontalMultiplier = 1;
        verticalMultiplier = 1;
      }
      else if (svgTextBase.ID == "tspan5686-2")
      {
        fontSelection = 3;
        horizontalMultiplier = 1;
        verticalMultiplier = 1;
      }
      else if (svgTextBase.ID == "tspan5686-2-3")
      {
        fontSelection = 3;
        horizontalMultiplier = 1;
        verticalMultiplier = 1;
      }
      else
      {
        base.GetFontSelection(svgTextBase,
                              fontSize,
                              out fontSelection,
                              out horizontalMultiplier,
                              out verticalMultiplier);
      }
    }
  }
}
