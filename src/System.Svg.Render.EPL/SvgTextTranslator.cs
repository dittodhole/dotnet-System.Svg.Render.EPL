using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgTextTranslator : SvgElementTranslator<SvgText>
  {
    // TODO translate dX and dY

    /// <exception cref="ArgumentNullException"><paramref name="svgUnitCalculator" /> is <see langword="null" />.</exception>
    public SvgTextTranslator(SvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator) {}

    public override object Translate(SvgText instance,
                                     Matrix matrix,
                                     int targetDpi)
    {
      object translation;

      var svgTextSpan = instance.Children.OfType<SvgTextSpan>()
                                .ToArray();
      if (svgTextSpan.Any())
      {
        var translations = svgTextSpan.Select(arg => this.Translate(arg,
                                                                    matrix,
                                                                    targetDpi))
                                      .Where(arg => arg != null)
                                      .ToArray();
        if (translations.Any())
        {
          translation = string.Join(Environment.NewLine,
                                    translations);
        }
        else
        {
          translation = null;
        }
      }
      else
      {
        translation = this.Translate(instance,
                                     matrix,
                                     targetDpi);
      }

      return translation;
    }

    private object Translate([NotNull] SvgTextBase instance,
                             [NotNull] Matrix matrix,
                             int targetDpi)
    {
      var text = this.RemoveIllegalCharacters(instance.Text);
      if (string.IsNullOrWhiteSpace(text))
      {
        return null;
      }

      object rotationTranslation;
      if (!this.SvgUnitCalculator.TryGetRotationTranslation(matrix,
                                                            out rotationTranslation))
      {
#if DEBUG
        return $"; could not get rotation translation: {instance.GetXML()}";
#else
        return null;
#endif
      }

      if (instance.X == null)
      {
#if DEBUG
        return $"; x is null: {instance.GetXML()}";
#else
        return null;
#endif
      }
      if (!instance.X.Any())
      {
#if DEBUG
        return $"; no x-coordinates: {instance.GetXML()}";
#else
        return null;
#endif
      }
      if (instance.Y == null)
      {
#if DEBUG
        return $"; y is null: {instance.GetXML()}";
#else
        return null;
#endif
      }
      if (!instance.Y.Any())
      {
#if DEBUG
        return $"; no y-coordinates: {instance.GetXML()}";
#else
        return null;
#endif
      }

      SvgUnit newX;
      SvgUnit newY;

      var x = instance.X.First();
      var y = instance.Y.First();
      if (!this.SvgUnitCalculator.TryApplyMatrix(x,
                                                 y,
                                                 matrix,
                                                 out newX,
                                                 out newY))
      {
#if DEBUG
        return $"; could not apply matrix on x and y: {instance.GetXML()}";
#else
        return null;
#endif
      }

      int horizontalStart;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(newX,
                                                     targetDpi,
                                                     out horizontalStart))
      {
#if DEBUG
        return $"; could not get device points (x): {instance.GetXML()}";
#else
        return null;
#endif
      }

      int verticalStart;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(newY,
                                                     targetDpi,
                                                     out verticalStart))
      {
#if DEBUG
        return $"; could not get device points (y): {instance.GetXML()}";
#else
        return null;
#endif
      }

      object fontTranslation;
      if (!this.SvgUnitCalculator.TryGetFontTranslation(instance,
                                                        matrix,
                                                        targetDpi,
                                                        out fontTranslation))
      {
#if DEBUG
        return $"; could not get font translation: {instance.GetXML()}";
#else
        return null;
#endif
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

      var translation = $@"A{horizontalStart},{verticalStart},{rotationTranslation},{fontTranslation},{reverseImage},""{text}""";

      return translation;
    }

    private string RemoveIllegalCharacters(string text)
    {
      // TODO add regex for removing illegal characters ...

      return text;
    }
  }
}