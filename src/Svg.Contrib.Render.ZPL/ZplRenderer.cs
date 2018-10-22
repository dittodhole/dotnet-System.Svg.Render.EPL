using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public class ZplRenderer : RendererBase<ZplContainer>
  {
    /// <exception cref="ArgumentNullException"><paramref name="zplCommands" /> is <see langword="null" />.</exception>
    public ZplRenderer([NotNull] ZplCommands zplCommands,
                       CharacterSet characterSet = CharacterSet.Utf8)
    {
      this.ZplCommands = zplCommands ?? throw new ArgumentNullException(nameof(zplCommands));
      this.CharacterSet = characterSet;
    }

    [NotNull]
    private ZplCommands ZplCommands { get; }

    private CharacterSet CharacterSet { get; }

    [NotNull]
    private IDictionary<CharacterSet, Encoding> CharacterSetToEncodingMappings { get; } = new Dictionary<CharacterSet, Encoding>
                                                                                          {
                                                                                            {
                                                                                              CharacterSet.ZebraCodePage850, Encoding.GetEncoding(850)
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.ZebraCodePage1252, Encoding.GetEncoding(1252)
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.Utf8, Encoding.UTF8
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.Utf16BigEndian, Encoding.BigEndianUnicode
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.Utf16LittleEndian, Encoding.Unicode
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.ZebraCodePage1250, Encoding.GetEncoding(1250)
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.CodePage1251, Encoding.GetEncoding(1251)
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.CodePage1253, Encoding.GetEncoding(1253)
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.CodePage1254, Encoding.GetEncoding(1254)
                                                                                            },
                                                                                            {
                                                                                              CharacterSet.CodePage1255, Encoding.GetEncoding(1255)
                                                                                            }
                                                                                          };

    [NotNull]
    [Pure]
    public override Encoding GetEncoding()
    {
      var encoding = this.CharacterSetToEncodingMappings[this.CharacterSet];

      return encoding;
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgDocument" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    [Pure]
    public override ZplContainer GetTranslation(SvgDocument svgDocument,
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
      var zplContainer = new ZplContainer();
      this.AddBodyToTranslation(svgDocument,
                                sourceMatrix,
                                viewMatrix,
                                zplContainer);
      this.AddHeaderToTranslation(svgDocument,
                                  sourceMatrix,
                                  viewMatrix,
                                  zplContainer);
      this.AddFooterToTranslation(svgDocument,
                                  sourceMatrix,
                                  viewMatrix,
                                  zplContainer);

      return zplContainer;
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgDocument" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplContainer" /> is <see langword="null" />.</exception>
    protected virtual void AddHeaderToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix sourceMatrix,
                                                  [NotNull] Matrix viewMatrix,
                                                  [NotNull] ZplContainer zplContainer)
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
      if (zplContainer == null)
      {
        throw new ArgumentNullException(nameof(zplContainer));
      }

    }

    /// <exception cref="ArgumentNullException"><paramref name="svgDocument" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplContainer" /> is <see langword="null" />.</exception>
    protected virtual void AddBodyToTranslation([NotNull] SvgDocument svgDocument,
                                                [NotNull] Matrix sourceMatrix,
                                                [NotNull] Matrix viewMatrix,
                                                [NotNull] ZplContainer zplContainer)
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
      if (zplContainer == null)
      {
        throw new ArgumentNullException(nameof(zplContainer));
      }

      zplContainer.Body.Add(this.ZplCommands.StartFormat());
      zplContainer.Body.Add(this.ZplCommands.ChangeInternationalFont(this.CharacterSet));
      zplContainer.Body.Add(this.ZplCommands.LabelHome(18,
                                                       8));
      zplContainer.Body.Add(this.ZplCommands.PrintOrientation(PrintOrientation.Normal));
      this.TranslateSvgElementAndChildren(svgDocument,
                                          sourceMatrix,
                                          viewMatrix,
                                          zplContainer);
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgDocument" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplContainer" /> is <see langword="null" />.</exception>
    protected virtual void AddFooterToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix sourceMatrix,
                                                  [NotNull] Matrix viewMatrix,
                                                  [NotNull] ZplContainer zplContainer)
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
      if (zplContainer == null)
      {
        throw new ArgumentNullException(nameof(zplContainer));
      }

      zplContainer.Footer.Add(this.ZplCommands.EndFormat());
    }
  }
}
