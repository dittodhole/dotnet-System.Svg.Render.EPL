using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgTextBaseTranslator<T> : SvgElementTranslatorBase<T>
    where T : SvgTextBase
  {
    // TODO translate dX and dY

    public SvgTextBaseTranslator([NotNull] EplTransformer eplTransformer,
                                 [NotNull] EplCommands eplCommands)
    {
      this.EplTransformer = eplTransformer;
      this.EplCommands = eplCommands;
    }

    [NotNull]
    protected EplTransformer EplTransformer { get; }

    [NotNull]
    protected EplCommands EplCommands { get; }

    public override void Translate([NotNull] T svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] EplStream container)
    {
      if (svgElement.Text == null)
      {
        return;
      }

      var text = this.RemoveIllegalCharacters(svgElement.Text);
      if (string.IsNullOrWhiteSpace(text))
      {
        return;
      }

      float x;
      float y;
      float fontSize;
      int rotation;
      this.EplTransformer.Transform(svgElement,
                                    matrix,
                                    out x,
                                    out y,
                                    out fontSize,
                                    out rotation);

      int fontSelection;
      int multiplier;
      this.EplTransformer.GetFontSelection(svgElement,
                                           fontSize,
                                           out fontSelection,
                                           out multiplier);

      var reverseImage = this.EplTransformer.GetReverseImage(svgElement);

      var horizontalStart = (int) x;
      var verticalStart = (int) y;

      var eplStream = this.EplCommands.AsciiText(horizontalStart,
                                                 verticalStart,
                                                 rotation,
                                                 fontSelection,
                                                 multiplier,
                                                 multiplier,
                                                 reverseImage,
                                                 text);
      if (!eplStream.IsEmpty)
      {
        container.Add(eplStream);
      }
    }

    protected virtual string RemoveIllegalCharacters([NotNull] string text)
    {
      // TODO add regex for removing illegal characters ...

      return text.Replace("\"",
                          "'");
    }
  }
}