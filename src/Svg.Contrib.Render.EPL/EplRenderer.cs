using System.Drawing.Drawing2D;
using Svg;
using System.Text;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class EplRenderer : RendererBase<EplStream>
  {
    public EplRenderer([NotNull] Matrix viewMatrix,
                       [NotNull] EplCommands eplCommands,
                       PrinterCodepage printerCodepage,
                       int countryCode)
    {
      this.ViewMatrix = viewMatrix;
      this.EplCommands = eplCommands;
      this.PrinterCodepage = printerCodepage;
      this.CountryCode = countryCode;
    }

    [NotNull]
    protected Matrix ViewMatrix { get; }

    [NotNull]
    protected EplCommands EplCommands { get; }

    protected PrinterCodepage PrinterCodepage { get; }

    protected int CountryCode { get; }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public Encoding GetEncoding()
    {
      var codepage = (int) this.PrinterCodepage;
      // ReSharper disable ExceptionNotDocumentedOptional
      var encoding = Encoding.GetEncoding(codepage);
      // ReSharper restore ExceptionNotDocumentedOptional

      return encoding;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual Matrix CreateParentMatrix() => new Matrix();

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public override EplStream GetTranslation([NotNull] SvgDocument svgDocument)
    {
      var parentMatrix = this.CreateParentMatrix();
      var result = this.EplCommands.CreateEplStream();

      var streamContainer = new Container<EplStream>(this.EplCommands.CreateEplStream(),
                                                     this.EplCommands.CreateEplStream(),
                                                     this.EplCommands.CreateEplStream());
      this.AddBodyToTranslation(svgDocument,
                                parentMatrix,
                                streamContainer);
      this.AddHeaderToTranslation(svgDocument,
                                  parentMatrix,
                                  streamContainer);
      this.AddFooterToTranslation(svgDocument,
                                  parentMatrix,
                                  streamContainer);

      result.Add(streamContainer.Header);
      result.Add(streamContainer.Body);
      result.Add(streamContainer.Footer);

      return result;
    }

    protected virtual void AddHeaderToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix parentMatrix,
                                                  [NotNull] Container<EplStream> container)
    {
      container.Header.Add(this.EplCommands.SetReferencePoint(0,
                                                              0));
      container.Header.Add(this.EplCommands.PrintDirection(PrintOrientation.Top));
      container.Header.Add(this.EplCommands.CharacterSetSelection(8,
                                                                  this.PrinterCodepage,
                                                                  this.CountryCode));
    }

    protected virtual void AddBodyToTranslation([NotNull] SvgDocument svgDocument,
                                                [NotNull] Matrix parentMatrix,
                                                [NotNull] Container<EplStream> container)
    {
      container.Body.Add(string.Empty);
      container.Body.Add(this.EplCommands.ClearImageBuffer());
      this.TranslateSvgElementAndChildren(svgDocument,
                                          parentMatrix,
                                          this.ViewMatrix,
                                          container);
    }

    protected virtual void AddFooterToTranslation([NotNull] SvgDocument svgDocument,
                                                  [NotNull] Matrix parentMatrix,
                                                  [NotNull] Container<EplStream> container)
    {
      container.Footer.Add(this.EplCommands.Print(1));
      container.Footer.Add(string.Empty);
    }
  }
}