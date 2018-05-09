using System;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public class SvgTextBaseTranslator<T> : SvgElementTranslatorBase<ZplContainer, T>
    where T : SvgTextBase
  {
    // TODO translate dX and dY

    /// <exception cref="ArgumentNullException"><paramref name="zplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplCommands" /> is <see langword="null" />.</exception>
    public SvgTextBaseTranslator([NotNull] ZplTransformer zplTransformer,
                                 [NotNull] ZplCommands zplCommands)
    {
      this.ZplTransformer = zplTransformer ?? throw new ArgumentNullException(nameof(zplTransformer));
      this.ZplCommands = zplCommands ?? throw new ArgumentNullException(nameof(zplCommands));
    }

    [NotNull]
    protected ZplTransformer ZplTransformer { get; }

    [NotNull]
    protected ZplCommands ZplCommands { get; }

    /// <exception cref="ArgumentNullException"><paramref name="svgElement" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplContainer" /> is <see langword="null" />.</exception>
    public override void Translate([NotNull] T svgElement,
                                   [NotNull] Matrix sourceMatrix,
                                   [NotNull] Matrix viewMatrix,
                                   [NotNull] ZplContainer zplContainer)
    {
      if (svgElement == null)
      {
        throw new ArgumentNullException(nameof(svgElement));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }
      if (zplContainer == null)
      {
        throw new ArgumentNullException(nameof(zplContainer));
      }

      if (svgElement.Text == null)
      {
        return;
      }

      var text = this.RemoveIllegalCharacters(svgElement.Text);
      if (string.IsNullOrEmpty(text))
      {
        return;
      }

      this.GetPosition(svgElement,
                       sourceMatrix,
                       viewMatrix,
                       out int horizontalStart,
                       out int verticalStart,
                       out FieldOrientation fieldOrientation,
                       out float fontSize);

      this.GetFontSelection(svgElement,
                            fontSize,
                            out string fontName,
                            out int characterHeight,
                            out int width);

      this.AddTranslationToContainer(horizontalStart,
                                     verticalStart,
                                     fontName,
                                     fieldOrientation,
                                     characterHeight,
                                     width,
                                     text,
                                     zplContainer);
    }

    /// <exception cref="ArgumentNullException"><paramref name="text" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual string RemoveIllegalCharacters([NotNull] string text)
    {
      if (text == null)
      {
        throw new ArgumentNullException(nameof(text));
      }

      // TODO add regex for removing illegal characters ...

      // ReSharper disable ExceptionNotDocumentedOptional
      return text.Replace("^",
                          string.Empty);
      // ReSharper restore ExceptionNotDocumentedOptional
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgElement" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    [Pure]
    protected virtual void GetPosition([NotNull] T svgElement,
                                       [NotNull] Matrix sourceMatrix,
                                       [NotNull] Matrix viewMatrix,
                                       out int horizontalStart,
                                       out int verticalStart,
                                       out FieldOrientation fieldOrientation,
                                       out float fontSize)
    {
      if (svgElement == null)
      {
        throw new ArgumentNullException(nameof(svgElement));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }

      this.ZplTransformer.Transform(svgElement,
                                    sourceMatrix,
                                    viewMatrix,
                                    out float x,
                                    out float y,
                                    out fontSize);

      horizontalStart = (int) x;
      verticalStart = (int) y;
      fieldOrientation = this.ZplTransformer.GetFieldOrientation(sourceMatrix,
                                                                 viewMatrix);
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgTextBase" /> is <see langword="null" />.</exception>
    [Pure]
    protected virtual void GetFontSelection([NotNull] SvgTextBase svgTextBase,
                                            float fontSize,
                                            [NotNull] out string fontName,
                                            out int characterHeight,
                                            out int width)
    {
      this.ZplTransformer.GetFontSelection(svgTextBase,
                                           fontSize,
                                           out fontName,
                                           out characterHeight,
                                           out width);
    }

    /// <exception cref="ArgumentNullException"><paramref name="fontName" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="text" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplContainer" /> is <see langword="null" />.</exception>
    protected virtual void AddTranslationToContainer(int horizontalStart,
                                                     int verticalStart,
                                                     [NotNull] string fontName,
                                                     FieldOrientation fieldOrientation,
                                                     int characterHeight,
                                                     int width,
                                                     [NotNull] string text,
                                                     [NotNull] ZplContainer zplContainer)
    {
      if (fontName == null)
      {
        throw new ArgumentNullException(nameof(fontName));
      }
      if (text == null)
      {
        throw new ArgumentNullException(nameof(text));
      }
      if (zplContainer == null)
      {
        throw new ArgumentNullException(nameof(zplContainer));
      }

      zplContainer.Body.Add(this.ZplCommands.FieldTypeset(horizontalStart,
                                                          verticalStart));
      zplContainer.Body.Add(this.ZplCommands.Font(fontName,
                                                  fieldOrientation,
                                                  characterHeight,
                                                  width,
                                                  text));
    }
  }
}
