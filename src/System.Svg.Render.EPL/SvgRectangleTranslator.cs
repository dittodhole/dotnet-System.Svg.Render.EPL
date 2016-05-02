using System.Linq;

namespace System.Svg.Render.EPL
{
  public class SvgRectangleTranslator : SvgElementTranslator<SvgRectangle>
  {
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
      var translation = string.Join(Environment.NewLine,
                                    translations);

      return translation;
    }

    public SvgLine GetLeftLine(SvgRectangle instance)
    {
      var leftLine = new SvgLine // (10/10) - (10/110)
                     {
                       StartX = instance.X, // 10
                       StartY = instance.Y, // 10
                       EndX = instance.X, // 10
                       EndY = this.SvgUnitCalculator.Add(instance.Y,
                                                         instance.Height), // 110
                       StrokeWidth = instance.StrokeWidth
                     };
      return leftLine;
    }

    public SvgLine GetLowerLine(SvgRectangle instance)
    {
      var lowerLine = new SvgLine // (10/110) - (110/110)
                      {
                        StartX = instance.X, // 10
                        StartY = this.SvgUnitCalculator.Add(instance.Y,
                                                            instance.Height), // 110
                        EndX = this.SvgUnitCalculator.Add(instance.X,
                                                          instance.Width), // 110
                        EndY = this.SvgUnitCalculator.Add(instance.Y,
                                                          instance.Height), // 110
                        StrokeWidth = instance.StrokeWidth
                      };
      return lowerLine;
    }

    public SvgLine GetRightLine(SvgRectangle instance)
    {
      var rightLine = new SvgLine // (110/10) - (110/110)
                      {
                        StartX = this.SvgUnitCalculator.Add(instance.X,
                                                            instance.Width), // 110
                        StartY = instance.Y, // 10
                        EndX = this.SvgUnitCalculator.Add(instance.X,
                                                          instance.Width), // 110
                        EndY = this.SvgUnitCalculator.Add(instance.Y,
                                                          instance.Height), // 110
                        StrokeWidth = instance.StrokeWidth
                      };
      return rightLine;
    }

    public SvgLine GetUpperLine(SvgRectangle instance)
    {
      var upperLine = new SvgLine // (10/10) - (110/10)
                      {
                        StartX = instance.X, // 10
                        StartY = instance.Y, // 10
                        EndX = this.SvgUnitCalculator.Add(instance.X,
                                                          instance.Width), //110
                        EndY = instance.Y, // 10
                        StrokeWidth = instance.StrokeWidth
                      };
      return upperLine;
    }
  }
}