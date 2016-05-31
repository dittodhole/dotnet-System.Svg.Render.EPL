using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public class SvgLineTranslator : SvgElementTranslatorBase<ZplContainer, SvgLine>
  {
    public SvgLineTranslator([NotNull] ZplTransformer zplTransformer,
                             [NotNull] ZplCommands zplCommands)
    {
      this.ZplTransformer = zplTransformer;
      this.ZplCommands = zplCommands;
    }

    [NotNull]
    protected ZplTransformer ZplTransformer { get; }

    [NotNull]
    protected ZplCommands ZplCommands { get; }

    public override void Translate([NotNull] SvgLine svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] ZplContainer container)
    {
      int horizontalStart;
      int verticalStart;
      int width;
      int height;
      int verticalEnd;
      float strokeWidth;
      this.GetPosition(svgElement,
                       matrix,
                       out horizontalStart,
                       out verticalStart,
                       out width,
                       out height,
                       out verticalEnd,
                       out strokeWidth);

      if (width == 0
          || height == 0)
      {
        LineColor lineColor;
        var strokeShouldBeWhite = (svgElement.Stroke as SvgColourServer)?.Colour == Color.White;
        if (strokeShouldBeWhite)
        {
          lineColor = LineColor.White;
        }
        else
        {
          lineColor = LineColor.Black;
        }

        var thickness = (int) strokeWidth;

        container.Body.Add(this.ZplCommands.FieldTypeset(horizontalStart,
                                                         verticalStart));
        container.Body.Add(this.ZplCommands.GraphicBox(width,
                                                       height,
                                                       thickness,
                                                       lineColor));
      }
      else
      {
        throw new NotImplementedException();
      }
    }

    [Pure]
    protected virtual void GetPosition([NotNull] SvgLine svgElement,
                                       [NotNull] Matrix matrix,
                                       out int horizontalStart,
                                       out int verticalStart,
                                       out int width,
                                       out int height,
                                       out int verticalEnd,
                                       out float strokeWidth)
    {
      float startX;
      float startY;
      float endX;
      float endY;
      this.ZplTransformer.Transform(svgElement,
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
        width = (int) (endX - startX);
        height = (int) (endY - startY);
        verticalEnd = (int) endY;
      }
      else
      {
        throw new NotImplementedException();
        horizontalStart = (int) startX;
        verticalStart = (int) startY;
        width = (int) strokeWidth;
        height = (int) endX;
        verticalEnd = (int) endY;
      }
    }
  }
}