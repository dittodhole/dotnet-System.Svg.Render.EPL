using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgRectangleTranslator : SvgElementTranslatorBase<SvgRectangle>
  {
    // TODO add documentation/quote: strokes are printed inside the rectangle (calculation stuff)

    public SvgRectangleTranslator([NotNull] SvgLineTranslator svgLineTranslator,
                                  [NotNull] SvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator)
    {
      this.SvgLineTranslator = svgLineTranslator;
      this.SvgUnitCalculator = svgUnitCalculator;
    }

    [NotNull]
    protected SvgLineTranslator SvgLineTranslator { get; }

    [NotNull]
    private SvgUnitCalculator SvgUnitCalculator { get; }

    public override bool TryTranslate([NotNull] SvgRectangle instance,
                                      [NotNull] Matrix matrix,
                                      int targetDpi,
                                      out object translation)
    {
      // TODO fix calculation of stroke based on StrokeLineJoin

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

        var success = this.SvgLineTranslator.TryTranslate(svgLine,
                                                          matrix,
                                                          targetDpi,
                                                          out translation);

        return success;
      }

      SvgLine fillLine;
      if (!this.TryGetFillSvgLine(instance,
                                  out fillLine))
      {
#if DEBUG
        translation = $"; could not get filling line: {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
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
#if DEBUG
        translation = $"; could not get border lines: {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      ICollection<object> translations = new LinkedList<object>();
      foreach (var line in new[]
                           {
                             upperLine,
                             rightLine,
                             lowerLine,
                             leftLine,
                             fillLine
                           })
      {
        if (line == null)
        {
          continue;
        }

        if (!this.SvgLineTranslator.TryTranslate(line,
                                                 matrix,
                                                 targetDpi,
                                                 out translation))
        {
          return false;
        }

        translations.Add(translation);
      }

      if (translations.Any())
      {
        translation = string.Join(Environment.NewLine,
                                  translations);
      }
      else
      {
        translation = null;
      }

      return true;
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
      if (!this.SvgUnitCalculator.TryAdd(instance.X,
                                         instance.Width,
                                         out endX))
      {
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

      if (!this.TryGetUpperLine(instance,
                                out upperLine))
      {
        upperLine = null;
        rightLine = null;
        lowerLine = null;
        leftLine = null;
        return false;
      }

      if (!this.TryGetRightLine(instance,
                                out rightLine))
      {
        upperLine = null;
        rightLine = null;
        lowerLine = null;
        leftLine = null;
        return false;
      }
      if (!this.TryGetLowerLine(instance,
                                out lowerLine))
      {
        upperLine = null;
        rightLine = null;
        lowerLine = null;
        leftLine = null;
        return false;
      }

      if (!this.TryGetLeftLine(instance,
                               out leftLine))
      {
        upperLine = null;
        rightLine = null;
        lowerLine = null;
        leftLine = null;
        return false;
      }

      return true;
    }

    private bool TryGetUpperLine(SvgRectangle instance,
                                 out SvgLine upperLine)
    {
      var startX = instance.X;
      var y = instance.Y;

      SvgUnit endX;
      if (!this.SvgUnitCalculator.TryAdd(startX,
                                         instance.Width,
                                         out endX))
      {
        upperLine = null;
        return false;
      }

      upperLine = new SvgLine
                  {
                    StartX = startX,
                    StartY = y,
                    EndX = endX,
                    EndY = y,
                    StrokeWidth = instance.StrokeWidth
                  };

      return true;
    }

    private bool TryGetRightLine(SvgRectangle instance,
                                 out SvgLine rightLine)
    {
      SvgUnit startX;
      if (!this.SvgUnitCalculator.TryAdd(instance.X,
                                         instance.Width,
                                         out startX))
      {
        rightLine = null;
        return false;
      }

      var startY = instance.Y;

      SvgUnit endX;
      if (!this.SvgUnitCalculator.TryAdd(instance.X,
                                         instance.Width,
                                         out endX))
      {
        rightLine = null;
        return false;
      }

      SvgUnit endY;
      if (!this.SvgUnitCalculator.TryAdd(startY,
                                         instance.Height,
                                         out endY))
      {
        rightLine = null;
        return false;
      }

      rightLine = new SvgLine
                  {
                    StartX = startX,
                    StartY = startY,
                    EndX = endX,
                    EndY = endY,
                    StrokeWidth = instance.StrokeWidth
                  };

      return true;
    }

    private bool TryGetLowerLine(SvgRectangle instance,
                                 out SvgLine lowerLine)
    {
      var startX = instance.X;

      SvgUnit startY;
      if (!this.SvgUnitCalculator.TryAdd(instance.Y,
                                         instance.Height,
                                         out startY))
      {
        lowerLine = null;
        return false;
      }

      SvgUnit endX;
      if (!this.SvgUnitCalculator.TryAdd(startX,
                                         instance.Width,
                                         out endX))
      {
        lowerLine = null;
        return false;
      }

      SvgUnit endY;
      if (!this.SvgUnitCalculator.TryAdd(instance.Y,
                                         instance.Height,
                                         out endY))
      {
        lowerLine = null;
        return false;
      }

      lowerLine = new SvgLine
                  {
                    StartX = startX,
                    StartY = startY,
                    EndX = endX,
                    EndY = endY,
                    StrokeWidth = instance.StrokeWidth
                  };

      return true;
    }

    private bool TryGetLeftLine(SvgRectangle instance,
                                out SvgLine leftLine)
    {
      var x = instance.X;
      var startY = instance.Y;

      SvgUnit endY;
      if (!this.SvgUnitCalculator.TryAdd(startY,
                                         instance.Height,
                                         out endY))
      {
        leftLine = null;
        return false;
      }

      leftLine = new SvgLine
                 {
                   StartX = x,
                   StartY = startY,
                   EndX = x,
                   EndY = endY,
                   StrokeWidth = instance.StrokeWidth
                 };

      return true;
    }
  }
}