using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class SvgTextBaseTranslator<T> : SvgElementTranslatorBase<EplContainer, T>
    where T : SvgTextBase
  {
    // TODO translate dX and dY

    /// <exception cref="ArgumentNullException"><paramref name="eplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplCommands" /> is <see langword="null" />.</exception>
    public SvgTextBaseTranslator([NotNull] EplTransformer eplTransformer,
                                 [NotNull] EplCommands eplCommands)
    {
      this.EplTransformer = eplTransformer ?? throw new ArgumentNullException(nameof(eplTransformer));
      this.EplCommands = eplCommands ?? throw new ArgumentNullException(nameof(eplCommands));
    }

    [NotNull]
    private EplTransformer EplTransformer { get; }

    [NotNull]
    private EplCommands EplCommands { get; }

    /// <exception cref="ArgumentNullException"><paramref name="svgElement" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplContainer" /> is <see langword="null" />.</exception>
    public override void Translate(T svgElement,
                                   Matrix sourceMatrix,
                                   Matrix viewMatrix,
                                   EplContainer eplContainer)
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
      if (eplContainer == null)
      {
        throw new ArgumentNullException(nameof(eplContainer));
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
                       out var horizontalStart,
                       out var verticalStart,
                       out var sector,
                       out var fontSize);

      this.GetFontSelection(svgElement,
                            fontSize,
                            out var fontSelection,
                            out var horizontalMultiplier,
                            out var verticalMultiplier);

      this.AddTranslationToContainer(svgElement,
                                     horizontalStart,
                                     verticalStart,
                                     sector,
                                     fontSelection,
                                     horizontalMultiplier,
                                     verticalMultiplier,
                                     text,
                                     eplContainer);
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
                                       out int sector,
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

      this.EplTransformer.Transform(svgElement,
                                    sourceMatrix,
                                    viewMatrix,
                                    out var x,
                                    out var y,
                                    out fontSize);

      horizontalStart = (int) x;
      verticalStart = (int) y;
      sector = this.EplTransformer.GetRotationSector(sourceMatrix,
                                                     viewMatrix);
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

      return text.Replace("\"",
                          "'");
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgTextBase" /> is <see langword="null" />.</exception>
    [Pure]
    protected virtual void GetFontSelection([NotNull] SvgTextBase svgTextBase,
                                            float fontSize,
                                            out int fontSelection,
                                            out int horizontalMultiplier,
                                            out int verticalMultiplier)
    {
      this.EplTransformer.GetFontSelection(svgTextBase,
                                           fontSize,
                                           out fontSelection,
                                           out horizontalMultiplier,
                                           out verticalMultiplier);
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgElement" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="text" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplContainer" /> is <see langword="null" />.</exception>
    protected virtual void AddTranslationToContainer([NotNull] T svgElement,
                                                     int horizontalStart,
                                                     int verticalStart,
                                                     int sector,
                                                     int fontSelection,
                                                     int horizontalMultiplier,
                                                     int verticalMultiplier,
                                                     [NotNull] string text,
                                                     [NotNull] EplContainer eplContainer)
    {
      if (svgElement == null)
      {
        throw new ArgumentNullException(nameof(svgElement));
      }
      if (text == null)
      {
        throw new ArgumentNullException(nameof(text));
      }
      if (eplContainer == null)
      {
        throw new ArgumentNullException(nameof(eplContainer));
      }

      ReverseImage reverseImage;
      if ((svgElement.Fill as SvgColourServer)?.Colour == Color.White)
      {
        reverseImage = ReverseImage.Reverse;
      }
      else
      {
        reverseImage = ReverseImage.Normal;
      }

      eplContainer.Body.Add(this.EplCommands.AsciiText(horizontalStart,
                                                       verticalStart,
                                                       sector,
                                                       fontSelection,
                                                       horizontalMultiplier,
                                                       verticalMultiplier,
                                                       reverseImage,
                                                       text));
    }
  }
}
