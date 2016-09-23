using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using ImageMagick;
using JetBrains.Annotations;

// ReSharper disable UnusedParameter.Global
// ReSharper disable VirtualMemberNeverOverriden.Global

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class EplTransformer : GenericTransformer
  {
    public const int DefaultOutputWidth = 816;
    public const int DefaultOutputHeight = 1296;

    /// <exception cref="ArgumentNullException"><paramref name="svgUnitReader" /> is <see langword="null" />.</exception>
    public EplTransformer([NotNull] SvgUnitReader svgUnitReader)
      : base(svgUnitReader,
             EplTransformer.DefaultOutputWidth,
             EplTransformer.DefaultOutputHeight)
    {
      if (svgUnitReader == null)
      {
        throw new ArgumentNullException(nameof(svgUnitReader));
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgUnitReader" /> is <see langword="null" />.</exception>
    public EplTransformer([NotNull] SvgUnitReader svgUnitReader,
                          int outputWidth,
                          int outputHeight)
      : base(svgUnitReader,
             outputWidth,
             outputHeight)
    {
      if (svgUnitReader == null)
      {
        throw new ArgumentNullException(nameof(svgUnitReader));
      }
    }

    protected virtual int MaximumUpperFontSizeOverlap { get; } = 2;

    /// <exception cref="ArgumentNullException"><paramref name="svgTextBase"/> is <see langword="null" />.</exception>
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
        // this should never happen :beers:
        // but can happen, if tiny font is used - idgaf
        throw new NotImplementedException();
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

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix"/> is <see langword="null" />.</exception>
    [Pure]
    public override void Transform([NotNull] SvgRectangle svgRectangle,
                                   [NotNull] Matrix sourceMatrix,
                                   [NotNull] Matrix viewMatrix,
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

    /// <exception cref="ArgumentNullException"><paramref name="bitmap"/> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual byte[] ConvertToPcx([NotNull] Bitmap bitmap)
    {
      if (bitmap == null)
      {
        throw new ArgumentNullException(nameof(bitmap));
      }

      var width = bitmap.Width;
      var mod = width % 8;
      if (mod > 0)
      {
        width += 8 - mod;
      }
      var height = bitmap.Height;

      using (var magickImage = new MagickImage(bitmap))
      {
        if (mod > 0)
        {
          var magickGeometry = new MagickGeometry
                               {
                                 Width = width,
                                 Height = height,
                                 IgnoreAspectRatio = true
                               };
          magickImage.Resize(magickGeometry);
        }

        magickImage.ColorAlpha(MagickColors.White);

        var quantizeSettings = new QuantizeSettings
                               {
                                 Colors = 2,
                                 DitherMethod = DitherMethod.No
                               };
        magickImage.Quantize(quantizeSettings);
        magickImage.ColorSpace = ColorSpace.Gray;
        magickImage.ContrastStretch(new ImageMagick.Percentage(0));

        magickImage.Format = MagickFormat.Pcx;
        magickImage.ColorType = ColorType.Palette;
        magickImage.ColorSpace = ColorSpace.Gray;

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