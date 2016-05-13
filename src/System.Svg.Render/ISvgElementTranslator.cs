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
}