using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString

namespace Svg.Contrib.Render.EPL.Demo
{
  [PublicAPI]
  public class EplTransformer : EPL.EplTransformer
  {
    public EplTransformer([NotNull] SvgUnitReader svgUnitReader)
      : base(svgUnitReader) {}

    public override void GetFontSelection([NotNull] SvgTextBase svgTextBase,
                                          float fontSize,
                                          out int fontSelection,
                                          out int horizontalMultiplier,
                                          out int verticalMultiplier)
    {
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

    public override void Transform(SvgTextBase svgTextBase,
                                   Matrix matrix,
                                   out float startX,
                                   out float startY,
                                   out float fontSize)
    {
      base.Transform(svgTextBase,
                     matrix,
                     out startX,
                     out startY,
                     out fontSize);

      if (svgTextBase.ID == "tspan5668")
      {
        startX -= 100f;
      }
      else if (svgTextBase.ID == "tspan5670")
      {
        startX -= 100f;
      }
      else if (svgTextBase.ID == "tspan5676")
      {
        startX -= 100f;
      }
      else if (svgTextBase.ID == "tspan5682")
      {
        startX -= 100f;
      }
      else if (svgTextBase.ID == "tspan3131")
      {
        startX -= 10f;
        startY += 15f;
      }
      else if (svgTextBase.ID == "tspan5686-2")
      {
        startX += 30f;
        startY += 10f;
      }
      else if (svgTextBase.ID == "tspan5686-2-3")
      {
        startX += 30f;
        startY += 10f;
      }
      else if (svgTextBase.ID == "tspan4665")
      {
        startY -= 30f;
      }
    }
  }
}