using JetBrains.Annotations;

namespace System.Svg.Render.ZPL
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
    // TODO implement stuff
    //USA1 = 0,
    //USA2 = 1,
    //UK = 2,
    //Holland = 3,
    //Denmark = 4,
    //Norway = 4,
    //Sweden = 5,
    //Finnland = 5,
    //Germany = 6,
    //France1 = 7,
    //France2 = 8,
    //Italy = 9,
    //Spain = 10,
    //Miscellaneous = 11,
    //Japan = 12,
    ZebraCodePage850 = 13,
    //DoubleByteAsian = 14,
    //ShiftJIS = 15,
    //EUCJP = 16,
    //EUCCN = 16,

    //[Obsolete]
    //UCS2BigEndian = 17,
    //SingleByteAsian = 24,
    //MultibyteAsian = 26,
    ZebraCodePage1252 = 27
  }

  [PublicAPI]
  public enum ViewRotation
  {
    Normal = 0,
    RotateBy90Degrees = 90,
    RotateBy180Degrees = 180,
    RotateBy270Degress = 270
  }
}