using JetBrains.Annotations;

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public enum Direction
  {
    Direction1 = 1,
    Direction2 = 2,
    Direction3 = 3,
    Direction4 = 4,
    LeftToRight = Direction.Direction1,
    TopToBottom = Direction.Direction2,
    RightToLeft = Direction.Direction3,
    BottomToTop = Direction.Direction4
  }

  [PublicAPI]
  public enum Alignment
  {
    TopLeft = 7,
    TopMiddle = 8,
    TopRight = 9,
    BaseLineLeft = 4,
    BaseLineMiddle = 5,
    BaseLineRight = 6,
    BottomLeft = 1,
    BottomMiddle = 2,
    BottomRight = 3
  }

  [PublicAPI]
  public enum CharacterSet
  {
    // TODO implement stuff
    //Roman8 = 1,
    Utf8 = 8,
    //French = 33,
    //Spanish = 34,
    //English = 44,
    //Swedish = 46,
    //Norwegian = 47,
    //German = 49,
    //JapaneseLatin = 81,
    //Portuguese = 351,
    //PCMAP = -1,
    //Latin1 = 850,
    //Greek1 = 851,
    //Latin2 = 852,
    //Cyrillic = 855,
    //Turkish = 857,
    //Windows1250 = 1250,
    //Windows1251 = 1251,
    //Windows1252 = 1252,
    //Windows1254 = 1254,
    //Windows1257 = 1257
  }
}