using System.Collections.Generic;
using System.Drawing;
using System.Text;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class EplCommands
  {
    public EplCommands([NotNull] Encoding encoding)
    {
      this.Encoding = encoding;
    }

    [NotNull]
    private Encoding Encoding { get; }

    [NotNull]
    private IEnumerable<byte> GetBytes([NotNull] string s)
    {
      return this.Encoding.GetBytes(s);
    }

    [NotNull]
    public IEnumerable<byte> GraphicDirectWrite([NotNull] Bitmap bitmap,
                                                int horizontalStart,
                                                int verticalStart)
    {
      var octetts = (int) Math.Ceiling(bitmap.Width / 8f);
      var translation = $"GW{horizontalStart},{verticalStart},{octetts},{bitmap.Height}";
      foreach (var @byte in this.GetBytes(translation))
      {
        yield return @byte;
      }
      foreach (var @byte in this.GetBytes(Environment.NewLine))
      {
        yield return @byte;
      }

      var rawBinaryData = this.GetRawBinaryData(bitmap,
                                                octetts);
      foreach (var @byte in rawBinaryData)
      {
        yield return @byte;
      }
    }

    [NotNull]
    public IEnumerable<byte> StoreGraphics([NotNull] Bitmap bitmap,
                                           [NotNull] string name)
    {
      var height = bitmap.Height;
      var octetts = (int) Math.Ceiling(bitmap.Width / 8f);
      var fileSize = height * octetts;
      var translation = $@"GM""{name}""{fileSize + 128}";
      foreach (var @byte in this.GetBytes(translation))
      {
        yield return @byte;
      }
      foreach (var @byte in this.GetBytes(Environment.NewLine))
      {
        yield return @byte;
      }

      // header start
      /* 00+00 */ yield return 0x0A; // PCX File
      /* 01+00 */ yield return 0x05; // Version 5
      /* 02+00 */ yield return 0x00; // no compression
      /* 03+00 */ yield return 0x01; // 1 bit per pixel
      /* 04+00 */ yield return 0x00; // xmin
      /* 05+00 */ yield return 0x00; // xmin
      /* 06+00 */ yield return 0x00; // ymin
      /* 07+00 */ yield return 0x00; // ymin
      /* 00+08 */ yield return (byte) (bitmap.Width - 1 & 0xFF); // xmax
      /* 01+08 */ yield return (byte) (bitmap.Width - 1 >> 8); // xmax
      /* 02+08 */ yield return (byte) (bitmap.Height - 1 & 0xFF); // ymax
      /* 03+08 */ yield return (byte) (bitmap.Height - 1 >> 8); // ymax
      /* 04+08 */ yield return (byte) bitmap.HorizontalResolution; // horizontal dpi
      /* 05+08 */ yield return 0x00; // horizontal dpi
      /* 06+08 */ yield return (byte) bitmap.VerticalResolution; // vertical dpi
      /* 07+08 */ yield return 0x00; // vertical dpi
      /* 00+16 - 07+56 */ // 48-byte EGA palette info
      for (var i = 15;
           i >= 0;
           i--)
      {
        yield return (byte) i;
        yield return (byte) i;
        yield return (byte) i;
      }
      /* 00+64 */ yield return 0x00; // Reserved byte, always 0x00
      /* 01+64 */ yield return 0x01; // 1 bit plane
      /* 02+64 */ yield return (byte) (octetts & 0xFF); // high nibble of bytes per line
      /* 03+64 */ yield return (byte) (octetts >> 8); // low nibble of bytes per line
      /* 04+64 */ yield return 0x01; // palette
      /* 05+64 */ yield return 0x00; // palette
      /* 06+64 */ yield return 0x00; // high nibble width
      /* 07+64 */ yield return 0x00; // low nibble width
      /* 00+72 */ yield return 0x00; // high nibble height
      /* 01+72 */ yield return 0x00; // low nibble height
      /* 02+72 - 07+120 */ // filler
      for (var i = 0;
           i < 54;
           i++)
      {
        yield return 0x00;
      }
      // header end

      var rawBinaryData = this.GetRawBinaryData(bitmap,
                                                octetts);
      foreach (var @byte in rawBinaryData)
      {
        yield return @byte;
      }
    }

    [NotNull]
    public IEnumerable<byte> GetRawBinaryData([NotNull] Bitmap bitmap,
                                              int octetts)
    {
      var height = bitmap.Height;
      var width = bitmap.Width;

      for (var y = 0;
           y < height;
           y++)
      {
        for (var octett = 0;
             octett < octetts;
             octett++)
        {
          var value = (int) byte.MaxValue;

          for (var i = 0;
               i < 8;
               i++)
          {
            var x = octett * 8 + i;
            var bitIndex = 7 - i;
            if (x < width)
            {
              var color = bitmap.GetPixel(x,
                                          y);
              if (color.A > 0x32
                  || color.R > 0x96 && color.G > 0x96 && color.B > 0x96)
              {
                value &= ~(1 << bitIndex);
              }
            }
          }

          yield return (byte) value;
        }
      }
    }

    [NotNull]
    public IEnumerable<byte> PrintGraphics(int horizontalStart,
                                           int verticalStart,
                                           [NotNull] string name)
    {
      var translation = $@"GG{horizontalStart},{verticalStart},""{name}""";
      var result = this.GetBytes(translation);

      return result;
    }

    [NotNull]
    public IEnumerable<byte> LineDrawBlack(int horizontalStart,
                                           int verticalStart,
                                           int horizontalLength,
                                           int verticalLength)
    {
      var translation = $"LO{horizontalStart},{verticalStart},{horizontalLength},{verticalLength}";
      var result = this.GetBytes(translation);

      return result;
    }

    [NotNull]
    public IEnumerable<byte> LineDrawWhite(int horizontalStart,
                                           int verticalStart,
                                           int horizontalLength,
                                           int verticalLength)
    {
      horizontalStart -= horizontalLength;

      var translation = $"LW{horizontalStart},{verticalStart},{horizontalLength},{verticalLength}";
      var result = this.GetBytes(translation);

      return result;
    }

    [NotNull]
    public IEnumerable<byte> LineDrawDiagonal(int horizontalStart,
                                              int verticalStart,
                                              int horizontalLength,
                                              int verticalLength,
                                              int verticalEnd)
    {
      var translation = $"LS{horizontalStart},{verticalStart},{horizontalLength},{verticalLength},{verticalEnd}";
      var result = this.GetBytes(translation);

      return result;
    }

    [NotNull]
    public IEnumerable<byte> DrawBox(int horizontalStart,
                                     int verticalStart,
                                     int lineThickness,
                                     int horizontalEnd,
                                     int verticalEnd)
    {
      var translation = $"X{horizontalStart},{verticalStart},{lineThickness},{horizontalEnd},{verticalEnd}";
      var result = this.GetBytes(translation);

      return result;
    }

    [NotNull]
    public IEnumerable<byte> AsciiText(int horizontalStart,
                                       int verticalStart,
                                       int rotation,
                                       [NotNull] string fontSelection,
                                       int horizontalMulitplier,
                                       int verticalMulitplier,
                                       [NotNull] string reverseImage,
                                       [NotNull] string text)
    {
      var translation = $@"A{horizontalStart},{verticalStart},{rotation},{fontSelection},{horizontalMulitplier},{verticalMulitplier},{reverseImage},""{text}""";
      var result = this.GetBytes(translation);

      return result;
    }
  }
}