using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
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
      return $"PP {horizontalStart},{verticalStart}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Box(int width,
                              int height,
                              int lineWeight)
    {
      return $"PX {height},{width},{lineWeight}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Line(int length,
                               int lineWeight)
    {
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
    public virtual string Align(Alignment alignment)
    {
      return $"AN {(int) alignment}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string SelectCharacterSet(CharacterSet characterSet)
    {
      return $"NASC {characterSet.ToString("D")}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string VerbOff()
    {
      return "VERBOFF";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string InputOff()
    {
      return "INPUT OFF";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string NormalImage()
    {
      return "NI";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string InvertImage()
    {
      return "INVIMAGE";
    }
  }
}