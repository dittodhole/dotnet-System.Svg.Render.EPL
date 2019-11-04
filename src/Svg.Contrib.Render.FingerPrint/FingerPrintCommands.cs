using System;
using System.Collections.Generic;
using System.Linq;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public class FingerPrintCommands
  {
    [NotNull]
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
                                                                                FingerPrint.BarCodeType.EAN128, "EAN128"
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
    private IDictionary<CharacterSet, int> CharacterSetMappings { get; } = new Dictionary<CharacterSet, int>
                                                                           {
                                                                             {
                                                                               CharacterSet.Utf8, 8
                                                                             },
                                                                             {
                                                                               CharacterSet.Dos850, 850
                                                                             },
                                                                             {
                                                                               CharacterSet.Dos852, 852
                                                                             },
                                                                             {
                                                                               CharacterSet.Dos855, 855
                                                                             },
                                                                             {
                                                                               CharacterSet.Dos857, 857
                                                                             },
                                                                             {
                                                                               CharacterSet.Windows1250, 1250
                                                                             },
                                                                             {
                                                                               CharacterSet.Windows1251, 1251
                                                                             },
                                                                             {
                                                                               CharacterSet.Windows1252, 1252
                                                                             },
                                                                             {
                                                                               CharacterSet.Windows1253, 1253
                                                                             },
                                                                             {
                                                                               CharacterSet.Windows1254, 1254
                                                                             },
                                                                             {
                                                                               CharacterSet.Windows1257, 1257
                                                                             }
                                                                           };

    [NotNull]
    [Pure]
    public virtual string Position(int horizontalStart,
                                   int verticalStart)
    {
      return $"PP {horizontalStart},{verticalStart}"; // PRPOS
    }

    [NotNull]
    [Pure]
    public virtual string Box(int width,
                              int height,
                              int lineWeight)
    {
      return $"PX {height},{width},{lineWeight}"; // PRBOX
    }

    [NotNull]
    [Pure]
    public virtual string Line(int length,
                               int lineWeight)
    {
      return $"PL {length},{lineWeight}"; // PRLINE
    }

    [NotNull]
    [Pure]
    public virtual string PrintFeed(int copies = 1)
    {
      return $"PF {copies}"; // PRINTFEED
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

      return $@"PT ""{text}"""; // PRTXT
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

      return $@"FT ""{fontName}"",{height},{slant}"; // FONT
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
      return $"AN {(int) alignment}"; // ALIGN
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

      return $@"PM ""{name}"""; // PRIMAGE
    }

    [NotNull]
    [Pure]
    public virtual string SelectCharacterSet(CharacterSet characterSet)
    {
      return $"NASC {this.CharacterSetMappings[characterSet]}";
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
      return "NI"; // NORIMAGE
    }

    [NotNull]
    [Pure]
    public virtual string InvertImage()
    {
      return "II"; // INVIMAGE
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

      return $@"PB ""{data}"""; // PRBAR
    }

    [NotNull]
    [Pure]
    public virtual string BarCodeMagnify(int widthFactor)
    {
      return $"BM {widthFactor}"; // BARMAG
    }

    [NotNull]
    [Pure]
    public virtual string BarCodeHeight(int height)
    {
      return $"BH {height}"; // BARHEIGHT
    }

    [NotNull]
    [Pure]
    public virtual string BarCodeRatio(int wideBarFactor,
                                       decimal narrowBarFactor)
    {
      return $"BR {wideBarFactor},{narrowBarFactor}"; // BARRATIO
    }

    [NotNull]
    [Pure]
    public virtual string BarCodeType(BarCodeType barCodeType)
    {
      var barcode = this.BarCodeTypeMappings[barCodeType];

      return $@"BT ""{barcode}"""; // BARTYPE
    }
  }
}
