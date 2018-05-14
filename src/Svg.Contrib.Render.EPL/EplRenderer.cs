using System;
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
    [Pure]
    public Encoding GetEncoding()
    {
      var codepage = (int) this.PrinterCodepage;
      var encoding = Encoding.GetEncoding(codepage);

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
