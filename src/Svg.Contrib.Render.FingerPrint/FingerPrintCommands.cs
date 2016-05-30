using JetBrains.Annotations;

// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class FingerPrintCommands
  {
    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Position(int x,
                                   int y)
    {
      // PRPOS
      return $"PP {y},{x}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Box(int width,
                              int height,
                              int lineWeight)
    {
      // PRBOX
      return $"PX {width},{height},{lineWeight}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Line(int length,
                               int lineWeight)
    {
      // PRLINE
      return $"PL {lineWeight},{length}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Direction(Direction direction)
    {
      return $"DIR {(int) direction}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string PrintFeed(int copies = 1)
    {
      return $"PF {copies}";
    }
  }
}