using System.Collections.Generic;
using System.Drawing;
using System.Text;
using ImageMagick;
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
      MagickImage magickImage;

      var mod = bitmap.Width % 8;
      if (mod > 0)
      {
        var newWidth = bitmap.Width + 8 - mod;
        using (var resizedBitmap = new Bitmap(newWidth,
                                              bitmap.Height))
        {
          using (var graphics = Graphics.FromImage(resizedBitmap))
          {
            graphics.DrawImageUnscaled(bitmap,
                                       0,
                                       0);
            graphics.Save();
          }

          magickImage = new MagickImage(resizedBitmap);
        }
      }
      else
      {
        magickImage = new MagickImage(bitmap);
      }

      byte[] buffer;
      using (magickImage)
      {
        magickImage.ColorType = ColorType.Bilevel;
        magickImage.Negate();
        magickImage.Threshold(new Percentage(50));
        buffer = magickImage.ToByteArray(MagickFormat.Pcx);
      }

      foreach (var @byte in this.GetBytes($@"GK""{name}"""))
      {
        yield return @byte;
      }
      foreach (var @byte in this.GetBytes(Environment.NewLine))
      {
        yield return @byte;
      }
      foreach (var @byte in this.GetBytes($@"GM""{name}""{buffer.Length}"))
      {
        yield return @byte;
      }
      foreach (var @byte in this.GetBytes(Environment.NewLine))
      {
        yield return @byte;
      }
      foreach (var @byte in buffer)
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