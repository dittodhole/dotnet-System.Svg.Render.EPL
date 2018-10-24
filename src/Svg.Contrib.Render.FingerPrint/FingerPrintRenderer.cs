using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class FingerPrintRenderer : RendererBase<FingerPrintContainer>
  {
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintCommands" /> is <see langword="null" />.</exception>
    public FingerPrintRenderer([NotNull] FingerPrintCommands fingerPrintCommands,
                               CharacterSet characterSet = CharacterSet.Utf8)
    {
      this.FingerPrintCommands = fingerPrintCommands ?? throw new ArgumentNullException(nameof(fingerPrintCommands));
      this.CharacterSet = characterSet;
    }

    [NotNull]
    private FingerPrintCommands FingerPrintCommands { get; }

    private CharacterSet CharacterSet { get; }

    /// <exception cref="ArgumentNullException"><paramref name="svgDocument" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    [Pure]
    public override FingerPrintContainer GetTranslation(SvgDocument svgDocument,
                                                        Matrix viewMatrix)
    {
      if (svgDocument == null)
      {
        throw new ArgumentNullException(nameof(svgDocument));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }

      var sourceMatrix = new Matrix();
      var fingerPrintContainer = new FingerPrintContainer();
      this.AddBodyToTranslation(svgDocument,
                                sourceMatrix,
                                viewMatrix,
                                fingerPrintContainer);
      this.AddHeaderToTranslation(svgDocument,
                                  sourceMatrix,
                                  viewMatrix,
                                  fingerPrintContainer);
      this.AddFooterToTranslation(svgDocument,
                                  sourceMatrix,
                                  viewMatrix,
                                  fingerPrintContainer);

      return fingerPrintContainer;
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgDocument" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintContainer" /> is <see langword="null" />.</exception>
    protected virtual void AddHeaderToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix sourceMatrix,
                                                  [NotNull] Matrix viewMatrix,
                                                  [NotNull] FingerPrintContainer fingerPrintContainer)
    {
      if (svgDocument == null)
      {
        throw new ArgumentNullException(nameof(svgDocument));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }
      if (fingerPrintContainer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintContainer));
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgDocument" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintContainer" /> is <see langword="null" />.</exception>
    protected virtual void AddBodyToTranslation([NotNull] SvgDocument svgDocument,
                                                [NotNull] Matrix sourceMatrix,
                                                [NotNull] Matrix viewMatrix,
                                                [NotNull] FingerPrintContainer fingerPrintContainer)
    {
      if (svgDocument == null)
      {
        throw new ArgumentNullException(nameof(svgDocument));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }
      if (fingerPrintContainer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintContainer));
      }

      fingerPrintContainer.Header.Add(this.FingerPrintCommands.ImmediateOn());
      fingerPrintContainer.Header.Add(this.FingerPrintCommands.SelectCharacterSet(this.CharacterSet));
      fingerPrintContainer.Body.Add(this.FingerPrintCommands.VerbOff());
      this.TranslateSvgElementAndChildren(svgDocument,
                                          sourceMatrix,
                                          viewMatrix,
                                          fingerPrintContainer);
      fingerPrintContainer.Body.Add(this.FingerPrintCommands.InputOff());
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgDocument" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintContainer" /> is <see langword="null" />.</exception>
    protected virtual void AddFooterToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix sourceMatrix,
                                                  [NotNull] Matrix viewMatrix,
                                                  [NotNull] FingerPrintContainer fingerPrintContainer)
    {
      if (svgDocument == null)
      {
        throw new ArgumentNullException(nameof(svgDocument));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }
      if (fingerPrintContainer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintContainer));
      }

      fingerPrintContainer.Footer.Add(this.FingerPrintCommands.PrintFeed());
      fingerPrintContainer.Footer.Add(string.Empty);
    }

    [NotNull]
    private IDictionary<CharacterSet, Encoding> CharacterSetToEncodingMappings { get; } = new Dictionary<CharacterSet, Encoding>
                                                                                          {
                                                                                            {
                                                                                              CharacterSet.Dos850, Encoding.GetEncoding(850)
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.Dos852, Encoding.GetEncoding(852)
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.Dos855, Encoding.GetEncoding(855)
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.Dos857, Encoding.GetEncoding(857)
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.Windows1250, Encoding.GetEncoding(1250)
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.Windows1251, Encoding.GetEncoding(1251)
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.Windows1252, Encoding.GetEncoding(1252)
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.Windows1253, Encoding.GetEncoding(1253)
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.Windows1254, Encoding.GetEncoding(1254)
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.Windows1257, Encoding.GetEncoding(1257)
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.Utf8, Encoding.UTF8
                                                                                            }
                                                                                          };


    [Pure]
    [NotNull]
    public override Encoding GetEncoding()
    {
      var encoding = this.CharacterSetToEncodingMappings[this.CharacterSet];

      return encoding;
    }
  }
}
