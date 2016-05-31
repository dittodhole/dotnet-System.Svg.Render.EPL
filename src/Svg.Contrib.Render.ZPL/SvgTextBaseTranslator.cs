using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

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
                                   [NotNull] ZplContainer container)
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

      float fontSize;
      int horizontalStart;
      int verticalStart;
      FieldOrientation fieldOrientation;
      this.GetPosition(svgElement,
                       matrix,
                       out horizontalStart,
                       out verticalStart,
                       out fieldOrientation,
                       out fontSize);

      string fontName;
      int characterHeight;
      int width;
      this.ZplTransformer.GetFontSelection(svgElement,
                                           fontSize,
                                           out fontName,
                                           out characterHeight,
                                           out width);

      this.AddTranslationToContainer(horizontalStart,
                                     verticalStart,
                                     fontName,
                                     fieldOrientation,
                                     characterHeight,
                                     width,
                                     text,
                                     container);
    }

    [NotNull]
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

    [Pure]
    protected virtual void GetPosition([NotNull] T svgElement,
                                       [NotNull] Matrix matrix,
                                       out int horizontalStart,
                                       out int verticalStart,
                                       out FieldOrientation fieldOrientation,
                                       out float fontSize)
    {
      float x;
      float y;
      this.ZplTransformer.Transform(svgElement,
                                    matrix,
                                    out x,
                                    out y,
                                    out fontSize);

      horizontalStart = (int) x;
      verticalStart = (int) y;
      fieldOrientation = this.ZplTransformer.GetFieldOrientation(matrix);
    }

    protected virtual void AddTranslationToContainer(int horizontalStart,
                                                     int verticalStart,
                                                     [NotNull] string fontName,
                                                     FieldOrientation fieldOrientation,
                                                     int characterHeight,
                                                     int width,
                                                     [NotNull] string text,
                                                     [NotNull] ZplContainer container)
    {
      container.Body.Add(this.ZplCommands.FieldTypeset(horizontalStart,
                                                       verticalStart));
      container.Body.Add(this.ZplCommands.Font(fontName,
                                               fieldOrientation,
                                               characterHeight,
                                               width,
                                               text));
    }
  }
}