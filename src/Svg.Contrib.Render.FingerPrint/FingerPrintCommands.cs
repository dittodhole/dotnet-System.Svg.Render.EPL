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
    public virtual string Box(int height,
                              int width,
                              int lineWeight)
    {
      // PRBOX
      return $"PX {height},{width},{lineWeight}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Line(int length,
                               int lineWeight)
    {
      // PRLINE
      return $"PL {length},{lineWeight}";
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