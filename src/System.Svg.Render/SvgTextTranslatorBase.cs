using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public abstract class SvgTextTranslatorBase : SvgElementTranslatorBase<SvgText>
  {
    protected SvgTextTranslatorBase([NotNull] SvgUnitCalculatorBase svgUnitCalculator)
      : base(svgUnitCalculator) {}

    public override bool TryTranslate(SvgText instance,
                                      Matrix matrix,
                                      int targetDpi,
                                      out object translation)
    {
      var svgTextSpans = instance.Children.OfType<SvgTextSpan>()
                                 .ToArray();
      if (svgTextSpans.Any())
      {
        ICollection<object> translations = new LinkedList<object>();
        foreach (var svgTextSpan in svgTextSpans)
        {
          if (!this.TryTranslate(svgTextSpan,
                                 matrix,
                                 targetDpi,
                                 out translation))
          {
            return false;
          }
          if (translation != null)
          {
            translations.Add(translation);
          }
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

      var success = this.TryTranslate(instance,
                                      matrix,
                                      targetDpi,
                                      out translation);

      return success;
    }

    protected abstract bool TryTranslate([NotNull] SvgTextBase instance,
                                         [NotNull] Matrix matrix,
                                         int targetDpi,
                                         out object translation);
  }
}