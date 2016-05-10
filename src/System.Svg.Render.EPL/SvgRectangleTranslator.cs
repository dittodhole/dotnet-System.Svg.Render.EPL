using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgRectangleTranslator : SvgElementTranslatorBase<SvgRectangle>
  {
    public SvgRectangleTranslator([NotNull] ISvgUnitCalculator svgUnitCalculator,
                                  [NotNull] SvgLineTranslator svgLineTranslator)
      : base(svgUnitCalculator)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
      this.SvgLineTranslator = svgLineTranslator;
    }

    [NotNull]
    private ISvgUnitCalculator SvgUnitCalculator { get; }

    [NotNull]
    private SvgLineTranslator SvgLineTranslator { get; }

    public override bool TryTranslate([NotNull] SvgRectangle instance,
                                      [NotNull] Matrix matrix,
                                      int targetDpi,
                                      out object translation)
    {
      bool success;
      if (instance.Fill != SvgPaintServer.None
        && (instance.Fill as SvgColourServer)?.Colour != Color.White)
      {
        success = this.TryTranslateFilledBox(instance,
                                             matrix,
                                             targetDpi,
                                             out translation);
      }
      else
      {
        success = this.TryTranslateBox(instance,
                                       matrix,
                                       targetDpi,
                                       out translation);
      }

      return success;
    }

    private bool TryTranslateFilledBox(SvgRectangle instance,
                                       Matrix matrix,
                                       int targetDpi,
                                       out object translation)
    {
      SvgUnit endX;
      if (!this.SvgUnitCalculator.TryAdd(instance.X,
                                         instance.Width,
                                         out endX))
      {
#if DEBUG
        translation = "; could not get endX (fill) : {instance.GetXML()}";
#endif
        translation = null;
        return false;
      }

      SvgUnit startY;
      if (!this.SvgUnitCalculator.TryAdd(instance.Y,
                                         instance.Height,
                                         out startY))
      {
#if DEBUG
        translation = "; could not get startY (fill) : {instance.GetXML()}";
#endif
        translation = null;
        return false;
      }

      var svgLine = new SvgLine
                    {
                      StartX = instance.X,
                      StartY = startY,
                      EndX = endX,
                      EndY = startY,
                      StrokeWidth = instance.Height
                    };
      var success = this.SvgLineTranslator.TryTranslate(svgLine,
                                                        matrix,
                                                        targetDpi,
                                                        out translation);

      return success;
    }

    private bool TryTranslateBox(SvgRectangle instance,
                                 Matrix matrix,
                                 int targetDpi,
                                 out object translation)
    {
      int horizontalStart;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.X,
                                                     targetDpi,
                                                     out horizontalStart))
      {
#if DEBUG
        translation = $"; could not get device points (startX): {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      int verticalStart;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.Y,
                                                     targetDpi,
                                                     out verticalStart))
      {
#if DEBUG
        translation = $"; could not get device points (startY): {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      SvgUnit endX;
      if (!this.SvgUnitCalculator.TryAdd(instance.X,
                                         instance.Width,
                                         out endX))
      {
#if DEBUG
        translation = $"; could not add x and width: {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      int horizontalEnd;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(endX,
                                                     targetDpi,
                                                     out horizontalEnd))
      {
#if DEBUG
        translation = $"; could not get device points (endX): {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      SvgUnit endY;
      if (!this.SvgUnitCalculator.TryAdd(instance.Y,
                                         instance.Height,
                                         out endY))
      {
#if DEBUG
        translation = $"; could not add y and height: {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      int verticalEnd;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(endY,
                                                     targetDpi,
                                                     out verticalEnd))
      {
#if DEBUG
        translation = $"; could not get device points (endY): {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      int lineThickness;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.StrokeWidth,
                                                     targetDpi,
                                                     out lineThickness))
      {
#if DEBUG
        translation = $"; could not get device points (stroke): {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      horizontalStart -= (int) Math.Ceiling(lineThickness / 2f);
      verticalStart -= (int) Math.Ceiling(lineThickness / 2f);
      horizontalEnd += (int) Math.Ceiling(lineThickness / 2f);
      verticalEnd += (int) Math.Ceiling(lineThickness / 2f);

      this.SvgUnitCalculator.ApplyMatrixToDevicePoints(horizontalStart,
                                                       verticalStart,
                                                       matrix,
                                                       out horizontalStart,
                                                       out verticalStart);

      this.SvgUnitCalculator.ApplyMatrixToDevicePoints(horizontalEnd,
                                                       verticalEnd,
                                                       matrix,
                                                       out horizontalEnd,
                                                       out verticalEnd);

      translation = $"X{horizontalStart},{verticalStart},{lineThickness},{horizontalEnd},{verticalEnd}";

      return true;
    }
  }
}