using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class EplRenderer : RendererBase<EplContainer>
  {
    /// <exception cref="ArgumentNullException"><paramref name="eplCommands" /> is <see langword="null" />.</exception>
    public EplRenderer([NotNull] EplCommands eplCommands,
                       PrinterCodepage printerCodepage = PrinterCodepage.Dos850,
                       int countryCode = 850)
    {
      this.EplCommands = eplCommands ?? throw new ArgumentNullException(nameof(eplCommands));
      this.PrinterCodepage = printerCodepage;
      this.CountryCode = countryCode;
    }

    [NotNull]
    private EplCommands EplCommands { get; }

    private PrinterCodepage PrinterCodepage { get; }

    private int CountryCode { get; }

    [NotNull]
    private IDictionary<PrinterCodepage, Encoding> PrinterCodepageToEncodingMappings { get; } = new Dictionary<PrinterCodepage, Encoding>
                                                                                                {
                                                                                                  {
                                                                                                    PrinterCodepage.Dos850, Encoding.GetEncoding(850)
                                                                                                  },
                                                                                                  {
                                                                                                    PrinterCodepage.Dos852, Encoding.GetEncoding(852)
                                                                                                  },
                                                                                                  {
                                                                                                    PrinterCodepage.Dos860, Encoding.GetEncoding(860)
                                                                                                  },
                                                                                                  {
                                                                                                    PrinterCodepage.Dos863, Encoding.GetEncoding(863)
                                                                                                  },
                                                                                                  {
                                                                                                    PrinterCodepage.Dos865, Encoding.GetEncoding(865)
                                                                                                  },
                                                                                                  {
                                                                                                    PrinterCodepage.Dos857, Encoding.GetEncoding(857)
                                                                                                  },
                                                                                                  {
                                                                                                    PrinterCodepage.Dos861, Encoding.GetEncoding(861)
                                                                                                  },
                                                                                                  {
                                                                                                    PrinterCodepage.Dos862, Encoding.GetEncoding(862)
                                                                                                  },
                                                                                                  {
                                                                                                    PrinterCodepage.Dos855, Encoding.GetEncoding(855)
                                                                                                  },
                                                                                                  {
                                                                                                    PrinterCodepage.Dos866, Encoding.GetEncoding(866)
                                                                                                  },
                                                                                                  {
                                                                                                    PrinterCodepage.Dos737, Encoding.GetEncoding(737)
                                                                                                  },
                                                                                                  {
                                                                                                    PrinterCodepage.Dos869, Encoding.GetEncoding(869)
                                                                                                  },
                                                                                                  {
                                                                                                    PrinterCodepage.Windows1250, Encoding.GetEncoding(1250)
                                                                                                  },
                                                                                                  {
                                                                                                    PrinterCodepage.Windows1251, Encoding.GetEncoding(1251)
                                                                                                  },
                                                                                                  {
                                                                                                    PrinterCodepage.Windows1252, Encoding.GetEncoding(1252)
                                                                                                  },
                                                                                                  {
                                                                                                    PrinterCodepage.Windows1253, Encoding.GetEncoding(1253)
                                                                                                  },
                                                                                                  {
                                                                                                    PrinterCodepage.Windows1254, Encoding.GetEncoding(1254)
                                                                                                  },
                                                                                                  {
                                                                                                    PrinterCodepage.Windows1255, Encoding.GetEncoding(1255)
                                                                                                  }
                                                                                                };

    [NotNull]
    [Pure]
    public override Encoding GetEncoding()
    {
      var encoding = this.PrinterCodepageToEncodingMappings[this.PrinterCodepage];

      return encoding;
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgDocument" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    [Pure]
    public override EplContainer GetTranslation(SvgDocument svgDocument,
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
      var eplContainer = new EplContainer();
      this.AddBodyToTranslation(svgDocument,
                                sourceMatrix,
                                viewMatrix,
                                eplContainer);
      this.AddHeaderToTranslation(svgDocument,
                                  sourceMatrix,
                                  viewMatrix,
                                  eplContainer);
      this.AddFooterToTranslation(svgDocument,
                                  sourceMatrix,
                                  viewMatrix,
                                  eplContainer);

      return eplContainer;
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgDocument" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplContainer" /> is <see langword="null" />.</exception>
    protected virtual void AddHeaderToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix sourceMatrix,
                                                  [NotNull] Matrix viewMatrix,
                                                  [NotNull] EplContainer eplContainer)
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
      if (eplContainer == null)
      {
        throw new ArgumentNullException(nameof(eplContainer));
      }

      eplContainer.Header.Add(this.EplCommands.SetReferencePoint(0,
                                                                 0));
      eplContainer.Header.Add(this.EplCommands.PrintDirection(PrintOrientation.Top));
      eplContainer.Header.Add(this.EplCommands.CharacterSetSelection(8,
                                                                     this.PrinterCodepage,
                                                                     this.CountryCode));
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgDocument" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplContainer" /> is <see langword="null" />.</exception>
    protected virtual void AddBodyToTranslation([NotNull] SvgDocument svgDocument,
                                                [NotNull] Matrix sourceMatrix,
                                                [NotNull] Matrix viewMatrix,
                                                [NotNull] EplContainer eplContainer)
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
      if (eplContainer == null)
      {
        throw new ArgumentNullException(nameof(eplContainer));
      }

      eplContainer.Body.Add(string.Empty);
      eplContainer.Body.Add(this.EplCommands.ClearImageBuffer());
      this.TranslateSvgElementAndChildren(svgDocument,
                                          sourceMatrix,
                                          viewMatrix,
                                          eplContainer);
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgDocument" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplContainer" /> is <see langword="null" />.</exception>
    protected virtual void AddFooterToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix sourceMatrix,
                                                  [NotNull] Matrix viewMatrix,
                                                  [NotNull] EplContainer eplContainer)
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
      if (eplContainer == null)
      {
        throw new ArgumentNullException(nameof(eplContainer));
      }

      eplContainer.Footer.Add(this.EplCommands.Print(1));
      eplContainer.Footer.Add(string.Empty);
    }
  }
}
