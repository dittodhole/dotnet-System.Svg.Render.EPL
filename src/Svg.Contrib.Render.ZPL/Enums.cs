using JetBrains.Annotations;

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public enum LineColor
  {
    Black = 'B',
    White = 'W'
  }

  public enum FieldOrientation
  {
    Normal = 'N',
    RotatedBy90Degrees = 'R',
    RotatedBy180Degrees = 'I',
    RotatedBy270Degrees = 'B'
  }

  [PublicAPI]
  public enum PrintOrientation
  {
    Normal = 'N',
    Invert = 'I'
  }

  [PublicAPI]
  public enum CharacterSet
  {
    ZebraCodePage850,
    ZebraCodePage1252,
    Utf8,
    Utf16BigEndian,
    Utf16LittleEndian,
    ZebraCodePage1250,
    CodePage1251,
    CodePage1253,
    CodePage1254,
    CodePage1255
  }

  [PublicAPI]
  public enum PrintInterpretationLine
  {
    Yes = 'Y',
    No = 'N'
  }

  [PublicAPI]
  public enum PrintInterpretationLineAboveCode
  {
    Yes = 'Y',
    No = 'N'
  }

  [PublicAPI]
  public enum Mode
  {
    NoSelectedMode = 'N',
    UccCaseMode = 'U',
    AutomaticMode = 'A',
    UccEanMode = 'D'
  }

  [PublicAPI]
  public enum CalculateAndPrintMod10CheckDigit
  {
    Yes = 'Y',
    No = 'N'
  }

  [PublicAPI]
  public enum UccCheckDigit
  {
    Yes = 'Y',
    No = 'N'
  }

  [PublicAPI]
  public enum Mod43Check
  {
    Yes = 'Y',
    No = 'N'
  }

  [PublicAPI]
  public enum ExtendedChannelInterpretationCodeIndicator
  {
    Yes = 'Y',
    No = 'N'
  }

  [PublicAPI]
  public enum MenuSymbolIndicator
  {
    Yes = 'Y',
    No = 'N'
  }
}
