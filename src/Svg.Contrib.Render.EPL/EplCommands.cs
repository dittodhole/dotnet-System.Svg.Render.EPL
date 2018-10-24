using System;
using System.Collections.Generic;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class EplCommands
  {
    [NotNull]
    private IDictionary<BarCodeSelection, string> BarCodeSelectionMappings { get; } = new Dictionary<BarCodeSelection, string>
                                                                                      {
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
                                                                                        },
                                                                                        {
                                                                                          BarCodeSelection.Code39, "3"
                                                                                        },
                                                                                        {
                                                                                          BarCodeSelection.Code39WithCheckDigit, "3C"
                                                                                        }
                                                                                      };

    [NotNull]
    private IDictionary<PrinterCodepage, string> PrinterCodepageMappings { get; } = new Dictionary<PrinterCodepage, string>
                                                                                    {
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
    public virtual string GraphicDirectWrite(int horizontalStart,
                                             int verticalStart,
                                             int numberOfBytesPerRow,
                                             int rows)
    {
      return $"GW{horizontalStart},{verticalStart},{numberOfBytesPerRow},{rows}";
    }

    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual string DeleteGraphics([NotNull] string name)
    {
      if (name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      return $@"GK""{name}""";
    }

    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual string StoreGraphics([NotNull] string name,
                                        int length)
    {
      if (name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      return $@"GM""{name}""{length}";
    }

    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual string PrintGraphics(int horizontalStart,
                                        int verticalStart,
                                        [NotNull] string name)
    {
      if (name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      return $@"GG{horizontalStart},{verticalStart},""{name}""";
    }

    [NotNull]
    [Pure]
    public virtual string LineDrawBlack(int horizontalStart,
                                        int verticalStart,
                                        int horizontalLength,
                                        int verticalLength)
    {
      return $"LO{horizontalStart},{verticalStart},{horizontalLength},{verticalLength}";
    }

    [NotNull]
    [Pure]
    public virtual string LineDrawWhite(int horizontalStart,
                                        int verticalStart,
                                        int horizontalLength,
                                        int verticalLength)
    {
      return $"LW{horizontalStart},{verticalStart},{horizontalLength},{verticalLength}";
    }

    [NotNull]
    [Pure]
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
    public virtual string DrawBox(int horizontalStart,
                                  int verticalStart,
                                  int lineThickness,
                                  int horizontalEnd,
                                  int verticalEnd)
    {
      return $"X{horizontalStart},{verticalStart},{lineThickness},{horizontalEnd},{verticalEnd}";
    }

    /// <exception cref="ArgumentNullException"><paramref name="text" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual string AsciiText(int horizontalStart,
                                    int verticalStart,
                                    int rotation,
                                    int fontSelection,
                                    int horizontalMulitplier,
                                    int verticalMulitplier,
                                    ReverseImage reverseImage,
                                    [NotNull] string text)
    {
      if (text == null)
      {
        throw new ArgumentNullException(nameof(text));
      }

      return $@"A{horizontalStart},{verticalStart},{rotation},{fontSelection},{horizontalMulitplier},{verticalMulitplier},{(char) reverseImage},""{text}""";
    }

    /// <exception cref="ArgumentNullException"><paramref name="content" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
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
      if (content == null)
      {
        throw new ArgumentNullException(nameof(content));
      }

      var barcode = this.BarCodeSelectionMappings[barCodeSelection];

      return $@"B{horizontalStart},{verticalStart},{rotation},{barcode},{narrowBarWidth},{wideBarWidth},{height},{(char) printHumanReadable},""{content}""";
    }

    [NotNull]
    [Pure]
    public virtual string SetReferencePoint(int horizontalStart,
                                            int verticalStart)
    {
      return $"R{horizontalStart},{verticalStart}";
    }

    [NotNull]
    [Pure]
    public virtual string PrintDirection(PrintOrientation printOrientation)
    {
      return $"Z{(char) printOrientation}";
    }

    [NotNull]
    [Pure]
    public virtual string Print(int copies)
    {
      return $"P{copies}";
    }

    [NotNull]
    [Pure]
    public virtual string CharacterSetSelection(int bytes,
                                                PrinterCodepage printerCodepage,
                                                int countryCode)
    {
      return $"I{bytes},{this.PrinterCodepageMappings[printerCodepage]},{countryCode}";
    }

    [NotNull]
    [Pure]
    public virtual string ClearImageBuffer()
    {
      return "N";
    }
  }
}
