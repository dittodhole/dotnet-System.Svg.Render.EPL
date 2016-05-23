using JetBrains.Annotations;

// ReSharper disable NonLocalizedString

namespace System.Svg.Render.ZPL
{
  [PublicAPI]
  public class ZplCommands
  {
    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual ZplStream CreateZplStream() => new ZplStream();

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string FieldOrigin(int horizontalStart,
                                      int verticalStart)
    {
      return $"^FO{horizontalStart},{verticalStart}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual ZplStream GraphicBox(int horizontalStart,
                                        int verticalStart,
                                        int width,
                                        int height,
                                        int thickness,
                                        LineColor lineColor)
    {
      var zplStream = this.CreateZplStream();
      zplStream.Add(this.FieldOrigin(horizontalStart,
                                     verticalStart));
      zplStream.Add($"^GB{width},{height},{thickness},{(char) lineColor}^FS");

      return zplStream;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual ZplStream GraphicDiagonalLine(int horizontalStart,
                                                 int verticalStart,
                                                 int width,
                                                 int height,
                                                 int thickness,
                                                 LineColor lineColor,
                                                 Orientation orientation)
    {
      var zplStream = this.CreateZplStream();
      zplStream.Add(this.FieldOrigin(horizontalStart,
                                     verticalStart));
      zplStream.Add($"^GD{width},{height},{thickness},{(char) lineColor},{(char) orientation}^FS");

      return zplStream;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual ZplStream Font(int horizontalStart,
                                  int verticalStart,
                                  [NotNull] string fontName,
                                  FieldOrientation fieldOrientation,
                                  int characterHeight,
                                  int width,
                                  string text)
    {
      var zplStream = this.CreateZplStream();
      zplStream.Add(this.FieldOrigin(horizontalStart,
                                     verticalStart));
      zplStream.Add($"^A{fontName}{(char) fieldOrientation},{characterHeight},{width}");
      zplStream.Add($"^FD{text}^FS");

      return zplStream;
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string StartFormat()
    {
      return "^XA";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string EndFormat()
    {
      return "^XZ";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string PrintOrientation(PrintOrientation printOrientation)
    {
      return $"^PO{(char) printOrientation}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string LabelHome(int horizontalStart,
                                    int verticalStart)
    {
      return $"^LH{horizontalStart},{verticalStart}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string ChangeInternationalFont(CharacterSet characterSet)
    {
      // ReSharper disable ExceptionNotDocumentedOptional
      return $"^CI{characterSet.ToString("D")}";
      // ReSharper restore ExceptionNotDocumentedOptional
    }
  }
}