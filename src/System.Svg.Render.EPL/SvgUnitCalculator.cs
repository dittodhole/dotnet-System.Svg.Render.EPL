using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgUnitCalculator : SvgUnitCalculatorBase
  {
    public static Matrix CreateViewMatrix(int sourceDpi,
                                          int targetDpi)
    {
      var magnificationFactor = targetDpi / sourceDpi;

      // we use no identity matrix here, as we need to
      // rotate and flip the coordinates from svg to
      // epl
      // svg matrix:
      // +- x+
      // |y+
      // epl matrix:
      // +- y+
      // | x +
      var matrix = new Matrix(0f,
                              magnificationFactor,
                              magnificationFactor,
                              0f,
                              0f,
                              0f);

      return matrix;
    }
    public SvgUnitCalculator(PrintDirection printDirection = PrintDirection.Top)
    {
      this.PrintDirection = printDirection;
    }

    public SvgUnitCalculator(PrintDirection printDirection,
                             int labelWithInDevicePoints,
                             int labelHeightInDevicePoints)
      : this(printDirection)
    {
      this.LabelWidthInDevicePoints = labelWithInDevicePoints;
      this.LabelHeightInDevicePoints = labelHeightInDevicePoints;
    }

    protected int MaximumUpperFontSizeOverlap { get; } = 2;
    public int LabelWidthInDevicePoints { get; set; } = 1296;
    public int LabelHeightInDevicePoints { get; set; } = 816;
    private PrintDirection PrintDirection { get; }

    private PointF GetFontHeightVector()
    {
      PointF fontHeightVector;
      if (this.PrintDirection == PrintDirection.Top)
      {
        fontHeightVector = new PointF(-10f,
                                     0f);
      }
      else if (this.PrintDirection == PrintDirection.None)
      {
        fontHeightVector = new PointF(0f,
                                      10f);
      }
      else
      {
        // TODO
        throw new NotImplementedException();
      }

      return fontHeightVector;
    }

    public object GetRotationTranslation([NotNull] Matrix matrix)
    {
      var vector = this.GetFontHeightVector();
      var vectors = new[]
                    {
                      vector
                    };
      matrix.TransformVectors(vectors);
      vector = vectors.First();

      var rotation = Math.Atan2(vector.Y,
                                vector.X) / (2 * Math.PI);
      var rotationTranslation = Math.Floor(rotation * 4);
      rotationTranslation = Math.Abs(rotationTranslation);

      return rotationTranslation;
    }

    public bool TryGetFontTranslation(int fontSize,
                                      [NotNull] Matrix matrix,
                                      int targetDpi,
                                      out object translation)
    {
      var height = this.GetHeightOfFontSize(fontSize,
                                            matrix);

      object fontSelection;
      object multiplier;
      if (!this.TryGetFontSelection(height,
                                    targetDpi,
                                    out fontSelection,
                                    out multiplier))
      {
#if DEBUG
        translation = $"; could not get font selection from {nameof(fontSize)} ({fontSize}) in {nameof(targetDpi)} of {targetDpi}";
#else
        translation = null;
#endif
        return false;
      }

      translation = $"{fontSelection},{multiplier},{multiplier}";

      return true;
    }

    private bool TryGetFontSelection(int height,
                                     int targetDpi,
                                     out object fontSelection,
                                     out object multiplier)
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

      SortedList<int, string> fontDefinitions;
      if (targetDpi == 203)
      {
        fontDefinitions = new SortedList<int, string>
                          {
                            {
                              12, "1"
                            },
                            {
                              16, "2"
                            },
                            {
                              20, "3"
                            },
                            {
                              24, "4"
                            }
                          };
      }
      else if (targetDpi == 300)
      {
        fontDefinitions = new SortedList<int, string>
                          {
                            {
                              20, "1"
                            },
                            {
                              28, "2"
                            },
                            {
                              36, "3"
                            },
                            {
                              44, "4"
                            }
                          };
      }
      else
      {
        fontSelection = null;
        multiplier = null;
        return false;
      }

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
          var actualHeight = fontDefinition.Key * factor;
          if (actualHeight == height)
          {
            fontSelection = fontDefinition.Value;
            multiplier = factor;
            return true;
          }
          if (actualHeight < height)
          {
            if (lowerFontDefinitionCandidate == null
                || actualHeight > lowerFontDefinitionCandidate.ActualHeight)
            {
              lowerFontDefinitionCandidate = new FontDefinitionCandidate
                                             {
                                               FontSelection = fontDefinition.Value,
                                               ActualHeight = actualHeight,
                                               Multiplier = factor
                                             };
            }
          }
          else if (actualHeight <= height + this.MaximumUpperFontSizeOverlap)
          {
            if (upperFontDefinitionCandidate == null
                || actualHeight < upperFontDefinitionCandidate.ActualHeight)
            {
              upperFontDefinitionCandidate = new FontDefinitionCandidate
                                             {
                                               FontSelection = fontDefinition.Value,
                                               ActualHeight = actualHeight,
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
        fontSelection = null;
        multiplier = null;
        return false;
      }

      if (lowerFontDefinitionCandidate == null)
      {
        fontSelection = upperFontDefinitionCandidate.FontSelection;
        multiplier = upperFontDefinitionCandidate.Multiplier;
      }
      else if (upperFontDefinitionCandidate == null)
      {
        fontSelection = lowerFontDefinitionCandidate.FontSelection;
        multiplier = lowerFontDefinitionCandidate.Multiplier;
      }
      else
      {
        // :question: why dafuq are you doing it like this, and using no comparisons in the if-clause :question:
        // reason: idk if lower or upper is better, so I am leveling the playing field here
        // if I would add this to the if-clauses, the arithmetic behind it would be done
        // twice for the worst case. with this solution, the cost is stable for all scenarios
        var differenceLower = height - lowerFontDefinitionCandidate.ActualHeight;
        var differenceUpper = upperFontDefinitionCandidate.ActualHeight - height;
        if (differenceLower <= differenceUpper)
        {
          fontSelection = lowerFontDefinitionCandidate.FontSelection;
          multiplier = lowerFontDefinitionCandidate.Multiplier;
        }
        else
        {
          fontSelection = upperFontDefinitionCandidate.FontSelection;
          multiplier = upperFontDefinitionCandidate.Multiplier;
        }
      }

      return true;
    }

    private int GetHeightOfFontSize(int fontSize,
                                    [NotNull] Matrix matrix)
    {
      var vector = new PointF(0f,
                              fontSize);
      var vectors = new[]
                    {
                      vector
                    };
      matrix.TransformVectors(vectors);
      vector = vectors.First();

      var result = Math.Sqrt(Math.Pow(vector.X,
                                      2) + Math.Pow(vector.Y,
                                                    2));

      return (int) result;
    }

    protected override Point AdaptPoint(Point point)
    {
      // TODO clarify: can this be done w/ matrix?

      point = base.AdaptPoint(point);

      if (this.PrintDirection == PrintDirection.Top)
      {
        point.X = this.LabelHeightInDevicePoints - point.X;
      }

      return point;
    }

    private class FontDefinitionCandidate
    {
      public object FontSelection { get; set; }
      public int ActualHeight { get; set; }
      public object Multiplier { get; set; }
    }
  }
}