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
    [ItemNotNull]
    private IDictionary<BarCodeSelection, string> BarCodeSelectionMappings { get; } = new Dictionary<BarCodeSelection, string>
                                                                                      {
                                                                                        {
                                                                                          BarCodeSelection.Code39, "3"
                                                                                        },
                                                                                        {
                                                                                          BarCodeSelection.Code39WithCheckDigit, "3C"
                                                                                        },
                                                                                        {
                                                                                          BarCodeSelection.Code93, "9"
                                                                                        },
                                                                                        {
                                                                                          BarCodeSelection.Code128UCC, "0"
                                                                                        },
                                                                                        {
                                                                                          BarCodeSelection.Code128Auto, "1"
                                                                                        },
                                                                                        {
                                                                                          BarCodeSelection.Code128A, "1A"
                                                                                        },
                                                                                        {
                                                                                          BarCodeSelection.Code128B, "1B"
                                                                                        },
                                                                                        {
                                                                                          BarCodeSelection.Code128C, "1C"
                                                                                        },
                                                                                        {
                                                                                          BarCodeSelection.Interleaved2Of5, "2"
                                                                                        },
                                                                                        {
                                                                                          BarCodeSelection.Interleaved2Of5WithMod10CheckDigit, "2C"
                                                                                        },
                                                                                        {
                                                                                          BarCodeSelection.Interleaved2Of5WithHumanReadableCheckDigit, "2D"
                                                                                        }
                                                                                      };

    [NotNull]
    [ItemNotNull]
    private IDictionary<PrinterCodepage, string> PrinterCodepageMappings { get; } = new Dictionary<PrinterCodepage, string>
                                                                                    {
                                                                                      {
                                                                                        PrinterCodepage.Dos347, "0"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Dos850, "1"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Dos852, "2"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Dos860, "3"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Dos863, "4"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Dos865, "5"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Dos857, "6"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Dos861, "7"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Dos862, "8"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Dos855, "9"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Dos866, "10"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Dos737, "11"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Dos851, "12"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Dos869, "13"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Windows1252, "A"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Windows1250, "B"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Windows1251, "C"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Windows1253, "D"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Windows1254, "E"
                                                                                      },
                                                                                      {
                                                                                        PrinterCodepage.Windows1255, "F"
                                                                                      }
                                                                                    };

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
      var octetts = (int) Math.Ceiling(bitmap.Width / 8f);

      var eplStream = this.CreateEplStream();
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
    public virtual string PrintGraphics(int horizontalStart,
                                        int verticalStart,
                                        [NotNull] string name)
    {
      return $@"GG{horizontalStart},{verticalStart},""{name}""";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string LineDrawBlack(int horizontalStart,
                                        int verticalStart,
                                        int horizontalLength,
                                        int verticalLength)
    {
      return $"LO{horizontalStart},{verticalStart},{horizontalLength},{verticalLength}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string LineDrawWhite(int horizontalStart,
                                        int verticalStart,
                                        int horizontalLength,
                                        int verticalLength)
    {
      return $"LW{horizontalStart},{verticalStart},{horizontalLength},{verticalLength}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string LineDrawDiagonal(int horizontalStart,
                                           int verticalStart,
                                           int horizontalLength,
                                           int verticalLength,
                                           int verticalEnd)
    {
      return $"LS{horizontalStart},{verticalStart},{horizontalLength},{verticalLength},{verticalEnd}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string DrawBox(int horizontalStart,
                                  int verticalStart,
                                  int lineThickness,
                                  int horizontalEnd,
                                  int verticalEnd)
    {
      return $"X{horizontalStart},{verticalStart},{lineThickness},{horizontalEnd},{verticalEnd}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string AsciiText(int horizontalStart,
                                    int verticalStart,
                                    int rotation,
                                    int fontSelection,
                                    int horizontalMulitplier,
                                    int verticalMulitplier,
                                    ReverseImage reverseImage,
                                    [NotNull] string text)
    {
      return $@"A{horizontalStart},{verticalStart},{rotation},{fontSelection},{horizontalMulitplier},{verticalMulitplier},{(char) reverseImage},""{text}""";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string BarCode(int horizontalStart,
                                  int verticalStart,
                                  int rotation,
                                  BarCodeSelection barCodeSelection,
                                  int narrowBarWidth,
                                  int wideBarWidth,
                                  int height,
                                  PrintHumanReadable printHumanReadable,
                                  [NotNull] string content)
    {
      var barcode = this.BarCodeSelectionMappings[barCodeSelection];

      return $@"B{horizontalStart},{verticalStart},{rotation},{barcode},{narrowBarWidth},{wideBarWidth},{height},{(char) printHumanReadable},""{content}""";
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
      var codepage = this.PrinterCodepageMappings[printerCodepage];

      return $"I{bytes},{codepage},{countryCode}";
    }
  }
}