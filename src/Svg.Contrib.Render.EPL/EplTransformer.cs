using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using ImageMagick;
using JetBrains.Annotations;

#if NETSTANDARD2_0
using System.IO;
#endif

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class EplTransformer : GenericTransformer
  {
    public const int DefaultOutputWidth = 816;
    public const int DefaultOutputHeight = 1296;
    public const int DefaultMaximumUpperFontSizeOverlap = 2;

    /// <inheritdoc />
    public EplTransformer([NotNull] SvgUnitReader svgUnitReader,
                          int outputWidth = EplTransformer.DefaultOutputWidth,
                          int outputHeight = EplTransformer.DefaultOutputHeight,
                          int maximumUpperFontSizeOverlap = EplTransformer.DefaultMaximumUpperFontSizeOverlap)
      : base(svgUnitReader,
             outputWidth,
             outputHeight)
    {
      this.MaximumUpperFontSizeOverlap = maximumUpperFontSizeOverlap;
    }

    private int MaximumUpperFontSizeOverlap { get; }

    /// <exception cref="ArgumentNullException"><paramref name="svgTextBase" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentOutOfRangeException"><paramref name="fontSize" /> is out of range.</exception>
    [Pure]
    public virtual void GetFontSelection([NotNull] SvgTextBase svgTextBase,
                                         float fontSize,
                                         out int fontSelection,
                                         out int horizontalMultiplier,
                                         out int verticalMultiplier)
    {
      if (svgTextBase == null)
      {
        throw new ArgumentNullException(nameof(svgTextBase));
      }

      // VALUE    203dpi        300dpi
      // ==================================
      //  1       20.3cpi       25cpi
      //          6pts          4pts
      //          8x12 dots     12x20 dots
      //          1:1.5         1:1.66
      // ==================================
      //  2       16.9cpi       18.75cpi
      //          7pts          6pts
      //          10x16 dots    16x28 dots
      //          1:1.6         1:1.75
      // ==================================
      //  3       14.5cpi       15cpi
      //          10pts         8pts
      //          12x20 dots    20x36 dots
      //          1:1.66        1:1.8
      // ==================================
      //  4       12.7cpi       12.5cpi
      //          12pts         10pts
      //          14x24 dots    24x44 dots
      //          1:1.71        1:1.83
      // ==================================
      //  5       5.6cpi        6.25cpi
      //          24pts         21pts
      //          32x48 dots    48x80 dots
      //          1:1.5         1:1.6
      // ==================================
      // horizontal multiplier: Accepted Values: 1–6, 8
      // vertical multiplier: Accepted Values: 1–9

      var fontDefinitions = new SortedList<int, int>
                            {
                              {
                                12, 1
                              },
                              {
                                16, 2
                              },
                              {
                                20, 3
                              },
                              {
                                24, 4
                              }
                            };

      var lowerFontDefinitionCandidate = default(FontDefinitionCandidate);
      var upperFontDefinitionCandidate = default(FontDefinitionCandidate);
      foreach (var factor in new[]
                             {
                               1,
                               2,
                               3,
                               4,
                               5,
                               6,
                               8
                             })
      {
        foreach (var fontDefinition in fontDefinitions)
        {
          var actualFontSize = fontDefinition.Key * factor;

          // TODO find a good TOLERANCE
          if (Math.Abs(actualFontSize - fontSize) < 0.5f)
          {
            fontSelection = fontDefinition.Value;
            horizontalMultiplier = factor;
            verticalMultiplier = factor;
            return;
          }

          if (actualFontSize < fontSize)
          {
            if (lowerFontDefinitionCandidate == null
                || actualFontSize > lowerFontDefinitionCandidate.ActualHeight)
            {
              lowerFontDefinitionCandidate = new FontDefinitionCandidate
                                             {
                                               FontSelection = fontDefinition.Value,
                                               ActualHeight = actualFontSize,
                                               Multiplier = factor
                                             };
            }
          }
          else if (actualFontSize <= fontSize + this.MaximumUpperFontSizeOverlap)
          {
            if (upperFontDefinitionCandidate == null
                || actualFontSize < upperFontDefinitionCandidate.ActualHeight)
            {
              upperFontDefinitionCandidate = new FontDefinitionCandidate
                                             {
                                               FontSelection = fontDefinition.Value,
                                               ActualHeight = actualFontSize,
                                               Multiplier = factor
                                             };
            }
            break;
          }
        }
      }

      if (lowerFontDefinitionCandidate == null
          && upperFontDefinitionCandidate == null)
      {
        throw new ArgumentOutOfRangeException(nameof(fontSize),
                                              fontSize,
                                              $"Parameter {nameof(fontSize)} must be greater than {fontDefinitions.Keys.Min()}.");
      }

      if (lowerFontDefinitionCandidate == null)
      {
        fontSelection = upperFontDefinitionCandidate.FontSelection;
        horizontalMultiplier = upperFontDefinitionCandidate.Multiplier;
        verticalMultiplier = upperFontDefinitionCandidate.Multiplier;
      }
      else if (upperFontDefinitionCandidate == null)
      {
        fontSelection = lowerFontDefinitionCandidate.FontSelection;
        horizontalMultiplier = lowerFontDefinitionCandidate.Multiplier;
        verticalMultiplier = lowerFontDefinitionCandidate.Multiplier;
      }
      else
      {
        // :question: why dafuq are you doing it like this, and using no comparisons in the if-clause :question:
        // reason: idk if lower or upper is better, so I am leveling the playing field here
        // if I would add this to the if-clauses, the arithmetic behind it would be done
        // twice for the worst case. with this solution, the cost is stable for all scenarios
        var differenceLower = fontSize - lowerFontDefinitionCandidate.ActualHeight;
        var differenceUpper = upperFontDefinitionCandidate.ActualHeight - fontSize;
        if (differenceLower <= differenceUpper)
        {
          fontSelection = lowerFontDefinitionCandidate.FontSelection;
          horizontalMultiplier = lowerFontDefinitionCandidate.Multiplier;
          verticalMultiplier = lowerFontDefinitionCandidate.Multiplier;
        }
        else
        {
          fontSelection = upperFontDefinitionCandidate.FontSelection;
          horizontalMultiplier = upperFontDefinitionCandidate.Multiplier;
          verticalMultiplier = upperFontDefinitionCandidate.Multiplier;
        }
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    [Pure]
    public override void Transform(SvgRectangle svgRectangle,
                                   Matrix sourceMatrix,
                                   Matrix viewMatrix,
                                   out float startX,
                                   out float startY,
                                   out float endX,
                                   out float endY,
                                   out float strokeWidth)
    {
      if (svgRectangle == null)
      {
        throw new ArgumentNullException(nameof(svgRectangle));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }

      base.Transform(svgRectangle,
                     sourceMatrix,
                     viewMatrix,
                     out startX,
                     out startY,
                     out endX,
                     out endY,
                     out strokeWidth);

      startX -= strokeWidth / 4f;
      endX += strokeWidth / 4f;
      startY -= strokeWidth / 4f;
      endY += strokeWidth / 4f;
    }

    /// <exception cref="ArgumentNullException"><paramref name="bitmap" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual byte[] ConvertToPcx([NotNull] Bitmap bitmap)
    {
      if (bitmap == null)
      {
        throw new ArgumentNullException(nameof(bitmap));
      }

      // TODO merge with Svg.Contrib.Render.FingerPrint.FingerPrintTransformer.ConvertToPcx, Svg.Contrib.Render.FingerPrint

      var width = bitmap.Width;
      var mod = width % 8;
      if (mod > 0)
      {
        width += 8 - mod;
      }
      var height = bitmap.Height;

      MagickImage magickImage;
#if NETSTANDARD2_0
      using (var memoryStream = new MemoryStream())
      {
        bitmap.Save(memoryStream,
                    bitmap.RawFormat);

        magickImage = new MagickImage(memoryStream);
      }
#else
      magickImage = new MagickImage(bitmap);
#endif

      using (magickImage)
      {
        if (mod > 0)
        {
          var magickGeometry = new MagickGeometry(width,
                                                  height)
                               {
                                 IgnoreAspectRatio = true
                               };
          magickImage.Resize(magickGeometry);
        }

        if (magickImage.HasAlpha)
        {
          magickImage.ColorAlpha(MagickColors.White);
        }

        var quantizeSettings = new QuantizeSettings
                               {
                                 Colors = 2,
                                 DitherMethod = DitherMethod.No
                               };
        magickImage.Quantize(quantizeSettings);

        magickImage.ColorType = ColorType.Bilevel;
        magickImage.Depth = 1;
        magickImage.Format = MagickFormat.Pcx;

        magickImage.Density = new Density(bitmap.HorizontalResolution,
                                          bitmap.VerticalResolution);

        magickImage.Negate(); // TODO see https://github.com/dlemstra/Magick.NET/issues/569

        var array = magickImage.ToByteArray();

        return array;
      }
    }

    private class FontDefinitionCandidate
    {
      public int FontSelection { get; set; }
      public int ActualHeight { get; set; }
      public int Multiplier { get; set; }
    }
  }
}
