using System.Drawing;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public abstract class SvgElementTranslatorBase<T> : ISvgElementTranslator<T>
    where T : SvgElement
  {
    protected SvgElementTranslatorBase([NotNull] ISvgUnitCalculator svgUnitCalculator)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
    }

    [NotNull]
    private ISvgUnitCalculator SvgUnitCalculator { get; }

    public bool TryTranslateUntyped([NotNull] object untypedInstance,
                                    [NotNull] Matrix matrix,
                                    Point origin,
                                    int targetDpi,
                                    out object translation)
    {
      var success = this.TryTranslate((T) untypedInstance,
                                      matrix,
                                      origin,
                                      targetDpi,
                                      out translation);

      return success;
    }

    public virtual bool TryTranslate([NotNull] T instance,
                                     [NotNull] Matrix matrix,
                                     Point origin,
                                     int targetDpi,
                                     out object translation)
    {
      translation = null;
      return true;
    }
  }
}