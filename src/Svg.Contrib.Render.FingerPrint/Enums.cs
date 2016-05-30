using JetBrains.Annotations;

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public enum PrintDirection
  {
    Normal = 4,
    RotatedBy90Degrees = 3,
    RotatedBy180Degrees = 2,
    RotatedBy270Degrees = 1
  }
}