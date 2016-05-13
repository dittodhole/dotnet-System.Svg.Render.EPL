using System.Collections.Generic;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public interface ISvgElementTranslator
  {
    [NotNull]
    IEnumerable<byte> TranslateUntyped([NotNull] object untypedInstance,
                                       [NotNull] Matrix matrix);
  }

  public interface ISvgElementTranslator<T> : ISvgElementTranslator
    where T : SvgElement
  {
    [NotNull]
    IEnumerable<byte> Translate([NotNull] T instance,
                                [NotNull] Matrix matrix);
  }
}