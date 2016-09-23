using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class SvgRectangleTranslator : SvgElementTranslatorBase<EplContainer, SvgRectangle>
  {
    /// <exception cref="ArgumentNullException"><paramref name="eplTransformer"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplCommands"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitReader"/> is <see langword="null" />.</exception>
    public SvgRectangleTranslator([NotNull] EplTransformer eplTransformer,
                                  [NotNull] EplCommands eplCommands,
                                  [NotNull] SvgUnitReader svgUnitReader)
    {
      if (eplTransformer == null)
      {
        throw new ArgumentNullException(nameof(eplTransformer));
      }
      if (eplCommands == null)
      {
        throw new ArgumentNullException(nameof(eplCommands));
      }
      if (svgUnitReader == null)
      {
        throw new ArgumentNullException(nameof(svgUnitReader));
      }
      this.EplTransformer = eplTransformer;
      this.EplCommands = eplCommands;
      this.SvgUnitReader = svgUnitReader;
    }

    [NotNull]
    protected EplTransformer EplTransformer { get; }

    [NotNull]
    protected EplCommands EplCommands { get; }

    [NotNull]
    protected SvgUnitReader SvgUnitReader { get; }

    /// <exception cref="ArgumentNullException"><paramref name="svgElement"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container"/> is <see langword="null" />.</exception>
    public override void Translate([NotNull] SvgRectangle svgElement,
                                   [NotNull] Matrix sourceMatrix,
                                   [NotNull] Matrix viewMatrix,
                                   [NotNull] EplContainer container)
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
      if (container == null)
      {
        throw new ArgumentNullException(nameof(container));
      }

      if (svgElement.Fill != SvgPaintServer.None
          && (svgElement.Fill as SvgColourServer)?.Colour != Color.White)
      {
        this.TranslateFilledBox(svgElement,
                                sourceMatrix,
                                viewMatrix,
                                container);
      }

      if (svgElement.Stroke != SvgPaintServer.None)
      {
        this.TranslateBox(svgElement,
                          sourceMatrix,
                          viewMatrix,
                          container);
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplContainer"/> is <see langword="null" />.</exception>
    protected virtual void TranslateFilledBox([NotNull] SvgRectangle svgRectangle,
                                              [NotNull] Matrix sourceMatrix,
                                              [NotNull] Matrix viewMatrix,
                                              [NotNull] EplContainer eplContainer)
    {
      if (svgRectangle == null)
      {
        throw new ArgumentNullException(nameof(svgRectangle));
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

      int horizontalStart;
      int verticalStart;
      int lineThickness;
      int horizontalEnd;
      int verticalEnd;
      this.GetPosition(svgRectangle,
                       sourceMatrix,
                       viewMatrix,
                       out horizontalStart,
                       out verticalStart,
                       out lineThickness,
                       out horizontalEnd,
                       out verticalEnd);

      var horizontalLength = horizontalEnd - horizontalStart;
      var verticalLength = verticalEnd - verticalStart;

      eplContainer.Body.Add(this.EplCommands.LineDrawBlack(horizontalStart,
                                                        verticalStart,
                                                        horizontalLength,
                                                        verticalLength));
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplContainer"/> is <see langword="null" />.</exception>
    protected virtual void TranslateBox([NotNull] SvgRectangle svgRectangle,
                                        [NotNull] Matrix sourceMatrix,
                                        [NotNull] Matrix viewMatrix,
                                        [NotNull] EplContainer eplContainer)
    {
      if (svgRectangle == null)
      {
        throw new ArgumentNullException(nameof(svgRectangle));
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

      int horizontalStart;
      int verticalStart;
      int lineThickness;
      int horizontalEnd;
      int verticalEnd;
      this.GetPosition(svgRectangle,
                       sourceMatrix,
                       viewMatrix,
                       out horizontalStart,
                       out verticalStart,
                       out lineThickness,
                       out horizontalEnd,
                       out verticalEnd);

      eplContainer.Body.Add(this.EplCommands.DrawBox(horizontalStart,
                                                  verticalStart,
                                                  lineThickness,
                                                  horizontalEnd,
                                                  verticalEnd));
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix"/> is <see langword="null" />.</exception>
    [Pure]
    protected virtual void GetPosition([NotNull] SvgRectangle svgRectangle,
                                       [NotNull] Matrix sourceMatrix,
                                       [NotNull] Matrix viewMatrix,
                                       out int horizontalStart,
                                       out int verticalStart,
                                       out int lineThickness,
                                       out int horizontalEnd,
                                       out int verticalEnd)
    {
      if (svgRectangle == null)
      {
        throw new ArgumentNullException(nameof(svgRectangle));
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
      float endX;
      float startY;
      float endY;
      float strokeWidth;
      this.EplTransformer.Transform(svgRectangle,
                                    sourceMatrix,
                                    viewMatrix,
                                    out startX,
                                    out startY,
                                    out endX,
                                    out endY,
                                    out strokeWidth);

      horizontalStart = (int) startX;
      verticalStart = (int) startY;
      lineThickness = (int) strokeWidth;
      horizontalEnd = (int) endX;
      verticalEnd = (int) endY;
    }
  }
}