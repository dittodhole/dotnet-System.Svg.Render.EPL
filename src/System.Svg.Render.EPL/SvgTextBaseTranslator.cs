using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgTextBaseTranslator<T> : SvgTextTranslatorBase<T>
    where T : SvgTextBase
  {
    // TODO translate dX and dY
    // TODO translate rotation

    public SvgTextBaseTranslator([NotNull] SvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
    }

    [NotNull]
    private SvgUnitCalculator SvgUnitCalculator { get; }

    public float LineHeightFactor { get; set; } = 1.25f;

    public override bool TryTranslate([NotNull] T instance,
                                      [NotNull] Matrix matrix,
                                      int targetDpi,
                                      out object translation)
    {
      var text = this.RemoveIllegalCharacters(instance.Text);
      if (string.IsNullOrWhiteSpace(text))
      {
#if DEBUG
        translation = $"; text is empty: {instance.GetXML()}";
#else
        translation = null;
#endif
        return true;
      }

      if (instance.X == null)
      {
#if DEBUG
        translation = $"; x is null: {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      if (!instance.X.Any())
      {
#if DEBUG
        translation = $"; no x-coordinates: {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      if (instance.Y == null)
      {
#if DEBUG
        translation = $"; y is null: {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      if (!instance.Y.Any())
      {
#if DEBUG
        translation = $"; no y-coordinates: {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      int x;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.X.First(),
                                                     targetDpi,
                                                     out x))
      {
#if DEBUG
        translation = $"; could not get device points (y): {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      int y;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.Y.First(),
                                                     targetDpi,
                                                     out y))
      {
#if DEBUG
        translation = $"; could not get device points (x): {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      int fontSize;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(instance.FontSize,
                                                     targetDpi,
                                                     out fontSize))
      {
#if DEBUG
        translation = $"; could not get device points (fontSize): {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      y -= (int) Math.Ceiling(fontSize / this.LineHeightFactor);

      this.SvgUnitCalculator.ApplyMatrixToDevicePoints(x,
                                                       y,
                                                       matrix,
                                                       out x,
                                                       out y);

      var rotationTranslation = this.SvgUnitCalculator.GetRotationTranslation(matrix);

      object fontTranslation;
      if (!this.SvgUnitCalculator.TryGetFontTranslation(fontSize,
                                                        matrix,
                                                        targetDpi,
                                                        out fontTranslation))
      {
#if DEBUG
        translation = $"; could not get font translation: {instance.GetXML()}";
#else
        translation = null;
#endif
        return false;
      }

      string reverseImage;
      if ((instance.Fill as SvgColourServer)?.Colour == Color.White)
      {
        reverseImage = "R";
      }
      else
      {
        reverseImage = "N";
      }

      translation = $@"A{x},{y},{rotationTranslation},{fontTranslation},{reverseImage},""{text}""";

      return true;
    }

    private string RemoveIllegalCharacters(string text)
    {
      // TODO add regex for removing illegal characters ...

      return text;
    }
  }
}