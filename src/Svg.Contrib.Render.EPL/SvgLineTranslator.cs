using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class SvgLineTranslator : SvgElementTranslatorBase<EplContainer, SvgLine>
  {
    /// <exception cref="ArgumentNullException"><paramref name="eplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplCommands" /> is <see langword="null" />.</exception>
    public SvgLineTranslator([NotNull] EplTransformer eplTransformer,
                             [NotNull] EplCommands eplCommands)
    {
      this.EplTransformer = eplTransformer ?? throw new ArgumentNullException(nameof(eplTransformer));
      this.EplCommands = eplCommands ?? throw new ArgumentNullException(nameof(eplCommands));
    }

    [NotNull]
    private EplTransformer EplTransformer { get; }

    [NotNull]
    private EplCommands EplCommands { get; }

    /// <exception cref="ArgumentNullException"><paramref name="svgLine" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplContainer" /> is <see langword="null" />.</exception>
    public override void Translate(SvgLine svgLine,
                                   Matrix sourceMatrix,
                                   Matrix viewMatrix,
                                   EplContainer eplContainer)

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
      if (eplContainer == null)
      {
        throw new ArgumentNullException(nameof(eplContainer));
      }

      this.GetPosition(svgLine,
                       sourceMatrix,
                       viewMatrix,
                       out var horizontalStart,
                       out var verticalStart,
                       out var horizontalLength,
                       out var verticalLength,
                       out var verticalEnd,
                       out var strokeWidth);

      this.AddTranslationToContainer(svgLine,
                                     horizontalStart,
                                     verticalStart,
                                     verticalEnd,
                                     horizontalLength,
                                     verticalLength,
                                     strokeWidth,
                                     eplContainer);
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
                                       out int horizontalLength,
                                       out int verticalLength,
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

      this.EplTransformer.Transform(svgLine,
                                    sourceMatrix,
                                    viewMatrix,
                                    out var startX,
                                    out var startY,
                                    out var endX,
                                    out var endY,
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

    /// <exception cref="ArgumentNullException"><paramref name="svgLine" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplContainer" /> is <see langword="null" />.</exception>
    protected virtual void AddTranslationToContainer([NotNull] SvgLine svgLine,
                                                     int horizontalStart,
                                                     int verticalStart,
                                                     int verticalEnd,
                                                     int horizontalLength,
                                                     int verticalLength,
                                                     float strokeWidth,
                                                     [NotNull] EplContainer eplContainer)
    {
      if (svgLine == null)
      {
        throw new ArgumentNullException(nameof(svgLine));
      }
      if (eplContainer == null)
      {
        throw new ArgumentNullException(nameof(eplContainer));
      }

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

        var strokeShouldBeWhite = (svgLine.Stroke as SvgColourServer)?.Colour == Color.White;
        if (strokeShouldBeWhite)
        {
          eplContainer.Body.Add(this.EplCommands.LineDrawWhite(horizontalStart,
                                                               verticalStart,
                                                               horizontalLength,
                                                               verticalLength));
        }
        else
        {
          eplContainer.Body.Add(this.EplCommands.LineDrawBlack(horizontalStart,
                                                               verticalStart,
                                                               horizontalLength,
                                                               verticalLength));
        }
      }
      else
      {
        eplContainer.Body.Add(this.EplCommands.LineDrawDiagonal(horizontalStart,
                                                                verticalStart,
                                                                horizontalLength,
                                                                verticalLength,
                                                                verticalEnd));
      }
    }
  }
}
