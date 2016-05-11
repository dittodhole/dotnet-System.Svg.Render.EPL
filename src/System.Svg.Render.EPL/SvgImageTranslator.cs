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
      }

      var startX = this.SvgUnitCalculator.GetValue(instance.X);
      var startY = this.SvgUnitCalculator.GetValue(instance.Y);
      var endX = startX + this.SvgUnitCalculator.GetValue(instance.Width);
      var endY = startY + this.SvgUnitCalculator.GetValue(instance.Height);

      this.SvgUnitCalculator.ApplyMatrix(startX,
                                         startY,
                                         matrix,
                                         out startX,
                                         out startY);

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
      var stringBuilder = new StringBuilder();
      stringBuilder.AppendLine($"GW{horizontalStart},{verticalStart},{bytes},{height}");
      for (var line = 0;
           line < height;
           line++)
      {
        var fillChar = (char) (0 << 8);
        var lineTranslation = new string(fillChar,
                                         bytes);
        stringBuilder.Append(lineTranslation);
      }

      translation = stringBuilder;
    }
  }
}