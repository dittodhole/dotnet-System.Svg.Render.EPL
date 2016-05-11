using System.Drawing;
using System.Drawing.Drawing2D;
using System.Text;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgImageTranslator : SvgElementTranslatorBase<SvgImage>
  {
    public SvgImageTranslator([NotNull] SvgUnitCalculator svgUnitCalculator)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
    }

    [NotNull]
    private SvgUnitCalculator SvgUnitCalculator { get; }

    public override void Translate([NotNull] SvgImage instance,
                                   [NotNull] Matrix matrix,
                                   out object translation)
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

      var bytes = (width >> 3) + 1;

      using (var image = instance.GetImage() as Image)
      {
        if (image == null)
        {
#if DEBUG
          translation = $"; could not translate image (no content): {instance.GetXML()}";
#else
        translation = null;
#endif
          return;
        }

        var stringBuilder = new StringBuilder();
        stringBuilder.AppendLine($"GW{horizontalStart},{verticalStart},{bytes},{height}");

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

          // TODO fix the algorithm

          for (var y = 0;
               y < height;
               y++)
          {
            for (var octett = 0;
                 octett <= bytes;
                 octett++)
            {
              var lowerBound = octett << 3;
              var upperBound = octett + 1 << 3;

              var result = 0;
              for (int x = lowerBound,
                       i = 0;
                   x < upperBound;
                   x++, i++)
              {
                if (x >= width)
                {
                  continue;
                }

                var color = destinationBitmap.GetPixel(x,
                                                       y);
                if (color.A < 0x32
                    || color.R < 0x32
                    || color.G < 0x32
                    || color.B < 0.32)
                {
                  result |= 1 << 7 - i;
                }
              }

              var fillChar = (char) result;
              stringBuilder.Append(fillChar);
            }
          }
        }

        translation = stringBuilder;
      }
    }
  }
}