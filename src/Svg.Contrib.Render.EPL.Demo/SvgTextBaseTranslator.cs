using System;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.EPL.Demo
{
  [PublicAPI]
  public class SvgTextBaseTranslator<T> : EPL.SvgTextBaseTranslator<T>
    where T : SvgTextBase
  {
    /// <exception cref="ArgumentNullException"><paramref name="eplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplCommands" /> is <see langword="null" />.</exception>
    public SvgTextBaseTranslator([NotNull] EPL.EplTransformer eplTransformer,
                                 [NotNull] EplCommands eplCommands)
      : base(eplTransformer,
             eplCommands)
    {
      if (eplTransformer == null)
      {
        throw new ArgumentNullException(nameof(eplTransformer));
      }
      if (eplCommands == null)
      {
        throw new ArgumentNullException(nameof(eplCommands));
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgElement" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    [Pure]
    protected override void GetPosition([NotNull] T svgElement,
                                        [NotNull] Matrix sourceMatrix,
                                        [NotNull] Matrix viewMatrix,
                                        out int horizontalStart,
                                        out int verticalStart,
                                        out int sector,
                                        out float fontSize)
    {
      if (svgElement == null)
      {
        throw new ArgumentNullException(nameof(svgElement));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }

      base.GetPosition(svgElement,
                       sourceMatrix,
                       viewMatrix,
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
