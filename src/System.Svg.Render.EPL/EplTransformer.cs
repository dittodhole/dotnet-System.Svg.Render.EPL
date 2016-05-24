using System.Collections.Generic;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable UnusedParameter.Global

namespace System.Svg.Render.EPL
{
  [PublicAPI]
  public class EplTransformer : GenericTransformer
  {
    public const int DefaultOutputWidth = 816;
    public const int DefaultOutputHeight = 1296;

    public EplTransformer([NotNull] SvgUnitReader svgUnitReader)
      : base(svgUnitReader,
             EplTransformer.DefaultOutputWidth,
             EplTransformer.DefaultOutputHeight) {}

    public EplTransformer([NotNull] SvgUnitReader svgUnitReader,
                          int outputWidth,
                          int outputHeight)
      : base(svgUnitReader,
             outputWidth,
             outputHeight) {}

    protected virtual int MaximumUpperFontSizeOverlap { get; } = 2;

    public virtual void GetFontSelection([NotNull] SvgTextBase svgTextBase,
                                         float fontSize,
                                         out int fontSelection,
                                         out int horizontalMultiplier,
                                         out int verticalMultiplier)
    {
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

    private class FontDefinitionCandidate
    {
      public int FontSelection { get; set; }
      public int ActualHeight { get; set; }
      public int Multiplier { get; set; }
    }
  }
}