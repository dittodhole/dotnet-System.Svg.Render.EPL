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
    ZebraCodePage850 = 13,
    ZebraCodePage1252 = 27,
    Utf8 = 28,
    Utf16BigEndian = 29,
    Utf16LittleEndian = 30,
    ZebraCodePage1250 = 31,
    CodePage1251 = 33,
    CodePage1253 = 34,
    CodePage1254 = 35,
    CodePage1255 = 36
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
