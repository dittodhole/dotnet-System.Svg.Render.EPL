using System;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.FingerPrint.Demo
{
  [PublicAPI]
  public class SvgTextBaseTranslator<T> : FingerPrint.SvgTextBaseTranslator<T>
    where T : SvgTextBase
  {
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintTransformer"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintCommands"/> is <see langword="null" />.</exception>
    public SvgTextBaseTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                 [NotNull] FingerPrintCommands fingerPrintCommands)
      : base(fingerPrintTransformer,
             fingerPrintCommands)
    {
      if (fingerPrintTransformer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintTransformer));
      }
      if (fingerPrintCommands == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintCommands));
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgElement"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix"/> is <see langword="null" />.</exception>
    [Pure]
    protected override void GetPosition([NotNull] T svgElement,
                                        [NotNull] Matrix sourceMatrix,
                                        [NotNull] Matrix viewMatrix,
                                        out int horizontalStart,
                                        out int verticalStart,
                                        out float fontSize,
                                        out Direction direction)
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