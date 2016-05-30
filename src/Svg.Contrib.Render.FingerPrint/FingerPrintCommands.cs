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
    public virtual string Position(int horizontalStart,
                                   int verticalStart)
    {
      // PRPOS
      return $"PP {horizontalStart},{verticalStart}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Box(int width,
                              int height,
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
    public virtual string PrintFeed(int copies = 1)
    {
      return $"PF {copies}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string PrintText([NotNull] string text)
    {
      return $@"PT ""{text}""";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Font([NotNull] string fontName,
                               int height,
                               int slant)
    {
      return $@"FT ""{fontName}"",{height},{slant}";
    }
  }
}