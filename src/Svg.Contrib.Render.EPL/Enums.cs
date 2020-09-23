using JetBrains.Annotations;

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public enum PrinterCodepage
  {
    Dos850,
    Dos852,
    Dos860,
    Dos863,
    Dos865,
    Dos857,
    Dos861,
    Dos862,
    Dos855,
    Dos866,
    Dos737,
    Dos869,
    Windows1250,
    Windows1251,
    Windows1252,
    Windows1253,
    Windows1254,
    Windows1255
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
    Interleaved2Of5WithHumanReadableCheckDigit,
    Code39,
    Code39WithCheckDigit
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
