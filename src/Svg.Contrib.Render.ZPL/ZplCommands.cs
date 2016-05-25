using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using JetBrains.Annotations;

// ReSharper disable NonLocalizedString

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public class ZplCommands
  {
    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual ZplStream CreateZplStream() => new ZplStream();

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
    public virtual string FieldTypeset(int horizontalStart,
                                       int verticalStart)
    {
      return $"^FT{horizontalStart},{verticalStart}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string GraphicBox(int width,
                                     int height,
                                     int thickness,
                                     LineColor lineColor)
    {
      return $"^GB{width},{height},{thickness},{(char) lineColor}^FS";
    }

    //[NotNull]
    //[Pure]
    //[MustUseReturnValue]
    //public virtual string GraphicDiagonalLine(int width,
    //                                          int height,
    //                                          int thickness,
    //                                          LineColor lineColor,
    //                                          Orientation orientation)
    //{
    //  return $"^GD{width},{height},{thickness},{(char) lineColor},{(char) orientation}^FS";
    //}

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string Font([NotNull] string fontName,
                               FieldOrientation fieldOrientation,
                               int characterHeight,
                               int width,
                               string text)
    {
      return $"^A{fontName}{(char) fieldOrientation},{characterHeight},{width}^FD{text}^FS";
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

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string DownloadGraphics([NotNull] Bitmap bitmap,
                                              [NotNull] string name)
    {
      var numberOfBytesPerRow = (int) Math.Ceiling(bitmap.Width / 8f);
      var totalNumberOfBytes = numberOfBytesPerRow * bitmap.Height;
      var binaryData = this.GetRawBinaryData(bitmap,
                                             numberOfBytesPerRow);
      var data = BitConverter.ToString(binaryData.ToArray())
                             .Replace("-",
                                      string.Empty);

      return $@"~DGR:{name},{totalNumberOfBytes},{numberOfBytesPerRow},{data}";
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual IEnumerable<byte> GetRawBinaryData([NotNull] Bitmap bitmap,
                                                      int numberOfBytesPerRow)
    {
      // TODO merge with MagickImage, as we are having different thresholds here

      var height = bitmap.Height;
      var width = bitmap.Width;

      for (var y = 0;
           y < height;
           y++)
      {
        for (var octett = 0;
             octett < numberOfBytesPerRow;
             octett++)
        {
          var value = (int) byte.MinValue;

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
                value |= (1 << bitIndex);
              }
            }
          }

          yield return (byte) value;
        }
      }
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public virtual string RecallGraphic([NotNull] string name)
    {
      return $"^XGR:{name},1,1^FS";
    }
  }
}