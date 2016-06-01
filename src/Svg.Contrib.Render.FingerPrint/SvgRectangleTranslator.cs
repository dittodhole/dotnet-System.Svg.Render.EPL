using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class SvgRectangleTranslator : SvgElementTranslatorBase<FingerPrintContainer, SvgRectangle>
  {
    public SvgRectangleTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                  [NotNull] FingerPrintCommands fingerPrintCommands,
                                  [NotNull] SvgUnitReader svgUnitReader)
    {
      this.FingerPrintTransformer = fingerPrintTransformer;
      this.FingerPrintCommands = fingerPrintCommands;
      this.SvgUnitReader = svgUnitReader;
    }

    [NotNull]
    protected SvgUnitReader SvgUnitReader { get; }

    [NotNull]
    protected FingerPrintCommands FingerPrintCommands { get; }

    [NotNull]
    protected FingerPrintTransformer FingerPrintTransformer { get; }

    public override void Translate([NotNull] SvgRectangle svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] FingerPrintContainer container)
    {
      if (svgElement.Fill != SvgPaintServer.None
          && (svgElement.Fill as SvgColourServer)?.Colour != Color.White)
      {
        this.TranslateFilledBox(svgElement,
                                matrix,
                                container);
      }
      else if (svgElement.Stroke != SvgPaintServer.None)
      {
        this.TranslateBox(svgElement,
                          matrix,
                          container);
      }
    }

    protected virtual void TranslateFilledBox([NotNull] SvgRectangle instance,
                                              [NotNull] Matrix matrix,
                                              [NotNull] FingerPrintContainer container)
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

      var length = horizontalEnd - horizontalStart;
      var lineWeight = verticalEnd - verticalStart;

      container.Body.Add(this.FingerPrintCommands.Magnify(1,
                                                          1));
      container.Body.Add(this.FingerPrintCommands.Position(horizontalStart,
                                                           verticalStart));
      container.Body.Add(this.FingerPrintCommands.Direction(Direction.LeftToRight));
      container.Body.Add(this.FingerPrintCommands.Align(Alignment.TopLeft));
      container.Body.Add(this.FingerPrintCommands.Line(length,
                                                       lineWeight));
    }

    protected virtual void TranslateBox([NotNull] SvgRectangle instance,
                                        [NotNull] Matrix matrix,
                                        [NotNull] FingerPrintContainer container)
    {
      int horizontalStart;
      int verticalStart;
      int lineWeight;
      int horizontalEnd;
      int verticalEnd;
      this.GetPosition(instance,
                       matrix,
                       out horizontalStart,
                       out verticalStart,
                       out lineWeight,
                       out horizontalEnd,
                       out verticalEnd);

      var width = horizontalEnd - horizontalStart;
      var height = verticalEnd - verticalStart;

      container.Body.Add(this.FingerPrintCommands.Magnify(1,
                                                          1));
      container.Body.Add(this.FingerPrintCommands.Position(horizontalStart,
                                                           verticalStart));
      container.Body.Add(this.FingerPrintCommands.Direction(Direction.LeftToRight));
      container.Body.Add(this.FingerPrintCommands.Align(Alignment.TopLeft));
      container.Body.Add(this.FingerPrintCommands.Box(width,
                                                      height,
                                                      lineWeight));
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
      this.FingerPrintTransformer.Transform(instance,
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