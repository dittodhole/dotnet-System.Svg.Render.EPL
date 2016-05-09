using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgRectangleTranslator : SvgElementTranslatorBase<SvgRectangle>
  {
    public SvgRectangleTranslator([NotNull] ISvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
    }

    [NotNull]
    private ISvgUnitCalculator SvgUnitCalculator { get; }

    public override bool TryTranslate([NotNull] SvgRectangle instance,
                                      [NotNull] Matrix matrix,
                                      int targetDpi,
                                      out object translation)
    {
      // TODO implement filling

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

      translation = $"X{horizontalStart},{verticalStart},{lineThickness},{horizontalEnd},{verticalEnd}";

      return true;
    }
  }
}