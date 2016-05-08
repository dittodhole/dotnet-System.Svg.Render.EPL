using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public interface ISvgElementTranslator {
    bool TryTranslateUntyped([NotNull] object untypedInstance,
                                             [NotNull] Matrix matrix,
                                             int targetDpi,
                                             out Matrix newMatrix,
                                             out object translation);
  }
}