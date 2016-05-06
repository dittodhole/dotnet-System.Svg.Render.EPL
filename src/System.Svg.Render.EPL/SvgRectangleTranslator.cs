using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Anotar.LibLog;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgRectangleTranslator : SvgElementTranslator<SvgRectangle>
  {
    // TODO add documentation/quote: strokes are printed inside the rectangle (calculation stuff)

    /// <exception cref="ArgumentNullException"><paramref name="svgLineTranslator" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitCalculator" /> is <see langword="null" />.</exception>
    public SvgRectangleTranslator(SvgLineTranslator svgLineTranslator,
                                  SvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator)
    {
      if (svgLineTranslator == null)
      {
        throw new ArgumentNullException(nameof(svgLineTranslator));
      }

      this.SvgLineTranslator = svgLineTranslator;
    }

    protected SvgLineTranslator SvgLineTranslator { get; }

    public override object Translate([NotNull] SvgRectangle instance,
                                     [NotNull] Matrix matrix,
                                     int targetDpi)
    {
      // TODO allow diagnoal rectangle ...
      // TODO fix calculation of stroke based on StrokeLineJoin

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
                        StrokeWidth = instance.StrokeWidth,
                        Stroke = instance.Stroke
                      };

        translation = this.SvgLineTranslator.Translate(svgLine,
                                                       matrix,
                                                       targetDpi);
      }
      else
      {
        SvgLine fillLine;
        if (!this.TryGetFillSvgLine(instance,
                                    out fillLine))
        {
          LogTo.Error($"could not get line for filling");
          return null;
        }

        SvgLine upperLine;
        SvgLine rightLine;
        SvgLine lowerLine;
        SvgLine leftLine;
        if (!this.TryGetBorderSvgLines(instance,
                                       out upperLine,
                                       out rightLine,
                                       out lowerLine,
                                       out leftLine))
        {
          LogTo.Error($"could not get lines for borders");
          return null;
        }

        if (fillLine == null
            && upperLine == null
            && rightLine == null
            && lowerLine == null
            && leftLine == null)
        {
          translation = null;
        }
        else
        {
          var translations = new[]
                             {
                               upperLine,
                               rightLine,
                               lowerLine,
                               leftLine,
                               fillLine
                             }.Where(arg => arg != null)
                              .Select(arg => this.SvgLineTranslator.Translate(arg,
                                                                              matrix,
                                                                              targetDpi))
                              .Where(arg => arg != null);

          translation = string.Join(Environment.NewLine,
                                    translations);
        }
      }

      return translation;
    }

    private bool TryGetFillSvgLine([NotNull] SvgRectangle instance,
                                   out SvgLine fillLine)
    {
      // TODO fix dat for every scenario - test cases!

      var fillColor = (instance.Fill as SvgColourServer)?.Colour ?? Color.Empty;
      if (fillColor == Color.Empty)
      {
        fillLine = null;
        return true;
      }

      SvgUnit endX;
      try
      {
        endX = this.SvgUnitCalculator.Add(instance.X,
                                          instance.Width);
      }
      catch (ArgumentException argumentException)
      {
        LogTo.ErrorException($"could not calculate fill",
                             argumentException);
        fillLine = null;
        return false;
      }

      fillLine = new SvgLine
                 {
                   StartX = instance.X,
                   StartY = instance.Y,
                   EndX = endX,
                   EndY = instance.Y,
                   StrokeWidth = instance.Height,
                   Stroke = instance.Fill
                 };

      return true;
    }

    private bool TryGetBorderSvgLines([NotNull] SvgRectangle instance,
                                      out SvgLine upperLine,
                                      out SvgLine rightLine,
                                      out SvgLine lowerLine,
                                      out SvgLine leftLine)
    {
      var strokeColor = (instance.Stroke as SvgColourServer)?.Colour ?? Color.Empty;
      if (strokeColor == Color.Empty)
      {
        upperLine = null;
        rightLine = null;
        lowerLine = null;
        leftLine = null;
        return true;
      }

      try
      {
        upperLine = new SvgLine
                    {
                      StartX = instance.X,
                      StartY = instance.Y,
                      EndX = this.SvgUnitCalculator.Add(instance.X,
                                                        instance.Width),
                      EndY = instance.Y,
                      StrokeWidth = instance.StrokeWidth
                    };

        rightLine = new SvgLine
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

        lowerLine = new SvgLine
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

        leftLine = new SvgLine
                   {
                     StartX = instance.X,
                     StartY = instance.Y,
                     EndX = instance.X,
                     EndY = this.SvgUnitCalculator.Add(instance.Y,
                                                       instance.Height),
                     StrokeWidth = instance.StrokeWidth
                   };
      }
      catch (ArgumentException argumentException)
      {
        LogTo.ErrorException($"could not calculate fill",
                             argumentException);
        upperLine = null;
        rightLine = null;
        lowerLine = null;
        leftLine = null;
        return false;
      }

      return true;
    }
  }
}