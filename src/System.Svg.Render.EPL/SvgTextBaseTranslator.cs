using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString

namespace System.Svg.Render.EPL
{
  [PublicAPI]
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
      this.EplTransformer.Transform(svgElement,
                                    matrix,
                                    out x,
                                    out y,
                                    out fontSize);

      var horizontalStart = (int) x;
      var verticalStart = (int) y;
      var sector = this.EplTransformer.GetRotationSector(matrix);

      int fontSelection;
      int horizontalMultiplier;
      int verticalMultiplier;
      this.EplTransformer.GetFontSelection(svgElement,
                                           fontSize,
                                           out fontSelection,
                                           out horizontalMultiplier,
                                           out verticalMultiplier);

      ReverseImage reverseImage;
      if ((svgElement.Fill as SvgColourServer)?.Colour == Color.White)
      {
        reverseImage = ReverseImage.Reverse;
      }
      else
      {
        reverseImage = ReverseImage.Normal;
      }

      container.Add(this.EplCommands.AsciiText(horizontalStart,
                                               verticalStart,
                                               sector,
                                               fontSelection,
                                               horizontalMultiplier,
                                               verticalMultiplier,
                                               reverseImage,
                                               text));
    }

    [Pure]
    [MustUseReturnValue]
    protected virtual string RemoveIllegalCharacters([NotNull] string text)
    {
      // TODO add regex for removing illegal characters ...

      // ReSharper disable ExceptionNotDocumentedOptional
      return text.Replace("\"",
                          "'");
      // ReSharper restore ExceptionNotDocumentedOptional
    }
  }
}