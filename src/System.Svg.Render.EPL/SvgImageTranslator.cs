using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgImageTranslator : SvgElementTranslatorBase<SvgImage>
  {
    public SvgImageTranslator([NotNull] EplTransformer eplTransformer,
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
    public override IEnumerable<byte> Translate([NotNull] SvgImage instance,
                                                [NotNull] Matrix matrix)
    {
      float startX;
      float startY;
      float endX;
      float endY;
      float width;
      float height;
      this.EplTransformer.Transform(instance,
                                    matrix,
                                    out startX,
                                    out startY,
                                    out endX,
                                    out endY,
                                    out width,
                                    out height);

      var horizontalStart = (int) (startX - Math.Abs(startX - endX));
      var verticalStart = (int) startY;

      using (var image = instance.GetImage() as Image)
      {
        if (image == null)
        {
          return Enumerable.Empty<byte>();
        }

        var rotationTranslation = this.EplTransformer.GetRotation(matrix);

        using (var destinationBitmap = new Bitmap(image,
                                                  (int) width,
                                                  (int) height))
        {
          var rotateFlipType = (RotateFlipType) rotationTranslation;
          destinationBitmap.RotateFlip(rotateFlipType);

          var result = this.EplCommands.GraphicDirectWrite(destinationBitmap,
                                                           horizontalStart,
                                                           verticalStart)
                           .ToArray();

          return result;
        }
      }
    }
  }
}