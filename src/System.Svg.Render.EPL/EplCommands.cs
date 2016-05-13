﻿using System.Collections.Generic;
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
    private IEnumerable<byte> GetBytes(string s)
    {
      return this.Encoding.GetBytes(s);
    }

    [NotNull]
    public IEnumerable<byte> GraphicDirectWrite([NotNull] Bitmap bitmap,
                                                int horizontalStart,
                                                int verticalStart)
    {
      var translation = $"GW{horizontalStart},{verticalStart},{Math.Ceiling(bitmap.Width / 8f)},{bitmap.Height}";
      foreach (var @byte in this.GetBytes(translation))
      {
        yield return @byte;
      }
      foreach (var @byte in this.GetBytes(Environment.NewLine))
      {
        yield return @byte;
      }

      var rawBinaryData = this.GetRawBinaryData(bitmap);
      foreach (var @byte in rawBinaryData)
      {
        yield return @byte;
      }
    }

    public IEnumerable<byte> GetRawBinaryData(Bitmap bitmap)
    {
      var height = bitmap.Height;
      var width = bitmap.Width;
      var alignedWidth = width;

      var mod = width % 8;
      if (mod >= 0)
      {
        alignedWidth += (8 - mod);
      }

      for (var y = 0;
           y < height;
           y++)
      {
        var octett = (1 << 8) - 1;
        for (var x = 0;
             x < alignedWidth;
             x++)
        {
          var bitIndex = 7 - x % 8;
          if (x < width)
          {
            var color = bitmap.GetPixel(x,
                                        y);
            if (color.A > 0x32
                || color.R > 0x96 && color.G > 0x96 && color.B > 0x96)
            {
              octett &= ~(1 << bitIndex);
            }
          }

          if (bitIndex == 0)
          {
            yield return (byte) octett;
            octett = (1 << 8) - 1;
          }
        }
      }
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
                                       string fontSelection,
                                       int horizontalMulitplier,
                                       int verticalMulitplier,
                                       string reverseImage,
                                       string text)
    {
      var translation = $@"A{horizontalStart},{verticalStart},{rotation},{fontSelection},{horizontalMulitplier},{verticalMulitplier},{reverseImage},""{text}""";
      var result = this.GetBytes(translation);

      return result;
    }
  }
}