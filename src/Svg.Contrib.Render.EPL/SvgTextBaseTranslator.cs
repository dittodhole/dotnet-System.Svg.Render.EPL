using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global
// ReSharper disable VirtualMemberNeverOverriden.Global

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
      if (eplTransformer == null)
      {
        throw new ArgumentNullException(nameof(eplTransformer));
      }
      if (eplCommands == null)
      {
        throw new ArgumentNullException(nameof(eplCommands));
      }
      this.EplTransformer = eplTransformer;
      this.EplCommands = eplCommands;
    }

    [NotNull]
    protected EplTransformer EplTransformer { get; }

    [NotNull]
    protected EplCommands EplCommands { get; }

    /// <exception cref="ArgumentNullException"><paramref name="svgElement" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplContainer" /> is <see langword="null" />.</exception>
    public override void Translate([NotNull] T svgElement,
                                   [NotNull] Matrix sourceMatrix,
                                   [NotNull] Matrix viewMatrix,
                                   [NotNull] EplContainer eplContainer)
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

      float fontSize;
      int horizontalStart;
      int verticalStart;
      int sector;
      this.GetPosition(svgElement,
                       sourceMatrix,
                       viewMatrix,
                       out horizontalStart,
                       out verticalStart,
                       out sector,
                       out fontSize);

      int fontSelection;
      int horizontalMultiplier;
      int verticalMultiplier;
      this.GetFontSelection(svgElement,
                            fontSize,
                            out fontSelection,
                            out horizontalMultiplier,
                            out verticalMultiplier);

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

      float x;
      float y;
      this.EplTransformer.Transform(svgElement,
                                    sourceMatrix,
                                    viewMatrix,
                                    out x,
                                    out y,
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

      // ReSharper disable ExceptionNotDocumentedOptional
      return text.Replace("\"",
                          "'");
      // ReSharper restore ExceptionNotDocumentedOptional
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
