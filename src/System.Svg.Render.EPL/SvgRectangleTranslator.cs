using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgRectangleTranslator : SvgElementTranslatorBase<SvgRectangle>
  {
    public SvgRectangleTranslator([NotNull] EplTransformer eplTransformer,
                                  [NotNull] EplCommands eplCommands)
    {
      this.EplTransformer = eplTransformer;
      this.EplCommands = eplCommands;
    }

    [NotNull]
    private EplTransformer EplTransformer { get; }

    [NotNull]
    private EplCommands EplCommands { get; }

    [NotNull]
    public override IEnumerable<byte> Translate([NotNull] SvgRectangle instance,
                                                [NotNull] Matrix matrix)
    {
      if (instance.Fill != SvgPaintServer.None
          && (instance.Fill as SvgColourServer)?.Colour != Color.White)
      {
        return this.TranslateFilledBox(instance,
                                       matrix);
      }
      if (instance.Stroke != SvgPaintServer.None)
      {
        return this.TranslateBox(instance,
                                 matrix);
      }

      return Enumerable.Empty<byte>();
    }

    [NotNull]
    private IEnumerable<byte> TranslateFilledBox([NotNull] SvgRectangle instance,
                                                 [NotNull] Matrix matrix)
    {
      float startX;
      float endX;
      float startY;
      float endY;
      float strokeWidth;
      this.EplTransformer.Transform(instance,
                                    matrix,
                                    false,
                                    out startX,
                                    out startY,
                                    out endX,
                                    out endY,
                                    out strokeWidth);

      var horizontalStart = (int) startX;
      var verticalStart = (int) startY;
      var horizontalLength = (int) (endX - startX);
      var verticalLength = (int) (endY - startY);
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
                                    true,
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