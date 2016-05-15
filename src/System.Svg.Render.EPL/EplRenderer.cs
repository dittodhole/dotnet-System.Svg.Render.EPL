using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Text;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString

namespace System.Svg.Render.EPL
{
  public class EplRenderer : RendererBase<EplStream>
  {
    public EplRenderer([NotNull] Matrix viewMatrix,
                       PrinterCodepage printerCodepage,
                       int countryCode)
    {
      this.ViewMatrix = viewMatrix;
      this.PrinterCodepage = printerCodepage;
      this.CountryCode = countryCode;
      this.Encoding = this.CreateEncoding();
    }

    [NotNull]
    protected Matrix ViewMatrix { get; }

    protected PrinterCodepage PrinterCodepage { get; }

    [NotNull]
    private Encoding Encoding { get; }

    [NotNull]
    protected int CountryCode { get; }

    [NotNull]
    public Encoding GetEncoding()
    {
      return this.Encoding;
    }

    [NotNull]
    public virtual IEnumerable<EplStream> GetInternalMemoryTranslation([NotNull] SvgDocument svgDocument)
    {
      var parentMatrix = this.CreateParentMatrix();
      var translations = this.TranslaveSvgElementAndChildrenForStoring(svgDocument,
                                                                       parentMatrix,
                                                                       this.ViewMatrix);

      return translations;
    }

    [NotNull]
    protected virtual IEnumerable<EplStream> TranslaveSvgElementAndChildrenForStoring([NotNull] SvgElement svgElement,
                                                                                      [NotNull] Matrix parentMatrix,
                                                                                      [NotNull] Matrix viewMatrix)
    {
      var matrix = this.MultiplyTransformationsIntoNewMatrix(svgElement,
                                                             parentMatrix);

      {
        var eplStream = this.TranslateSvgElementForStoring(svgElement,
                                                           matrix,
                                                           viewMatrix);
        if (!eplStream.IsEmpty)
        {
          yield return eplStream;
        }
      }

      foreach (var child in svgElement.Children)
      {
        var translations = this.TranslaveSvgElementAndChildrenForStoring(child,
                                                                         matrix,
                                                                         viewMatrix);

        foreach (var eplStream in translations)
        {
          if (eplStream == null)
          {
            continue;
          }
          if (eplStream.IsEmpty)
          {
            continue;
          }

          yield return eplStream;
        }
      }
    }

    [NotNull]
    protected virtual EplStream CreateEplStream() => new EplStream();

    [NotNull]
    protected virtual Matrix CreateParentMatrix() => new Matrix();

    [NotNull]
    protected virtual EplStream TranslateSvgElementForStoring([NotNull] SvgElement svgElement,
                                                              [NotNull] Matrix matrix,
                                                              [NotNull] Matrix viewMatrix)
    {
      var container = this.CreateEplStream();

      var type = svgElement.GetType();

      var svgElementToInternalMemoryTranslator = this.GetTranslator(type) as ISvgElementToInternalMemoryTranslator;
      if (svgElementToInternalMemoryTranslator == null)
      {
        return null;
      }

      matrix = matrix.Clone();
      matrix.Multiply(viewMatrix,
                      MatrixOrder.Append);

      svgElementToInternalMemoryTranslator.TranslateForStoring(svgElement,
                                                               matrix,
                                                               container);

      return container;
    }

    [NotNull]
    public override EplStream GetTranslation([NotNull] SvgDocument svgDocument)
    {
      var parentMatrix = this.CreateParentMatrix();
      var eplStream = this.CreateEplStream();

      eplStream.Add("R0,0");
      eplStream.Add("ZT");

      this.TranslateSvgElementAndChildren(svgDocument,
                                          parentMatrix,
                                          this.ViewMatrix,
                                          eplStream);

      eplStream.Add("P1");
      eplStream.Add(string.Empty);

      return eplStream;
    }

    [NotNull]
    private Encoding CreateEncoding()
    {
      switch (this.PrinterCodepage)
      {
        case PrinterCodepage.Dos347:
          return Encoding.GetEncoding(347);
        case PrinterCodepage.Dos850:
          return Encoding.GetEncoding(850);
        case PrinterCodepage.Dos852:
          return Encoding.GetEncoding(852);
        case PrinterCodepage.Dos860:
          return Encoding.GetEncoding(860);
        case PrinterCodepage.Dos863:
          return Encoding.GetEncoding(863);
        case PrinterCodepage.Dos865:
          return Encoding.GetEncoding(865);
        case PrinterCodepage.Dos857:
          return Encoding.GetEncoding(857);
        case PrinterCodepage.Dos861:
          return Encoding.GetEncoding(861);
        case PrinterCodepage.Dos862:
          return Encoding.GetEncoding(862);
        case PrinterCodepage.Dos855:
          return Encoding.GetEncoding(855);
        case PrinterCodepage.Dos737:
          return Encoding.GetEncoding(737);
        case PrinterCodepage.Dos851:
          return Encoding.GetEncoding(851);
        case PrinterCodepage.Dos869:
          return Encoding.GetEncoding(869);
        case PrinterCodepage.Windows1252:
          return Encoding.GetEncoding(1252);
        case PrinterCodepage.Windows1250:
          return Encoding.GetEncoding(1250);
        case PrinterCodepage.Windows1251:
          return Encoding.GetEncoding(1251);
        case PrinterCodepage.Windows1253:
          return Encoding.GetEncoding(1253);
        case PrinterCodepage.Windows1254:
          return Encoding.GetEncoding(1254);
        case PrinterCodepage.Windows1255:
          return Encoding.GetEncoding(1255);
        default:
          // TODO !
          // :beers: should never happen
          throw new ArgumentOutOfRangeException();
      }
    }
  }
}