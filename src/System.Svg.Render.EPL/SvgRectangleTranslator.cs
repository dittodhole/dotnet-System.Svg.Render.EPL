using System.Drawing;
using System.Linq;

namespace System.Svg.Render.EPL
{
  public class SvgRectangleTranslator : SvgElementTranslator<SvgRectangle>
  {
    // TODO add documentation/quote: strokes are printed inside the rectangle (calculation stuff)

    public SvgRectangleTranslator(SvgLineTranslator svgLineTranslator,
                                  SvgUnitCalculator svgUnitCalculator)
    {
      if (svgLineTranslator == null)
      {
        // TODO add documentation
        throw new ArgumentNullException(nameof(svgLineTranslator));
      }
      if (svgUnitCalculator == null)
      {
        // TODO add documentation
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
        var svgLine = new SvgLine
                      {
                        StartX = instance.X,
                        StartY = instance.Y,
                        EndX = this.SvgUnitCalculator.Add(instance.X,
                                                          instance.Width),
                        EndY = instance.Y,
                        StrokeWidth = instance.Height
                      };

        translation = this.SvgLineTranslator.Translate(svgLine,
                                                       targetDpi);
      }
      else
      {
        var upperLine = this.GetUpperLine(instance);
        var rightLine = this.GetRightLine(instance);
        var lowerLine = this.GetLowerLine(instance);
        var leftLine = this.GetLeftLine(instance);
        var translations = new[]
                           {
                             upperLine,
                             rightLine,
                             lowerLine,
                             leftLine
                           }.Select(instance1 => this.SvgLineTranslator.Translate(instance1,
                                                                                  targetDpi))
                            .Where(arg => arg != null);

        translation = string.Join(Environment.NewLine,
                                  translations);
      }
      return translation;
    }

    public SvgLine GetLeftLine(SvgRectangle instance)
    {
      var leftLine = new SvgLine
                     {
                       StartX = instance.X,
                       StartY = instance.Y,
                       EndX = instance.X,
                       EndY = this.SvgUnitCalculator.Add(instance.Y,
                                                         instance.Height),
                       StrokeWidth = instance.StrokeWidth
                     };

      return leftLine;
    }

    public SvgLine GetLowerLine(SvgRectangle instance)
    {
      var lowerLine = new SvgLine
                      {
                        StartX = instance.X,
                        StartY = this.SvgUnitCalculator.Add(instance.Y,
                                                            instance.Height),
                        EndX = this.SvgUnitCalculator.Add(instance.X,
                                                          instance.Width),
                        EndY = this.SvgUnitCalculator.Add(instance.Y,
                                                          instance.Height),
                        StrokeWidth = instance.StrokeWidth
                      };

      return lowerLine;
    }

    public SvgLine GetRightLine(SvgRectangle instance)
    {
      var rightLine = new SvgLine
                      {
                        StartX = this.SvgUnitCalculator.Add(instance.X,
                                                            instance.Width),
                        StartY = instance.Y,
                        EndX = this.SvgUnitCalculator.Add(instance.X,
                                                          instance.Width),
                        EndY = this.SvgUnitCalculator.Add(instance.Y,
                                                          instance.Height),
                        StrokeWidth = instance.StrokeWidth
                      };

      return rightLine;
    }

    public SvgLine GetUpperLine(SvgRectangle instance)
    {
      var upperLine = new SvgLine
                      {
                        StartX = instance.X,
                        StartY = instance.Y,
                        EndX = this.SvgUnitCalculator.Add(instance.X,
                                                          instance.Width),
                        EndY = instance.Y,
                        StrokeWidth = instance.StrokeWidth
                      };

      return upperLine;
    }
  }
}