using System.Drawing;
using System.Linq;

namespace System.Svg.Render.EPL
{
  public class SvgRectangleTranslator : SvgElementTranslator<SvgRectangle>
  {
    // TODO add documentation/quote: strokes are printed inside the rectangle (calculation stuff)

    /// <exception cref="ArgumentNullException"><paramref name="svgLineTranslator" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitCalculator" /> is <see langword="null" />.</exception>
    public SvgRectangleTranslator(SvgLineTranslator svgLineTranslator,
                                  SvgUnitCalculator svgUnitCalculator)
    {
      if (svgLineTranslator == null)
      {
        throw new ArgumentNullException(nameof(svgLineTranslator));
      }
      if (svgUnitCalculator == null)
      {
        throw new ArgumentNullException(nameof(svgUnitCalculator));
      }

      this.SvgLineTranslator = svgLineTranslator;
      this.SvgUnitCalculator = svgUnitCalculator;
    }

    private SvgLineTranslator SvgLineTranslator { get; }
    private SvgUnitCalculator SvgUnitCalculator { get; }

    public override object Translate(SvgRectangle instance,
                                     int targetDpi)
    {
      if (instance == null)
      {
        // TODO add logging
        return null;
      }

      // TODO allow diagnoal rectangle ...

      object translation;
      if (this.SvgUnitCalculator.IsValueZero(instance.Width)
          && this.SvgUnitCalculator.IsValueZero(instance.Height))
      {
        var svgLine = new SvgLine
                      {
                        StartX = instance.X,
                        StartY = instance.Y,
                        EndX = instance.X,
                        EndY = instance.Y,
                        StrokeWidth = instance.StrokeWidth
                      };

        translation = this.SvgLineTranslator.Translate(svgLine,
                                                       targetDpi);
      }
      else if ((instance.Color as SvgColourServer)?.Colour == Color.Black)
      {
        // TODO svgRectangle.StrokeLineJoin

        SvgUnit endX;
        try
        {
          endX = this.SvgUnitCalculator.Add(instance.X,
                                            instance.Width);
        }
        catch (ArgumentException argumentException)
        {
          // TODO add logging
          return null;
        }

        var svgLine = new SvgLine
                      {
                        StartX = instance.X,
                        StartY = instance.Y,
                        EndX = endX,
                        EndY = instance.Y,
                        StrokeWidth = instance.Height
                      };

        translation = this.SvgLineTranslator.Translate(svgLine,
                                                       targetDpi);
      }
      else
      {
        // TODO svgRectangle.StrokeLineJoin
        SvgLine upperLine;
        SvgLine rightLine;
        SvgLine lowerLine;
        SvgLine leftLine;
        try
        {
          upperLine = this.GetUpperLine(instance);
          rightLine = this.GetRightLine(instance);
          lowerLine = this.GetLowerLine(instance);
          leftLine = this.GetLeftLine(instance);
        }
        catch (ArgumentException argumentException)
        {
          // TODO add logging
          return null;
        }

        var translations = new[]
                           {
                             upperLine,
                             rightLine,
                             lowerLine,
                             leftLine
                           }.Select(arg => this.SvgLineTranslator.Translate(arg,
                                                                            targetDpi))
                            .Where(arg => arg != null);

        translation = string.Join(Environment.NewLine,
                                  translations);
      }
      return translation;
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">If <paramref name="svgRectangle" /> has mixed <see cref="SvgUnitType" /> for <see cref="SvgRectangle.X" />, <see cref="SvgRectangle.Y" />, <see cref="SvgRectangle.Height" /> or <see cref="SvgRectangle.Width" />.</exception>
    public SvgLine GetLeftLine(SvgRectangle svgRectangle)
    {
      if (svgRectangle == null)
      {
        throw new ArgumentNullException(nameof(svgRectangle));
      }

      var leftLine = new SvgLine
                     {
                       StartX = svgRectangle.X,
                       StartY = svgRectangle.Y,
                       EndX = svgRectangle.X,
                       EndY = this.SvgUnitCalculator.Add(svgRectangle.Y,
                                                         svgRectangle.Height),
                       StrokeWidth = svgRectangle.StrokeWidth
                     };

      return leftLine;
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">If <paramref name="svgRectangle" /> has mixed <see cref="SvgUnitType" /> for <see cref="SvgRectangle.X" />, <see cref="SvgRectangle.Y" />, <see cref="SvgRectangle.Height" /> or <see cref="SvgRectangle.Width" />.</exception>
    public SvgLine GetLowerLine(SvgRectangle svgRectangle)
    {
      if (svgRectangle == null)
      {
        throw new ArgumentNullException(nameof(svgRectangle));
      }

      var lowerLine = new SvgLine
                      {
                        StartX = svgRectangle.X,
                        StartY = this.SvgUnitCalculator.Add(svgRectangle.Y,
                                                            svgRectangle.Height),
                        EndX = this.SvgUnitCalculator.Add(svgRectangle.X,
                                                          svgRectangle.Width),
                        EndY = this.SvgUnitCalculator.Add(svgRectangle.Y,
                                                          svgRectangle.Height),
                        StrokeWidth = svgRectangle.StrokeWidth
                      };

      return lowerLine;
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">If <paramref name="svgRectangle" /> has mixed <see cref="SvgUnitType" /> for <see cref="SvgRectangle.X" />, <see cref="SvgRectangle.Y" />, <see cref="SvgRectangle.Height" /> or <see cref="SvgRectangle.Width" />.</exception>
    public SvgLine GetRightLine(SvgRectangle svgRectangle)
    {
      if (svgRectangle == null)
      {
        throw new ArgumentNullException(nameof(svgRectangle));
      }

      var rightLine = new SvgLine
                      {
                        StartX = this.SvgUnitCalculator.Add(svgRectangle.X,
                                                            svgRectangle.Width),
                        StartY = svgRectangle.Y,
                        EndX = this.SvgUnitCalculator.Add(svgRectangle.X,
                                                          svgRectangle.Width),
                        EndY = this.SvgUnitCalculator.Add(svgRectangle.Y,
                                                          svgRectangle.Height),
                        StrokeWidth = svgRectangle.StrokeWidth
                      };

      return rightLine;
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgRectangle" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentException">If <paramref name="svgRectangle" /> has mixed <see cref="SvgUnitType" /> for <see cref="SvgRectangle.X" />, <see cref="SvgRectangle.Y" />, <see cref="SvgRectangle.Height" /> or <see cref="SvgRectangle.Width" />.</exception>
    public SvgLine GetUpperLine(SvgRectangle svgRectangle)
    {
      if (svgRectangle == null)
      {
        throw new ArgumentNullException(nameof(svgRectangle));
      }

      var upperLine = new SvgLine
                      {
                        StartX = svgRectangle.X,
                        StartY = svgRectangle.Y,
                        EndX = this.SvgUnitCalculator.Add(svgRectangle.X,
                                                          svgRectangle.Width),
                        EndY = svgRectangle.Y,
                        StrokeWidth = svgRectangle.StrokeWidth
                      };

      return upperLine;
    }
  }
}