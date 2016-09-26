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
    /// <exception cref="ArgumentNullException"><paramref name="zplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplCommands" /> is <see langword="null" />.</exception>
    public SvgLineTranslator([NotNull] ZplTransformer zplTransformer,
                             [NotNull] ZplCommands zplCommands)
    {
      if (zplTransformer == null)
      {
        throw new ArgumentNullException(nameof(zplTransformer));
      }
      if (zplCommands == null)
      {
        throw new ArgumentNullException(nameof(zplCommands));
      }
      this.ZplTransformer = zplTransformer;
      this.ZplCommands = zplCommands;
    }

    [NotNull]
    protected ZplTransformer ZplTransformer { get; }

    [NotNull]
    protected ZplCommands ZplCommands { get; }

    /// <exception cref="ArgumentNullException"><paramref name="svgLine" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplContainer" /> is <see langword="null" />.</exception>
    public override void Translate([NotNull] SvgLine svgLine,
                                   [NotNull] Matrix sourceMatrix,
                                   [NotNull] Matrix viewMatrix,
                                   [NotNull] ZplContainer zplContainer)
    {
      if (svgLine == null)
      {
        throw new ArgumentNullException(nameof(svgLine));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }
      if (zplContainer == null)
      {
        throw new ArgumentNullException(nameof(zplContainer));
      }

      int horizontalStart;
      int verticalStart;
      int width;
      int height;
      int verticalEnd;
      float strokeWidth;
      this.GetPosition(svgLine,
                       sourceMatrix,
                       viewMatrix,
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
        var strokeShouldBeWhite = (svgLine.Stroke as SvgColourServer)?.Colour == Color.White;
        if (strokeShouldBeWhite)
        {
          lineColor = LineColor.White;
        }
        else
        {
          lineColor = LineColor.Black;
        }

        var thickness = (int) strokeWidth;

        zplContainer.Body.Add(this.ZplCommands.FieldTypeset(horizontalStart,
                                                            verticalStart));
        zplContainer.Body.Add(this.ZplCommands.GraphicBox(width,
                                                          height,
                                                          thickness,
                                                          lineColor));
      }
      else
      {
        throw new NotImplementedException();
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgLine" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    [Pure]
    protected virtual void GetPosition([NotNull] SvgLine svgLine,
                                       [NotNull] Matrix sourceMatrix,
                                       [NotNull] Matrix viewMatrix,
                                       out int horizontalStart,
                                       out int verticalStart,
                                       out int width,
                                       out int height,
                                       out int verticalEnd,
                                       out float strokeWidth)
    {
      if (svgLine == null)
      {
        throw new ArgumentNullException(nameof(svgLine));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }

      float startX;
      float startY;
      float endX;
      float endY;
      this.ZplTransformer.Transform(svgLine,
                                    sourceMatrix,
                                    viewMatrix,
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
