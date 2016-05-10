using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public abstract class SvgElementTranslatorBase<T> : ISvgElementTranslator<T>
    where T : SvgElement
  {
    public void TranslateUntyped([NotNull] object untypedInstance,
                                 [NotNull] Matrix matrix,
                                 out object translation)
    {
      this.Translate((T) untypedInstance,
                     matrix,
                     out translation);
    }

    public abstract void Translate([NotNull] T instance,
                                   [NotNull] Matrix matrix,
                                   out object translation);
  }
}