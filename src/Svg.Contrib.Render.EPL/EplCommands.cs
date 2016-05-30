using System.Collections.Generic;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class EplCommands
  {
    [NotNull]
    [ItemNotNull]
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
    public virtual string GraphicDirectWrite(int horizontalStart,
                                             int verticalStart,
                                             int numberOfBytesPerRow,
                                             int rows)
    {
      return $"GW{horizontalStart},{verticalStart},{numberOfBytesPerRow},{rows}";
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
    public virtual string StoreGraphics([NotNull] string name,
                                        int length)
    {
      return $@"GM""{name}""{length}";
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
      // ReSharper disable ExceptionNotDocumentedOptional
      var barcode = this.BarCodeSelectionMappings[barCodeSelection];
      // ReSharper restore ExceptionNotDocumentedOptional

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
      // ReSharper disable ExceptionNotDocumentedOptional
      var codepage = this.PrinterCodepageMappings[printerCodepage];
      // ReSharper restore ExceptionNotDocumentedOptional

      return $"I{bytes},{codepage},{countryCode}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string ClearImageBuffer()
    {
      return "N";
    }
  }
}