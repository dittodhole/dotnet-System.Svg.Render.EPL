using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public interface ISvgElementTranslator
  {
    void TranslateUntyped([NotNull] object untypedInstance,
                          [NotNull] Matrix matrix,
                          out object translation);
  }

  public interface ISvgElementTranslator<T> : ISvgElementTranslator
    where T : SvgElement
  {
    void Translate([NotNull] T instance,
                   [NotNull] Matrix matrix,
                   out object translation);
  }
}