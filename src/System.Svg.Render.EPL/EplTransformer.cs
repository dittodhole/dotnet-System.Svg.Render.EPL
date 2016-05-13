using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class EplTransformer : Transformer
  {
    public EplTransformer([NotNull] SvgUnitReader svgUnitReader,
                          PrintDirection printDirection = PrintDirection.TopOrBottom)
      : base(svgUnitReader)
    {
      this.PrintDirection = printDirection;
    }

    public EplTransformer([NotNull] SvgUnitReader svgUnitReader,
                          PrintDirection printDirection,
                          int labelWithInDevicePoints,
                          int labelHeightInDevicePoints)
      : this(svgUnitReader,
             printDirection)
    {
      this.LabelWidthInDevicePoints = labelWithInDevicePoints;
      this.LabelHeightInDevicePoints = labelHeightInDevicePoints;
    }

    protected int MaximumUpperFontSizeOverlap { get; } = 2;
    public int LabelWidthInDevicePoints { get; set; } = 1296;
    public int LabelHeightInDevicePoints { get; set; } = 816;
    private PrintDirection PrintDirection { get; }

    [NotNull]
    public Matrix CreateViewMatrix()
    {
      Matrix matrix;
      if (this.PrintDirection == PrintDirection.None)
      {
        matrix = new Matrix();
      }
      else
      {
        matrix = this.CreateViewMatrix(90f,
                                       203f);
      }

      return matrix;
    }

    [NotNull]
    public Matrix CreateViewMatrix(float sourceDpi,
                                   float targetDpi)
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

    public int GetRotation([NotNull] Matrix matrix)
    {
      float fontSize;
      var rotationTranslation = this.GetRotation(matrix,
                                                 out fontSize);

      return rotationTranslation;
    }

    public int GetRotation([NotNull] Matrix matrix,
                           out float linearScalingFactor)
    {
      var vector = new PointF(10 * -1f,
                              0f);

      this.ApplyMatrix(vector,
                       matrix,
                       out vector);

      linearScalingFactor = Math.Abs(10 / this.GetLengthOfVector(vector));

      var rotation = Math.Atan2(vector.Y,
                                vector.X) / (2 * Math.PI) * 4;

      var rotationTranslation = (int) Math.Abs(rotation) % 4;

      return rotationTranslation;
    }

    public virtual void GetFontSelection(float fontSize,
                                         [NotNull] out string fontSelection,
                                         out int multiplier)
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

      var fontDefinitions = new SortedList<int, string>
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
            multiplier = factor;
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
        var differenceLower = fontSize - lowerFontDefinitionCandidate.ActualHeight;
        var differenceUpper = upperFontDefinitionCandidate.ActualHeight - fontSize;
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
    }

    protected override PointF AdaptPoint(PointF point)
    {
      // TODO clarify: can this be done w/ matrix?

      point = base.AdaptPoint(point);

      if (this.PrintDirection == PrintDirection.TopOrBottom)
      {
        point.X = this.LabelHeightInDevicePoints - point.X;
      }

      return point;
    }

    public void Transform([NotNull] SvgTextBase svgTextBase,
                          [NotNull] Matrix matrix,
                          out float x,
                          out float y,
                          out float fontSize,
                          out int rotation)
    {
      base.Transform(svgTextBase,
                     matrix,
                     out x,
                     out y,
                     out fontSize);

      float linearScalingFactor;
      rotation = this.GetRotation(matrix,
                                  out linearScalingFactor);

      fontSize = fontSize * linearScalingFactor;
    }

    private class FontDefinitionCandidate
    {
      public string FontSelection { get; set; }
      public int ActualHeight { get; set; }
      public int Multiplier { get; set; }
    }
  }
}