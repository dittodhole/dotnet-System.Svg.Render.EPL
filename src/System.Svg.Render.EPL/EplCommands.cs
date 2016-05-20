using System.Collections.Generic;
using System.Drawing;
using ImageMagick;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString

namespace System.Svg.Render.EPL
{
  [PublicAPI]
  public class EplCommands
  {
    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual EplStream CreateEplStream() => new EplStream();

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual EplStream GraphicDirectWrite([NotNull] Bitmap bitmap,
                                                int horizontalStart,
                                                int verticalStart)
    {
      var eplStream = this.CreateEplStream();
      var octetts = (int) Math.Ceiling(bitmap.Width / 8f);
      eplStream.Add($"GW{horizontalStart},{verticalStart},{octetts},{bitmap.Height}");
      eplStream.Add(this.GetRawBinaryData(bitmap,
                                          octetts));

      return eplStream;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual IEnumerable<byte> GetRawBinaryData([NotNull] Bitmap bitmap,
                                                      int octetts)
    {
      // TODO merge with MagickImage, as we are having different thresholds here

      var height = bitmap.Height;
      var width = bitmap.Width;

      for (var y = 0;
           y < height;
           y++)
      {
        for (var octett = 0;
             octett < octetts;
             octett++)
        {
          var value = (int) byte.MaxValue;

          for (var i = 0;
               i < 8;
               i++)
          {
            var x = octett * 8 + i;
            var bitIndex = 7 - i;
            if (x < width)
            {
              var color = bitmap.GetPixel(x,
                                          y);
              if (color.A > 0x32
                  || color.R > 0x96 && color.G > 0x96 && color.B > 0x96)
              {
                value &= ~(1 << bitIndex);
              }
            }
          }

          yield return (byte) value;
        }
      }
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string DeleteGraphics([NotNull] string name)
    {
      return $@"GK""{name}""";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual EplStream StoreGraphics([NotNull] Bitmap bitmap,
                                           [NotNull] string name)
    {
      MagickImage magickImage;

      var mod = bitmap.Width % 8;
      if (mod > 0)
      {
        var newWidth = bitmap.Width + 8 - mod;
        using (var resizedBitmap = new Bitmap(newWidth,
                                              bitmap.Height))
        {
          using (var graphics = Graphics.FromImage(resizedBitmap))
          {
            graphics.DrawImageUnscaled(bitmap,
                                       0,
                                       0);
            graphics.Save();
          }

          magickImage = new MagickImage(resizedBitmap);
        }
      }
      else
      {
        magickImage = new MagickImage(bitmap);
      }

      byte[] array;
      using (magickImage)
      {
        // TODO threshold
        magickImage.ColorType = ColorType.Bilevel;
        magickImage.Negate();
        array = magickImage.ToByteArray(MagickFormat.Pcx);
      }

      var eplStream = this.CreateEplStream();
      eplStream.Add($@"GM""{name}""{array.Length}");
      eplStream.Add(array);

      return eplStream;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual EplStream PrintGraphics(int horizontalStart,
                                           int verticalStart,
                                           [NotNull] string name)
    {
      var eplStream = this.CreateEplStream();
      eplStream.Add($@"GG{horizontalStart},{verticalStart},""{name}""");

      return eplStream;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual EplStream LineDrawBlack(int horizontalStart,
                                           int verticalStart,
                                           int horizontalLength,
                                           int verticalLength)
    {
      var eplStream = this.CreateEplStream();
      eplStream.Add($"LO{horizontalStart},{verticalStart},{horizontalLength},{verticalLength}");

      return eplStream;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual EplStream LineDrawWhite(int horizontalStart,
                                           int verticalStart,
                                           int horizontalLength,
                                           int verticalLength)
    {
      var eplStream = this.CreateEplStream();
      eplStream.Add($"LW{horizontalStart},{verticalStart},{horizontalLength},{verticalLength}");

      return eplStream;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual EplStream LineDrawDiagonal(int horizontalStart,
                                              int verticalStart,
                                              int horizontalLength,
                                              int verticalLength,
                                              int verticalEnd)
    {
      var eplStream = this.CreateEplStream();
      eplStream.Add($"LS{horizontalStart},{verticalStart},{horizontalLength},{verticalLength},{verticalEnd}");

      return eplStream;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual EplStream DrawBox(int horizontalStart,
                                     int verticalStart,
                                     int lineThickness,
                                     int horizontalEnd,
                                     int verticalEnd)
    {
      var eplStream = this.CreateEplStream();
      eplStream.Add($"X{horizontalStart},{verticalStart},{lineThickness},{horizontalEnd},{verticalEnd}");

      return eplStream;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual EplStream AsciiText(int horizontalStart,
                                       int verticalStart,
                                       int rotation,
                                       int fontSelection,
                                       int horizontalMulitplier,
                                       int verticalMulitplier,
                                       bool invert,
                                       [NotNull] string text)
    {
      string reverseImage;
      if (invert)
      {
        reverseImage = "R";
      }
      else
      {
        reverseImage = "N";
      }

      var eplStream = this.CreateEplStream();
      eplStream.Add($@"A{horizontalStart},{verticalStart},{rotation},{fontSelection},{horizontalMulitplier},{verticalMulitplier},{reverseImage},""{text}""");

      return eplStream;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual EplStream BarCode(int horizontalStart,
                                     int verticalStart,
                                     int rotation,
                                     BarCodeSelection barCodeSelection,
                                     int narrowBarWidth,
                                     int wideBarWidth,
                                     int height,
                                     PrintHumanReadable printHumanReadable,
                                     [NotNull] string content)
    {
      var barcode = this.GetBarCodeSelection(barCodeSelection);

      var eplStream = this.CreateEplStream();
      eplStream.Add($@"B{horizontalStart},{verticalStart},{rotation},{barcode},{narrowBarWidth},{wideBarWidth},{height},{(char) printHumanReadable},""{content}""");

      return eplStream;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected string GetBarCodeSelection(BarCodeSelection barCodeSelection)
    {
      switch (barCodeSelection)
      {
        case BarCodeSelection.Code39:
          return "3";
        case BarCodeSelection.Code39WithCheckDigit:
          return "3C";
        case BarCodeSelection.Code93:
          return "9";
        case BarCodeSelection.Code128UCC:
          return "0";
        case BarCodeSelection.Code128Auto:
          return "1";
        case BarCodeSelection.Code128A:
          return "1A";
        case BarCodeSelection.Code128B:
          return "1B";
        case BarCodeSelection.Code128C:
          return "1C";
        case BarCodeSelection.Interleaved2Of5:
          return "2";
        case BarCodeSelection.Interleaved2Of5WithMod10CheckDigit:
          return "2C";
        case BarCodeSelection.Interleaved2Of5WithHumanReadableCheckDigit:
          return "2D";
        default:
          // TODO !
          // :beers: should never happen
          throw new ArgumentOutOfRangeException(nameof(barCodeSelection),
                                                barCodeSelection,
                                                null);
      }
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string SetReferencePoint(int horizontalStart,
                                            int verticalStart)
    {
      return $"R{horizontalStart},{verticalStart}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string PrintDirection(PrintOrientation printOrientation)
    {
      return $"Z{(char) printOrientation}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Print(int copies)
    {
      return $"P{copies}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string CharacterSetSelection(int bytes,
                                                PrinterCodepage printerCodepage,
                                                int countryCode)
    {
      var codepage = this.GetPrinterCodepage(printerCodepage);

      return $"I{bytes},{codepage},{countryCode}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual string GetPrinterCodepage(PrinterCodepage printerCodepage)
    {
      switch (printerCodepage)
      {
        case PrinterCodepage.Dos347:
          return "0";
        case PrinterCodepage.Dos850:
          return "1";
        case PrinterCodepage.Dos852:
          return "2";
        case PrinterCodepage.Dos860:
          return "3";
        case PrinterCodepage.Dos863:
          return "4";
        case PrinterCodepage.Dos865:
          return "5";
        case PrinterCodepage.Dos857:
          return "6";
        case PrinterCodepage.Dos861:
          return "7";
        case PrinterCodepage.Dos862:
          return "8";
        case PrinterCodepage.Dos855:
          return "9";
        case PrinterCodepage.Dos866:
          return "10";
        case PrinterCodepage.Dos737:
          return "11";
        case PrinterCodepage.Dos851:
          return "12";
        case PrinterCodepage.Dos869:
          return "13";
        case PrinterCodepage.Windows1252:
          return "A";
        case PrinterCodepage.Windows1250:
          return "B";
        case PrinterCodepage.Windows1251:
          return "C";
        case PrinterCodepage.Windows1253:
          return "D";
        case PrinterCodepage.Windows1254:
          return "E";
        case PrinterCodepage.Windows1255:
          return "F";
        default:
          // TODO !
          // :beers: should never happen
          throw new ArgumentOutOfRangeException(nameof(printerCodepage),
                                                printerCodepage,
                                                null);
      }
    }
  }
}