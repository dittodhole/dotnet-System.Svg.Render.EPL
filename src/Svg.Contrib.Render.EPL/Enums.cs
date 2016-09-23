using JetBrains.Annotations;

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public enum PrinterCodepage
  {
    Dos347 = 347,
    Dos850 = 850,
    Dos852 = 852,
    Dos860 = 860,
    Dos863 = 863,
    Dos865 = 865,
    Dos857 = 857,
    Dos861 = 861,
    Dos862 = 862,
    Dos855 = 855,
    Dos866 = 866,
    Dos737 = 737,
    Dos851 = 851,
    Dos869 = 869,
    Windows1252 = 1252,
    Windows1250 = 1250,
    Windows1251 = 1251,
    Windows1253 = 1253,
    Windows1254 = 1254,
    Windows1255 = 1255
  }

  [PublicAPI]
  public enum PrintOrientation
  {
    Top = 'T',
    Bottom = 'B'
  }

  [PublicAPI]
  public enum BarCodeSelection
  {
    Code128Auto,
    Code128A,
    Code128B,
    Code128C,
    Interleaved2Of5,
    Interleaved2Of5WithMod10CheckDigit,
    Interleaved2Of5WithHumanReadableCheckDigit
  }

  [PublicAPI]
  public enum PrintHumanReadable
  {
    Yes = 'B',
    No = 'N'
  }

  [PublicAPI]
  public enum ReverseImage
  {
    Normal = 'N',
    Reverse = 'R'
  }
}
