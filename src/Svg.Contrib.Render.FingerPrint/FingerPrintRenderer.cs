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
    private IDictionary<CharacterSet, int> CharacterSetMappings { get; } = new Dictionary<CharacterSet, int>
                                                                           {
                                                                             {
                                                                               CharacterSet.Dos850, 850
                                                                             },
                                                                             {
                                                                               CharacterSet.Dos851, 851
                                                                             },
                                                                             {
                                                                               CharacterSet.Dos852, 852
                                                                             },
                                                                             {
                                                                               CharacterSet.Dos855, 855
                                                                             },
                                                                             {
                                                                               CharacterSet.Dos857, 857
                                                                             },
                                                                             {
                                                                               CharacterSet.Windows1250, 1250
                                                                             },
                                                                             {
                                                                               CharacterSet.Windows1251, 1251
                                                                             },
                                                                             {
                                                                               CharacterSet.Windows1252, 1252
                                                                             },
                                                                             {
                                                                               CharacterSet.Windows1253, 1253
                                                                             },
                                                                             {
                                                                               CharacterSet.Windows1254, 1254
                                                                             },
                                                                             {
                                                                               CharacterSet.Windows1257, 1257
                                                                             },
                                                                             {
                                                                               CharacterSet.Utf8, 65001
                                                                             }
                                                                           };


    [Pure]
    [NotNull]
    public override Encoding GetEncoding()
    {
      var codepage = this.CharacterSetMappings[this.CharacterSet];
      var encoding = Encoding.GetEncoding(codepage);

      return encoding;
    }
  }
}
