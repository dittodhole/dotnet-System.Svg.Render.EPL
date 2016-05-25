using System.Drawing.Drawing2D;
using System.Svg;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString

namespace Svg.Contrib.Render.ZPL.Demo
{
  [PublicAPI]
  public class ZplTransformer : ZPL.ZplTransformer
  {
    public ZplTransformer([NotNull] SvgUnitReader svgUnitReader)
      : base(svgUnitReader) {}

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

      //if (svgTextBase.ID == "tspan5668")
      //{
      //  startX -= 100f;
      //}
      //else if (svgTextBase.ID == "tspan5670")
      //{
      //  startX -= 100f;
      //}
      //else if (svgTextBase.ID == "tspan5676")
      //{
      //  startX -= 100f;
      //}
      //else if (svgTextBase.ID == "tspan5682")
      //{
      //  startX -= 100f;
      //}
      //else if (svgTextBase.ID == "tspan3131")
      //{
      //  startX -= 10f;
      //  startY += 15f;
      //}
      //else if (svgTextBase.ID == "tspan5686-2")
      //{
      //  startX += 30f;
      //  startY += 10f;
      //}
      //else if (svgTextBase.ID == "tspan5686-2-3")
      //{
      //  startX += 30f;
      //  startY += 10f;
      //}
      //else if (svgTextBase.ID == "tspan4665")
      //{
      //  startY -= 30f;
      //}
    }
  }
}