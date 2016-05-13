using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgImageTranslator : SvgElementTranslatorBase<SvgImage>
  {
    public SvgImageTranslator([NotNull] SvgUnitCalculator svgUnitCalculator,
                              [NotNull] EplCommands eplCommands)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
      this.EplCommands = eplCommands;
    }

    [NotNull]
    private SvgUnitCalculator SvgUnitCalculator { get; }

    [NotNull]
    private EplCommands EplCommands { get; }

    public override IEnumerable<byte> Translate([NotNull] SvgImage instance,
                                                [NotNull] Matrix matrix)
    {
      var startX = this.SvgUnitCalculator.GetValue(instance.X);
      var startY = this.SvgUnitCalculator.GetValue(instance.Y);
      var originalWidth = this.SvgUnitCalculator.GetValue(instance.Width);
      var originalHeight = this.SvgUnitCalculator.GetValue(instance.Height);
      var endX = startX + originalWidth;
      var endY = startY + originalHeight;

      this.SvgUnitCalculator.ApplyMatrix(startX,
                                         startY,
                                         matrix,
                                         out startX,
                                         out startY);

      this.SvgUnitCalculator.ApplyMatrix(originalWidth,
                                         matrix,
                                         out originalWidth);
      this.SvgUnitCalculator.ApplyMatrix(originalHeight,
                                         matrix,
                                         out originalHeight);

      this.SvgUnitCalculator.ApplyMatrix(endX,
                                         endY,
                                         matrix,
                                         out endX,
                                         out endY);

      var horizontalStart = (int) startX;
      var verticalStart = (int) startY;

      using (var image = instance.GetImage() as Image)
      {
        if (image == null)
        {
          return Enumerable.Empty<byte>();
        }

        var heightVector = new PointF(image.Height * -1f,
                                      0f);
        this.SvgUnitCalculator.ApplyMatrix(heightVector,
                                           matrix,
                                           out heightVector);
        var rotationTranslation = this.SvgUnitCalculator.GetRotationTranslation(heightVector);

        using (var destinationBitmap = new Bitmap(image,
                                                  (int) originalWidth,
                                                  (int) originalHeight))
        {
          var rotateFlipType = (RotateFlipType) rotationTranslation;
          destinationBitmap.RotateFlip(rotateFlipType);

          var result = this.EplCommands.GraphicDirectWrite(destinationBitmap,
                                                           horizontalStart,
                                                           verticalStart);

          return result;
        }
      }
    }
  }
}