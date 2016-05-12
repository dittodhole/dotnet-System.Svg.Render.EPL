using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgImageTranslator : SvgElementTranslatorWithEncoding<SvgImage>
  {
    public SvgImageTranslator([NotNull] SvgUnitCalculator svgUnitCalculator,
                              [NotNull] Encoding encoding)
      : base(encoding)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
    }

    [NotNull]
    private SvgUnitCalculator SvgUnitCalculator { get; }

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
      var width = (int) Math.Abs(endX - startX);
      var height = (int) Math.Abs(endY - startY);

      horizontalStart -= width;

      var octetts = (int) Math.Ceiling(width / 8f);

      using (var image = instance.GetImage() as Image)
      {
        if (image == null)
        {
          return Enumerable.Empty<byte>();
        }

        var result = new LinkedList<byte>();

        var translation = $"GW{horizontalStart},{verticalStart},{octetts},{height}";
        foreach (var @byte in this.GetBytes(translation))
        {
          result.AddLast(@byte);
        }
        foreach (var @byte in this.GetBytes(Environment.NewLine))
        {
          result.AddLast(@byte);
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

          var alignedWidth = octetts * 8;
          for (var y = 0;
               y < height;
               y++)
          {
            var octett = (1 << 8) - 1;
            for (var x = 0;
                 x < alignedWidth;
                 x++)
            {
              var bitIndex = 7 - x % 8;
              if (x < width)
              {
                var color = destinationBitmap.GetPixel(x,
                                                       y);
                if (color.A > 0x32
                    || color.R > 0x96 && color.G > 0x96 && color.B > 0x96)
                {
                  octett &= ~(1 << bitIndex);
                }
              }

              if (bitIndex == 0)
              {
                result.AddLast((byte) octett);
                octett = byte.MaxValue;
              }
            }
          }
        }

        return result;
      }
    }
  }
}