using System.Collections.Generic;
using System.Drawing;
using ImageMagick;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class EplCommands
  {
    [NotNull]
    public virtual EplStream GraphicDirectWrite([NotNull] Bitmap bitmap,
                                                int horizontalStart,
                                                int verticalStart)
    {
      var eplStream = new EplStream();
      var octetts = (int) Math.Ceiling(bitmap.Width / 8f);
      eplStream.Add($"GW{horizontalStart},{verticalStart},{octetts},{bitmap.Height}");
      eplStream.Add(this.GetRawBinaryData(bitmap,
                                          octetts));

      return eplStream;
    }

    [NotNull]
    public virtual IEnumerable<byte> GetRawBinaryData([NotNull] Bitmap bitmap,
                                                      int octetts)
    {
      // TODO merge with MagickImage, as we are having different thresholds here

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
    public virtual EplStream DeleteGraphics([NotNull] string name)
    {
      var eplStream = new EplStream();
      eplStream.Add($@"GK""{name}""");

      return eplStream;
    }

    [NotNull]
    public virtual EplStream StoreGraphics([NotNull] Bitmap bitmap,
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

      byte[] array;
      using (magickImage)
      {
        // TODO threshold
        magickImage.ColorType = ColorType.Bilevel;
        magickImage.Negate();
        array = magickImage.ToByteArray(MagickFormat.Pcx);
      }

      var eplStream = new EplStream();
      eplStream.Add($@"GM""{name}""{array.Length}");
      eplStream.Add(array);

      return eplStream;
    }

    [NotNull]
    public virtual EplStream PrintGraphics(int horizontalStart,
                                           int verticalStart,
                                           [NotNull] string name)
    {
      var eplStream = new EplStream();
      eplStream.Add($@"GG{horizontalStart},{verticalStart},""{name}""");

      return eplStream;
    }

    [NotNull]
    public virtual EplStream LineDrawBlack(int horizontalStart,
                                           int verticalStart,
                                           int horizontalLength,
                                           int verticalLength)
    {
      var eplStream = new EplStream();
      eplStream.Add($"LO{horizontalStart},{verticalStart},{horizontalLength},{verticalLength}");

      return eplStream;
    }

    [NotNull]
    public virtual EplStream LineDrawWhite(int horizontalStart,
                                           int verticalStart,
                                           int horizontalLength,
                                           int verticalLength)
    {
      var eplStream = new EplStream();
      eplStream.Add($"LW{horizontalStart},{verticalStart},{horizontalLength},{verticalLength}");

      return eplStream;
    }

    [NotNull]
    public virtual EplStream LineDrawDiagonal(int horizontalStart,
                                              int verticalStart,
                                              int horizontalLength,
                                              int verticalLength,
                                              int verticalEnd)
    {
      var eplStream = new EplStream();
      eplStream.Add($"LS{horizontalStart},{verticalStart},{horizontalLength},{verticalLength},{verticalEnd}");

      return eplStream;
    }

    [NotNull]
    public virtual EplStream DrawBox(int horizontalStart,
                                     int verticalStart,
                                     int lineThickness,
                                     int horizontalEnd,
                                     int verticalEnd)
    {
      var eplStream = new EplStream();
      eplStream.Add($"X{horizontalStart},{verticalStart},{lineThickness},{horizontalEnd},{verticalEnd}");

      return eplStream;
    }

    [NotNull]
    public virtual EplStream AsciiText(int horizontalStart,
                                       int verticalStart,
                                       int rotation,
                                       [NotNull] string fontSelection,
                                       int horizontalMulitplier,
                                       int verticalMulitplier,
                                       [NotNull] string reverseImage,
                                       [NotNull] string text)
    {
      var eplStream = new EplStream();
      eplStream.Add($@"A{horizontalStart},{verticalStart},{rotation},{fontSelection},{horizontalMulitplier},{verticalMulitplier},{reverseImage},""{text}""");

      return eplStream;
    }
  }
}