using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using Svg;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class SvgLineTranslator : SvgElementTranslatorBase<SvgLine>
  {
    public SvgLineTranslator([NotNull] EplTransformer eplTransformer,
                             [NotNull] EplCommands eplCommands)
    {
      this.EplTransformer = eplTransformer;
      this.EplCommands = eplCommands;
    }

    [NotNull]
    protected EplTransformer EplTransformer { get; }

    [NotNull]
    protected EplCommands EplCommands { get; }

    public override void Translate([NotNull] SvgLine svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] Container<EplStream> container)

    {
      float startX;
      float startY;
      float endX;
      float endY;
      float strokeWidth;
      this.EplTransformer.Transform(svgElement,
                                    matrix,
                                    out startX,
                                    out startY,
                                    out endX,
                                    out endY,
                                    out strokeWidth);

      // TODO find a good TOLERANCE
      if (Math.Abs(startY - endY) < 0.5f
          || Math.Abs(startX - endX) < 0.5f)
      {
        var strokeShouldBeWhite = (svgElement.Stroke as SvgColourServer)?.Colour == Color.White;
        var horizontalStart = (int) startX;
        var verticalStart = (int) startY;
        var horizontalLength = (int) (endX - startX);
        if (horizontalLength == 0)
        {
          horizontalLength = (int) strokeWidth;
        }
        var verticalLength = (int) (endY - startY);
        if (verticalLength == 0)
        {
          verticalLength = (int) strokeWidth;
        }

        if (strokeShouldBeWhite)
        {
          container.Body.Add(this.EplCommands.LineDrawWhite(horizontalStart,
                                                            verticalStart,
                                                            horizontalLength,
                                                            verticalLength));
        }
        else
        {
          container.Body.Add(this.EplCommands.LineDrawBlack(horizontalStart,
                                                            verticalStart,
                                                            horizontalLength,
                                                            verticalLength));
        }
      }
      else
      {
        var horizontalStart = (int) startX;
        var verticalStart = (int) startY;
        var horizontalLength = (int) strokeWidth;
        var verticalLength = (int) endX;
        var verticalEnd = (int) endY;

        container.Body.Add(this.EplCommands.LineDrawDiagonal(horizontalStart,
                                                             verticalStart,
                                                             horizontalLength,
                                                             verticalLength,
                                                             verticalEnd));
      }
    }
  }
}