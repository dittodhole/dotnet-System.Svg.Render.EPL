using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.EPL.Demo
{
  [PublicAPI]
  public class SvgTextBaseTranslator<T> : EPL.SvgTextBaseTranslator<T>
    where T : SvgTextBase
  {
    public SvgTextBaseTranslator([NotNull] EPL.EplTransformer eplTransformer,
                                 [NotNull] EplCommands eplCommands)
      : base(eplTransformer,
             eplCommands) {}

    [Pure]
    protected override void GetPosition([NotNull] T svgElement,
                                        [NotNull] Matrix matrix,
                                        out int horizontalStart,
                                        out int verticalStart,
                                        out int sector,
                                        out float fontSize)
    {
      base.GetPosition(svgElement,
                       matrix,
                       out horizontalStart,
                       out verticalStart,
                       out sector,
                       out fontSize);

      if (svgElement.ID == "tspan5668")
      {
        horizontalStart -= 100;
      }
      else if (svgElement.ID == "tspan5670")
      {
        horizontalStart -= 100;
      }
      else if (svgElement.ID == "tspan5676")
      {
        horizontalStart -= 100;
      }
      else if (svgElement.ID == "tspan5682")
      {
        horizontalStart -= 100;
      }
      else if (svgElement.ID == "tspan3131")
      {
        horizontalStart -= 10;
        verticalStart += 15;
      }
      else if (svgElement.ID == "tspan5686-2")
      {
        horizontalStart += 30;
        verticalStart += 10;
      }
      else if (svgElement.ID == "tspan5686-2-3")
      {
        horizontalStart += 30;
        verticalStart += 10;
      }
      else if (svgElement.ID == "tspan4665")
      {
        verticalStart -= 30;
      }
    }
  }
}