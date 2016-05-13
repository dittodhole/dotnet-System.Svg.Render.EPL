using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class EplRenderer : RendererBase
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

    [NotNull]
    public override IEnumerable<byte> GetTranslation([NotNull] SvgDocument instance)
    {
      var translation = this.GetTranslation(instance,
                                            this.ViewMatrix);
      var result = this.Encoding.GetBytes("R0,0")
                       .Concat(this.Encoding.GetBytes(Environment.NewLine))
                       .Concat(this.Encoding.GetBytes("ZT"))
                       .Concat(this.Encoding.GetBytes(Environment.NewLine))
                       .Concat(translation)
                       .Concat(this.Encoding.GetBytes("P1"))
                       .Concat(this.Encoding.GetBytes(Environment.NewLine))
                       .Concat(this.Encoding.GetBytes(Environment.NewLine));

      return result;
    }

    [NotNull]
    protected override IEnumerable<byte> TranslateSvgElement(SvgElement svgElement,
                                                             Matrix matrix,
                                                             Matrix viewMatrix)
    {
      var result = base.TranslateSvgElement(svgElement,
                                            matrix,
                                            viewMatrix);
      if (result == null)
      {
        return Enumerable.Empty<byte>();
      }
      result = result.Concat(this.Encoding.GetBytes(Environment.NewLine));

      return result;
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