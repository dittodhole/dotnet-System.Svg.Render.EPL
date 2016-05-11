using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgTextBaseTranslator<T> : SvgElementTranslatorBase<T>
    where T : SvgTextBase
  {
    // TODO translate dX and dY

    public SvgTextBaseTranslator([NotNull] SvgUnitCalculator svgUnitCalculator)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
    }

    [NotNull]
    private SvgUnitCalculator SvgUnitCalculator { get; }

    public float LineHeightFactor { get; set; } = 1.25f;

    public override void Translate([NotNull] T instance,
                                   [NotNull] Matrix matrix,
                                   out object translation)
    {
      var text = this.RemoveIllegalCharacters(instance.Text);
      if (string.IsNullOrWhiteSpace(text))
      {
#if DEBUG
        translation = $"; text is empty: {instance.GetXML()}";
#else
        translation = null;
#endif
        return;
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
      var rotationTranslation = (int) this.SvgUnitCalculator.GetRotationTranslation(fontSizeVector);

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

      translation = $@"A{horizontalStart},{verticalStart},{rotationTranslation},{fontTranslation},{reverseImage},""{text}""";
    }

    private string RemoveIllegalCharacters(string text)
    {
      // TODO add regex for removing illegal characters ...

      return text;
    }
  }
}