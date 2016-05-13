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

    private Encoding Encoding { get; }

    private IEnumerable<byte> GetBytes(string s)
    {
      return this.Encoding.GetBytes(s);
    }

    public IEnumerable<byte> GraphicDirectWrite([NotNull] Bitmap bitmap,
                                                int horizontalStart,
                                                int verticalStart)
    {
      var width = bitmap.Width;
      var height = bitmap.Height;
      var octetts = (int) Math.Ceiling(width / 8f);
      var alignedWidth = octetts * 8;

      var translation = $"GW{horizontalStart},{verticalStart},{octetts},{height}";
      foreach (var @byte in this.GetBytes(translation))
      {
        yield return @byte;
      }
      foreach (var @byte in this.GetBytes(Environment.NewLine))
      {
        yield return @byte;
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
            octett = byte.MaxValue;
          }
        }
      }
    }
  }
}