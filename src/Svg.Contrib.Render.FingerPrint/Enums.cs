using JetBrains.Annotations;

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public enum Direction
  {
    Direction1 = 1,
    Direction2 = 2,
    Direction3 = 3,
    Direction4 = 4
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
}