using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgTextBaseTranslator<T> : SvgElementTranslatorWithEncoding<T>
    where T : SvgTextBase
  {
    // TODO translate dX and dY

    public SvgTextBaseTranslator([NotNull] SvgUnitCalculator svgUnitCalculator,
                                 [NotNull] Encoding encoding)
      : base(encoding)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
    }

    [NotNull]
    private SvgUnitCalculator SvgUnitCalculator { get; }

    public float LineHeightFactor { get; set; } = 1.25f;

    public override IEnumerable<byte> Translate([NotNull] T instance,
                                                [NotNull] Matrix matrix)
    {
      var text = this.RemoveIllegalCharacters(instance.Text);
      if (string.IsNullOrWhiteSpace(text))
      {
        return Enumerable.Empty<byte>();
      }

      var x = this.SvgUnitCalculator.GetValue(instance.X.First());
      var y = this.SvgUnitCalculator.GetValue(instance.Y.First());
      var fontSize = this.SvgUnitCalculator.GetValue(instance.FontSize);

      y -= fontSize / this.LineHeightFactor;

      this.SvgUnitCalculator.ApplyMatrix(x,
                                         y,
                                         matrix,
                                         out x,
                                         out y);

      var fontSizeVector = new PointF(fontSize * -1f,
                                      0f);
      this.SvgUnitCalculator.ApplyMatrix(fontSizeVector,
                                         matrix,
                                         out fontSizeVector);
      fontSize = this.SvgUnitCalculator.GetLengthOfVector(fontSizeVector);
      var rotationTranslation = this.SvgUnitCalculator.GetRotationTranslation(fontSizeVector);

      object fontSelection;
      object multiplier;
      this.SvgUnitCalculator.GetFontSelection(fontSize,
                                              out fontSelection,
                                              out multiplier);

      var fontTranslation = $"{fontSelection},{multiplier},{multiplier}";

      string reverseImage;
      if ((instance.Fill as SvgColourServer)?.Colour == Color.White)
      {
        reverseImage = "R";
      }
      else
      {
        reverseImage = "N";
      }

      var horizontalStart = (int) x;
      var verticalStart = (int) y;

      var translation = $@"A{horizontalStart},{verticalStart},{rotationTranslation},{fontTranslation},{reverseImage},""{text}""";
      var result = this.GetBytes(translation);

      return result;
    }

    private string RemoveIllegalCharacters(string text)
    {
      // TODO add regex for removing illegal characters ...

      return text;
    }
  }
}