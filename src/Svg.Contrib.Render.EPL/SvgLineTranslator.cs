using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

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
                                   [NotNull] EplContainer container)

    {
      int horizontalStart;
      int verticalStart;
      int horizontalLength;
      int verticalLength;
      int verticalEnd;
      float strokeWidth;
      this.GetPosition(svgElement,
                       matrix,
                       out horizontalStart,
                       out verticalStart,
                       out horizontalLength,
                       out verticalLength,
                       out verticalEnd,
                       out strokeWidth);

      this.AddTranslationToContainer(svgElement,
                                     horizontalStart,
                                     verticalStart,
                                     verticalEnd,
                                     horizontalLength,
                                     verticalLength,
                                     strokeWidth,
                                     container);
    }

    protected virtual void GetPosition([NotNull] SvgLine svgElement,
                                       [NotNull] Matrix matrix,
                                       out int horizontalStart,
                                       out int verticalStart,
                                       out int horizontalLength,
                                       out int verticalLength,
                                       out int verticalEnd,
                                       out float strokeWidth)
    {
      float startX;
      float startY;
      float endX;
      float endY;
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
        horizontalStart = (int) startX;
        verticalStart = (int) startY;
        horizontalLength = (int) (endX - startX);
        verticalLength = (int) (endY - startY);
        verticalEnd = (int) endY;
      }
      else
      {
        horizontalStart = (int) startX;
        verticalStart = (int) startY;
        horizontalLength = (int) strokeWidth;
        verticalLength = (int) endX;
        verticalEnd = (int) endY;
      }
    }

    protected virtual void AddTranslationToContainer([NotNull] SvgLine svgElement,
                                                     int horizontalStart,
                                                     int verticalStart,
                                                     int verticalEnd,
                                                     int horizontalLength,
                                                     int verticalLength,
                                                     float strokeWidth,
                                                     [NotNull] EplContainer container)
    {
      if (horizontalLength == 0
          || verticalLength == 0)
      {
        if (horizontalLength == 0)
        {
          horizontalLength = (int) strokeWidth;
        }
        if (verticalLength == 0)
        {
          verticalLength = (int) strokeWidth;
        }

        var strokeShouldBeWhite = (svgElement.Stroke as SvgColourServer)?.Colour == Color.White;
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
        container.Body.Add(this.EplCommands.LineDrawDiagonal(horizontalStart,
                                                             verticalStart,
                                                             horizontalLength,
                                                             verticalLength,
                                                             verticalEnd));
      }
    }
  }
}