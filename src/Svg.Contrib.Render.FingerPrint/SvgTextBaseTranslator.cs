using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
// ReSharper disable VirtualMemberNeverOverriden.Global

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class SvgTextBaseTranslator<T> : SvgElementTranslatorBase<FingerPrintContainer, T>
    where T : SvgTextBase
  {
    // TODO translate dX and dY

    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintTransformer"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintCommands"/> is <see langword="null" />.</exception>
    public SvgTextBaseTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                 [NotNull] FingerPrintCommands fingerPrintCommands)
    {
      if (fingerPrintTransformer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintTransformer));
      }
      if (fingerPrintCommands == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintCommands));
      }
      this.FingerPrintTransformer = fingerPrintTransformer;
      this.FingerPrintCommands = fingerPrintCommands;
    }

    [NotNull]
    protected FingerPrintTransformer FingerPrintTransformer { get; }

    [NotNull]
    protected FingerPrintCommands FingerPrintCommands { get; }

    /// <exception cref="ArgumentNullException"><paramref name="svgElement"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container"/> is <see langword="null" />.</exception>
    public override void Translate([NotNull] T svgElement,
                                   [NotNull] Matrix sourceMatrix,
                                   [NotNull] Matrix viewMatrix,
                                   [NotNull] FingerPrintContainer container)
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
      if (container == null)
      {
        throw new ArgumentNullException(nameof(container));
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
      Direction direction;
      this.GetPosition(svgElement,
                       sourceMatrix,
                       viewMatrix,
                       out horizontalStart,
                       out verticalStart,
                       out fontSize,
                       out direction);

      string fontName;
      int characterHeight;
      int slant;
      this.FingerPrintTransformer.GetFontSelection(svgElement,
                                                   fontSize,
                                                   out fontName,
                                                   out characterHeight,
                                                   out slant);

      this.AddTranslationToContainer(svgElement,
                                     horizontalStart,
                                     verticalStart,
                                     direction,
                                     fontName,
                                     characterHeight,
                                     slant,
                                     text,
                                     container);
    }

    /// <exception cref="ArgumentNullException"><paramref name="text"/> is <see langword="null" />.</exception>
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

    /// <exception cref="ArgumentNullException"><paramref name="svgElement"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix"/> is <see langword="null" />.</exception>
    [Pure]
    protected virtual void GetPosition([NotNull] T svgElement,
                                       [NotNull] Matrix sourceMatrix,
                                       [NotNull] Matrix viewMatrix,
                                       out int horizontalStart,
                                       out int verticalStart,
                                       out float fontSize,
                                       out Direction direction)
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
      this.FingerPrintTransformer.Transform(svgElement,
                                            sourceMatrix,
                                            viewMatrix,
                                            out x,
                                            out y,
                                            out fontSize,
                                            out direction);

      horizontalStart = (int) x;
      verticalStart = (int) y;
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgElement"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fontName"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="text"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintContainer"/> is <see langword="null" />.</exception>
    protected virtual void AddTranslationToContainer([NotNull] T svgElement,
                                                     int horizontalStart,
                                                     int verticalStart,
                                                     Direction direction,
                                                     [NotNull] string fontName,
                                                     int characterHeight,
                                                     int slant,
                                                     [NotNull] string text,
                                                     [NotNull] FingerPrintContainer fingerPrintContainer)
    {
      if (svgElement == null)
      {
        throw new ArgumentNullException(nameof(svgElement));
      }
      if (fontName == null)
      {
        throw new ArgumentNullException(nameof(fontName));
      }
      if (text == null)
      {
        throw new ArgumentNullException(nameof(text));
      }
      if (fingerPrintContainer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintContainer));
      }

      fingerPrintContainer.Body.Add(this.FingerPrintCommands.Position(horizontalStart,
                                                           verticalStart));
      fingerPrintContainer.Body.Add(this.FingerPrintCommands.Direction(direction));
      fingerPrintContainer.Body.Add(this.FingerPrintCommands.Align(Alignment.BaseLineLeft));

      if ((svgElement.Fill as SvgColourServer)?.Colour == Color.White)
      {
        fingerPrintContainer.Body.Add(this.FingerPrintCommands.InvertImage());
      }
      else
      {
        fingerPrintContainer.Body.Add(this.FingerPrintCommands.NormalImage());
      }

      fingerPrintContainer.Body.Add(this.FingerPrintCommands.Font(fontName,
                                                       characterHeight,
                                                       slant));
      fingerPrintContainer.Body.Add(this.FingerPrintCommands.PrintText(text));
    }
  }
}