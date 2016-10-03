using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString
// ReSharper disable ClassWithVirtualMembersNeverInherited.Global

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class FingerPrintCommands
  {
    [NotNull]
    [ItemNotNull]
    private IDictionary<BarCodeType, string> BarCodeTypeMappings { get; } = new Dictionary<BarCodeType, string>
                                                                            {
                                                                              {
                                                                                FingerPrint.BarCodeType.Code128, "CODE128"
                                                                              },
                                                                              {
                                                                                FingerPrint.BarCodeType.Code128A, "CODE128A"
                                                                              },
                                                                              {
                                                                                FingerPrint.BarCodeType.Code128B, "CODE128B"
                                                                              },
                                                                              {
                                                                                FingerPrint.BarCodeType.Code128C, "CODE128C"
                                                                              },
                                                                              {
                                                                                FingerPrint.BarCodeType.Interleaved2Of5, "INT2OF5"
                                                                              },
                                                                              {
                                                                                FingerPrint.BarCodeType.Code39, "CODE39"
                                                                              },
                                                                              {
                                                                                FingerPrint.BarCodeType.Code39FullAscii, "CODE39A"
                                                                              },
                                                                              {
                                                                                FingerPrint.BarCodeType.Code39WithChecksum, "CODE39C"
                                                                              }
                                                                            };

    [NotNull]
    [Pure]
    public virtual string Position(int horizontalStart,
                                   int verticalStart)
    {
      return $"PP {horizontalStart},{verticalStart}";
    }

    [NotNull]
    [Pure]
    public virtual string Box(int width,
                              int height,
                              int lineWeight)
    {
      return $"PX {height},{width},{lineWeight}";
    }

    [NotNull]
    [Pure]
    public virtual string Line(int length,
                               int lineWeight)
    {
      return $"PL {length},{lineWeight}";
    }

    [NotNull]
    [Pure]
    public virtual string PrintFeed(int copies = 1)
    {
      return $"PF {copies}";
    }

    /// <exception cref="ArgumentNullException"><paramref name="text" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual string PrintText([NotNull] string text)
    {
      if (text == null)
      {
        throw new ArgumentNullException(nameof(text));
      }

      return $@"PT ""{text}""";
    }

    /// <exception cref="ArgumentNullException"><paramref name="fontName" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual string Font([NotNull] string fontName,
                               int height,
                               int slant)
    {
      if (fontName == null)
      {
        throw new ArgumentNullException(nameof(fontName));
      }

      return $@"FT ""{fontName}"",{height},{slant}";
    }

    [NotNull]
    [Pure]
    public virtual string Direction(Direction direction)
    {
      return $"DIR {(int) direction}";
    }

    [NotNull]
    [Pure]
    public virtual string Align(Alignment alignment)
    {
      return $"AN {(int) alignment}";
    }

    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual string ImageLoad([NotNull] string name,
                                    int totalNumberOfBytes)
    {
      if (name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      var skip = Environment.NewLine.ToCharArray()
                            .Count() - 1;
      return $@"IMAGE LOAD {skip},""{name}"",{totalNumberOfBytes},""""";
    }

    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual string PrintImage([NotNull] string name)
    {
      if (name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      return $@"PM ""{name}""";
    }

    [NotNull]
    [Pure]
    public virtual string SelectCharacterSet(CharacterSet characterSet)
    {
      return $"NASC {characterSet.ToString("D")}";
    }

    [NotNull]
    [Pure]
    public virtual string VerbOff()
    {
      return "VERBOFF";
    }

    [NotNull]
    [Pure]
    public virtual string InputOff()
    {
      return "INPUT OFF";
    }

    [NotNull]
    [Pure]
    public virtual string ImmediateOn()
    {
      return "IMMEDIATE ON";
    }

    [NotNull]
    [Pure]
    public virtual string NormalImage()
    {
      return "NI";
    }

    [NotNull]
    [Pure]
    public virtual string InvertImage()
    {
      return "INVIMAGE";
    }

    /// <exception cref="ArgumentNullException"><paramref name="name" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual string RemoveImage([NotNull] string name)
    {
      if (name == null)
      {
        throw new ArgumentNullException(nameof(name));
      }

      return $@"REMOVE IMAGE ""{name}""";
    }

    [NotNull]
    [Pure]
    public virtual string PrintBuffer(int totalNumberOfBytes)
    {
      return $"PRBUF {totalNumberOfBytes}";
    }

    /// <exception cref="ArgumentNullException"><paramref name="data" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual string PrintBarCode([NotNull] string data)
    {
      if (data == null)
      {
        throw new ArgumentNullException(nameof(data));
      }

      return $@"PB ""{data}""";
    }

    [NotNull]
    [Pure]
    public virtual string BarCodeMagnify(int widthFactor)
    {
      return $"BM {widthFactor}";
    }

    [NotNull]
    [Pure]
    public virtual string BarCodeHeight(int height)
    {
      return $"BH {height}";
    }

    [NotNull]
    [Pure]
    public virtual string BarCodeRatio(int wideBarFactor,
                                       decimal narrowBarFactor)
    {
      return $"BR {wideBarFactor},{narrowBarFactor}";
    }

    [NotNull]
    [Pure]
    public virtual string BarCodeType(BarCodeType barCodeType)
    {
      var barcode = this.BarCodeTypeMappings[barCodeType];

      return $@"BT ""{barcode}""";
    }
  }
}
