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
    private Matrix ViewMatrix { get; }

    private PrinterCodepage PrinterCodepage { get; }

    [NotNull]
    private Encoding Encoding { get; }

    [NotNull]
    private int CountryCode { get; }

    [NotNull]
    public Encoding GetEncoding()
    {
      return this.Encoding;
    }

    // TODO
    /*
    [NotNull]
    public IEnumerable<IEnumerable<byte>> GetInternalMemoryTranslation([NotNull] SvgDocument svgDocument)
    {
      var parentMatrix = new Matrix();
      var translations = this.TranslateForInternalMemory(svgDocument,
                                                         parentMatrix,
                                                         this.ViewMatrix);

      return translations;
    }

    [NotNull]
    private IEnumerable<IEnumerable<byte>> TranslateForInternalMemory([NotNull] SvgElement svgElement,
                                                                      [NotNull] Matrix parentMatrix,
                                                                      [NotNull] Matrix viewMatrix)
    {
      var matrix = this.MultiplyTransformationsIntoNewMatrix(svgElement,
                                                             parentMatrix);

      var type = svgElement.GetType();
      var translator = this.GetTranslator<ISvgElementToInternalMemoryTranslator>(type);
      if (translator != null)
      {
        var concreteMatrix = matrix.Clone();
        concreteMatrix.Multiply(viewMatrix,
                                MatrixOrder.Append);

        var translation = translator.TranslateForStoring(svgElement,
                                                         concreteMatrix);
        if (translation != null)
        {
          translation = translation.Concat(this.Encoding.GetBytes(Environment.NewLine));
          yield return translation;
        }
      }

      foreach (var child in svgElement.Children)
      {
        var translations = this.TranslateForInternalMemory(child,
                                                           matrix,
                                                           viewMatrix);

        foreach (var translation in translations)
        {
          yield return translation;
        }
      }
    }
    */

    [NotNull]
    public override EplStream GetTranslation([NotNull] SvgDocument svgDocument)
    {
      var parentMatrix = new Matrix();
      var eplStream = new EplStream();

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