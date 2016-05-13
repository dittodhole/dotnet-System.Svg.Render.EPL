using System.Collections.Generic;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public abstract class SvgElementTranslatorBase<T> : ISvgElementTranslator<T>
    where T : SvgElement
  {
    [NotNull]
    public IEnumerable<byte> TranslateUntyped([NotNull] object untypedInstance,
                                              [NotNull] Matrix matrix)
    {
      var result = this.Translate((T) untypedInstance,
                                  matrix);

      return result;
    }

    [NotNull]
    public abstract IEnumerable<byte> Translate([NotNull] T instance,
                                                [NotNull] Matrix matrix);
  }
}