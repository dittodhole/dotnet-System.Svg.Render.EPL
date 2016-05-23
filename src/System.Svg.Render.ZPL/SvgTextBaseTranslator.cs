using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString

namespace System.Svg.Render.ZPL
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

    private IDictionary<int, FieldOrientation> RotationMappings { get; } = new Dictionary<int, FieldOrientation>
                                                                           {
                                                                             {
                                                                               0, FieldOrientation.Normal
                                                                             },
                                                                             {
                                                                               1, FieldOrientation.RotatedBy90Degrees
                                                                             },
                                                                             {
                                                                               2, FieldOrientation.RotatedBy180Degrees
                                                                             },
                                                                             {
                                                                               3, FieldOrientation.RotatedBy270Degrees
                                                                             }
                                                                           };

    [NotNull]
    protected ZplTransformer ZplTransformer { get; }

    [NotNull]
    protected ZplCommands ZplCommands { get; }

    public override void Translate([NotNull] T svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] ZplStream container)
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
      var fieldOrientation = this.ZplTransformer.GetRotation(matrix);

      string fontName;
      int characterHeight;
      int width;

      this.ZplTransformer.GetFontSelection(svgElement,
                                           fontSize,
                                           out fontName,
                                           out characterHeight,
                                           out width);

      var zplStream = this.ZplCommands.Font(horizontalStart,
                                            verticalStart,
                                            fontName,
                                            fieldOrientation,
                                            characterHeight,
                                            width,
                                            text);
      // ReSharper disable ExceptionNotDocumentedOptional
      if (zplStream.Any())
      // ReSharper restore ExceptionNotDocumentedOptional
      {
        container.Add(zplStream);
      }
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