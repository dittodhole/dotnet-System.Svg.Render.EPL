using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public interface ISvgElementTranslator
  {
    bool TryTranslateUntyped([NotNull] object untypedInstance,
                             [NotNull] Matrix matrix,
                             int targetDpi,
                             out object translation);
  }

  public interface ISvgElementTranslator<T> : ISvgElementTranslator
    where T : SvgElement
  {
    bool TryTranslate([NotNull] T instance,
                      [NotNull] Matrix matrix,
                      int targetDpi,
                      out object translation);
  }
}