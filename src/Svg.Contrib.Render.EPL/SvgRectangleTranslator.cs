using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class SvgRectangleTranslator : SvgElementTranslatorBase<EplContainer, SvgRectangle>
  {
    public SvgRectangleTranslator([NotNull] EplTransformer eplTransformer,
                                  [NotNull] EplCommands eplCommands,
                                  [NotNull] SvgUnitReader svgUnitReader)
    {
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

    public override void Translate([NotNull] SvgRectangle svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] EplContainer container)
    {
      if (svgElement.Fill != SvgPaintServer.None
          && (svgElement.Fill as SvgColourServer)?.Colour != Color.White)
      {
        this.TranslateFilledBox(svgElement,
                                matrix,
                                container);
      }

      if (svgElement.Stroke != SvgPaintServer.None)
      {
        this.TranslateBox(svgElement,
                          matrix,
                          container);
      }
    }

    protected virtual void TranslateFilledBox([NotNull] SvgRectangle instance,
                                              [NotNull] Matrix matrix,
                                              [NotNull] EplContainer container)
    {
      int horizontalStart;
      int verticalStart;
      int lineThickness;
      int horizontalEnd;
      int verticalEnd;
      this.GetPosition(instance,
                       matrix,
                       out horizontalStart,
                       out verticalStart,
                       out lineThickness,
                       out horizontalEnd,
                       out verticalEnd);

      var horizontalLength = horizontalEnd - horizontalStart;
      var verticalLength = verticalEnd - verticalStart;

      container.Body.Add(this.EplCommands.LineDrawBlack(horizontalStart,
                                                        verticalStart,
                                                        horizontalLength,
                                                        verticalLength));
    }

    protected virtual void TranslateBox([NotNull] SvgRectangle instance,
                                        [NotNull] Matrix matrix,
                                        [NotNull] EplContainer container)
    {
      int horizontalStart;
      int verticalStart;
      int lineThickness;
      int horizontalEnd;
      int verticalEnd;
      this.GetPosition(instance,
                       matrix,
                       out horizontalStart,
                       out verticalStart,
                       out lineThickness,
                       out horizontalEnd,
                       out verticalEnd);

      container.Body.Add(this.EplCommands.DrawBox(horizontalStart,
                                                  verticalStart,
                                                  lineThickness,
                                                  horizontalEnd,
                                                  verticalEnd));
    }

    [Pure]
    protected virtual void GetPosition([NotNull] SvgRectangle instance,
                                       [NotNull] Matrix matrix,
                                       out int horizontalStart,
                                       out int verticalStart,
                                       out int lineThickness,
                                       out int horizontalEnd,
                                       out int verticalEnd)
    {
      float startX;
      float endX;
      float startY;
      float endY;
      float strokeWidth;
      this.EplTransformer.Transform(instance,
                                    matrix,
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