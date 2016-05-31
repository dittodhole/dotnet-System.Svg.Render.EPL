using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.FingerPrint.Demo
{
  [PublicAPI]
  public class SvgTextBaseTranslator<T> : FingerPrint.SvgTextBaseTranslator<T>
    where T : SvgTextBase
  {
    public SvgTextBaseTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                 [NotNull] FingerPrintCommands fingerPrintCommands)
      : base(fingerPrintTransformer,
             fingerPrintCommands) {}

    protected override void GetPosition([NotNull] T svgElement,
                                        [NotNull] Matrix matrix,
                                        out int horizontalStart,
                                        out int verticalStart,
                                        out float fontSize,
                                        out Direction direction)
    {
      base.GetPosition(svgElement,
                       matrix,
                       out horizontalStart,
                       out verticalStart,
                       out fontSize,
                       out direction);

      if (svgElement.ID == "tspan5668")
      {
        horizontalStart += 50;
      }
      else if (svgElement.ID == "tspan5670")
      {
        horizontalStart += 50;
      }
      else if (svgElement.ID == "tspan5676")
      {
        horizontalStart += 50;
      }
      else if (svgElement.ID == "tspan5682")
      {
        horizontalStart += 50;
      }
      else if (svgElement.ID == "tspan4657")
      {
        verticalStart -= 10;
      }
      else if (svgElement.ID == "tspan4665")
      {
        verticalStart -= 15;
      }
    }
  }
}