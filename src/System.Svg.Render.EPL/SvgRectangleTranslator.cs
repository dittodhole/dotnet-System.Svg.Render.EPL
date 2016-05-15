using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgRectangleTranslator : SvgElementTranslatorBase<SvgRectangle>
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
    private EplTransformer EplTransformer { get; }

    [NotNull]
    private EplCommands EplCommands { get; }

    [NotNull]
    private SvgUnitReader SvgUnitReader { get; }

    public override IEnumerable<byte> Translate([NotNull] SvgRectangle svgElement,
                                                [NotNull] Matrix matrix)
    {
      if (svgElement.Fill != SvgPaintServer.None
          && (svgElement.Fill as SvgColourServer)?.Colour != Color.White)
      {
        return this.TranslateFilledBox(svgElement,
                                       matrix);
      }
      if (svgElement.Stroke != SvgPaintServer.None)
      {
        return this.TranslateBox(svgElement,
                                 matrix);
      }

      return null;
    }

    [NotNull]
    private IEnumerable<byte> TranslateFilledBox([NotNull] SvgRectangle instance,
                                                 [NotNull] Matrix matrix)
    {
      var startX = this.SvgUnitReader.GetValue(instance.X);
      var startY = this.SvgUnitReader.GetValue(instance.Y);
      var endX = startX + this.SvgUnitReader.GetValue(instance.Width);
      var endY = startY + this.SvgUnitReader.GetValue(instance.Height);

      var svgLine = new SvgLine
                    {
                      Color = instance.Color,
                      Stroke = SvgPaintServer.None,
                      StrokeWidth = instance.StrokeWidth,
                      StartX = startX,
                      StartY = startY,
                      EndX = endX,
                      EndY = endY
                    };

      float strokeWidth;
      this.EplTransformer.Transform(svgLine,
                                    matrix,
                                    out startX,
                                    out startY,
                                    out endX,
                                    out endY,
                                    out strokeWidth);

      var horizontalStart = (int) startX;
      var verticalStart = (int) startY;
      var horizontalLength = (int) Math.Abs(endX - startX);
      var verticalLength = (int) Math.Abs(endY - startY);
      var result = this.EplCommands.LineDrawBlack(horizontalStart,
                                                  verticalStart,
                                                  horizontalLength,
                                                  verticalLength);

      return result;
    }

    [NotNull]
    private IEnumerable<byte> TranslateBox([NotNull] SvgRectangle instance,
                                           [NotNull] Matrix matrix)
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

      var horizontalStart = (int) startX;
      var verticalStart = (int) startY;
      var lineThickness = (int) strokeWidth;
      var horizontalEnd = (int) endX;
      var verticalEnd = (int) endY;
      var result = this.EplCommands.DrawBox(horizontalStart,
                                            verticalStart,
                                            lineThickness,
                                            horizontalEnd,
                                            verticalEnd);

      return result;
    }
  }
}