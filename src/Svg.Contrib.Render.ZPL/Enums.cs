using JetBrains.Annotations;

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public enum LineColor
  {
    Black = 'B',
    White = 'W'
  }

  [PublicAPI]
  public enum Orientation
  {
    RightLeaningDiagnoal = 'R',
    LeftLeaningDiagonal = 'L'
  }

  [PublicAPI]
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
    ZebraCodePage1252 = 27
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
}
