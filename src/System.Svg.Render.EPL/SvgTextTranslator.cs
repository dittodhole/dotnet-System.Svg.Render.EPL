using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Anotar.LibLog;
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
        LogTo.Error($"could not calculate start point and rotation");
        return null;
      }

      if (instance.X == null)
      {
        LogTo.Error($"{nameof(SvgTextBase.X)} is null");
        return null;
      }
      if (!instance.X.Any())
      {
        LogTo.Error($"no values in {nameof(SvgTextBase.X)}");
        return null;
      }
      if (instance.Y == null)
      {
        LogTo.Error($"{nameof(SvgTextBase.Y)} is null");
        return null;
      }
      if (!instance.Y.Any())
      {
        LogTo.Error($"no values in {nameof(SvgTextBase.Y)}");
        return null;
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
        LogTo.Error($"could not apply matrix");
        return null;
      }

      int horizontalStart;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(newX,
                                                     targetDpi,
                                                     out horizontalStart))
      {
        LogTo.Error($"could not translate {nameof(newX)} ({newX}) to device points");
        return null;
      }

      int verticalStart;
      if (!this.SvgUnitCalculator.TryGetDevicePoints(newY,
                                                     targetDpi,
                                                     out verticalStart))
      {
        LogTo.Error($"could not translate {nameof(newY)} ({newY}) to device points");
        return null;
      }

      object fontTranslation;
      if (!this.SvgUnitCalculator.TryGetFontTranslation(instance,
                                                        matrix,
                                                        targetDpi,
                                                        out fontTranslation))
      {
        LogTo.Error($"could not get font translation");
        return null;
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