using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using Anotar.LibLog;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgTextTranslator : SvgElementTranslator<SvgText>
  {
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

      // TODO here comes the magic!
      var fontSelection = 1;
      // VALUE    203dpi        300dpi
      // ==================================
      //  1       20.3cpi       25cpi
      //          6pts          4pts
      //          8x12 dots     12x20 dots
      // ==================================
      //  2       16.9cpi       18.75cpi
      //          7pts          6pts
      //          10x16 dots    16x28 dots
      // ==================================
      //  3       14.5cpi       15cpi
      //          10pts         8pts
      //          12x20 dots    20x36 dots
      // ==================================
      //  4       12.7cpi       12.5cpi
      //          12pts         10pts
      //          14x24 dots    24x44 dots
      // ==================================
      //  5       5.6cpi        6.25cpi
      //          24pts         21pts
      //          32x48 dots    48x80 dots
      // ==================================

      var horizontalMultiplier = 1; // Accepted Values: 1–6, 8
      var verticalMultiplier = 1; // Accepted Values: 1–9

      string reverseImage;
      if ((instance.Fill as SvgColourServer)?.Colour == Color.White)
      {
        reverseImage = "R";
      }
      else
      {
        reverseImage = "N";
      }

      var translation = $@"A{horizontalStart},{verticalStart},{rotationTranslation},{fontSelection},{horizontalMultiplier},{verticalMultiplier},{reverseImage},""{text}""";

      return translation;
    }

    protected virtual string RemoveIllegalCharacters(string text)
    {
      return text;
    }
  }
}