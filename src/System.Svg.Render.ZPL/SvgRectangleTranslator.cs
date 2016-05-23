using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

namespace System.Svg.Render.ZPL
{
  [PublicAPI]
  public class SvgRectangleTranslator : SvgElementTranslatorBase<SvgRectangle>
  {
    public SvgRectangleTranslator([NotNull] ZplTransformer zplTransformer,
                                  [NotNull] ZplCommands zplCommands,
                                  [NotNull] SvgUnitReader svgUnitReader)
    {
      this.ZplTransformer = zplTransformer;
      this.ZplCommands = zplCommands;
      this.SvgUnitReader = svgUnitReader;
    }

    [NotNull]
    protected ZplTransformer ZplTransformer { get; }

    [NotNull]
    protected ZplCommands ZplCommands { get; }

    [NotNull]
    protected SvgUnitReader SvgUnitReader { get; }

    public override void Translate([NotNull] SvgRectangle svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] ZplStream container)
    {
      ZplStream zplStream;

      if (svgElement.Fill != SvgPaintServer.None
          && (svgElement.Fill as SvgColourServer)?.Colour != Color.White)
      {
        zplStream = this.TranslateFilledBox(svgElement,
                                            matrix);
      }
      else if (svgElement.Stroke != SvgPaintServer.None)
      {
        zplStream = this.TranslateBox(svgElement,
                                      matrix);
      }
      else
      {
        return;
      }

      if (zplStream.Any())
      {
        container.Add(zplStream);
      }
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual ZplStream TranslateFilledBox([NotNull] SvgRectangle instance,
                                                   [NotNull] Matrix matrix)
    {
      var startX = this.SvgUnitReader.GetValue(instance,
                                               instance.X);
      var startY = this.SvgUnitReader.GetValue(instance,
                                               instance.Y);
      var endX = startX + this.SvgUnitReader.GetValue(instance,
                                                      instance.Width);
      var endY = startY + this.SvgUnitReader.GetValue(instance,
                                                      instance.Height);

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
      this.ZplTransformer.Transform(svgLine,
                                    matrix,
                                    out startX,
                                    out startY,
                                    out endX,
                                    out endY,
                                    out strokeWidth);

      var horizontalStart = (int) startX;
      var verticalStart = (int) startY;
      var width = (int) Math.Abs(endX - startX);
      var thickness = (int) Math.Abs(endY - startY);
      var zplStream = this.ZplCommands.GraphicBox(horizontalStart,
                                                  verticalStart,
                                                  width,
                                                  0,
                                                  thickness,
                                                  LineColor.Black);

      return zplStream;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual ZplStream TranslateBox([NotNull] SvgRectangle instance,
                                             [NotNull] Matrix matrix)
    {
      float startX;
      float endX;
      float startY;
      float endY;
      float strokeWidth;
      this.ZplTransformer.Transform(instance,
                                    matrix,
                                    out startX,
                                    out startY,
                                    out endX,
                                    out endY,
                                    out strokeWidth);

      var horizontalStart = (int) startX;
      var verticalStart = (int) startY;
      var width = (int) Math.Abs(endX - startX);
      var height = (int) Math.Abs(endY - startY);
      var thickness = (int) strokeWidth;

      var zplStream = this.ZplCommands.GraphicBox(horizontalStart,
                                                  verticalStart,
                                                  width,
                                                  height,
                                                  thickness,
                                                  LineColor.Black);

      return zplStream;
    }
  }
}