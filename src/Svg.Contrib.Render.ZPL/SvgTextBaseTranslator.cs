using System.Drawing.Drawing2D;
using Svg;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public class SvgTextBaseTranslator<T> : SvgElementTranslatorBase<T>
    where T : SvgTextBase
  {
    // TODO translate dX and dY

    public SvgTextBaseTranslator([NotNull] ZplTransformer zplTransformer,
                                 [NotNull] ZplCommands zplCommands)
    {
      this.ZplTransformer = zplTransformer;
      this.ZplCommands = zplCommands;
    }

    [NotNull]
    protected ZplTransformer ZplTransformer { get; }

    [NotNull]
    protected ZplCommands ZplCommands { get; }

    public override void Translate([NotNull] T svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] Container<ZplStream> container)
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
      this.ZplTransformer.Transform(svgElement,
                                    matrix,
                                    out x,
                                    out y,
                                    out fontSize);

      var horizontalStart = (int) x;
      var verticalStart = (int) y;
      var fieldOrientation = this.ZplTransformer.GetFieldOrientation(matrix);

      string fontName;
      int characterHeight;
      int width;
      this.ZplTransformer.GetFontSelection(svgElement,
                                           fontSize,
                                           out fontName,
                                           out characterHeight,
                                           out width);

      container.Body.Add(this.ZplCommands.FieldTypeset(horizontalStart,
                                                       verticalStart));
      container.Body.Add(this.ZplCommands.Font(fontName,
                                               fieldOrientation,
                                               characterHeight,
                                               width,
                                               text));
    }

    [Pure]
    [MustUseReturnValue]
    protected virtual string RemoveIllegalCharacters([NotNull] string text)
    {
      // TODO add regex for removing illegal characters ...

      // ReSharper disable ExceptionNotDocumentedOptional
      return text.Replace("^",
                          string.Empty);
      // ReSharper restore ExceptionNotDocumentedOptional
    }
  }
}