using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  [PublicAPI]
  public interface ISvgElementToInternalMemoryTranslator : ISvgElementTranslator<EplStream>
  {
    void TranslateForStoring([NotNull] SvgElement svgElement,
                             [NotNull] Matrix matrix,
                             [NotNull] EplStream container);
  }

  [PublicAPI]
  public interface ISvgElementToInternalMemoryTranslator<T> : ISvgElementToInternalMemoryTranslator,
                                                              ISvgElementTranslator<EplStream, T>
    where T : SvgElement
  {
    void TranslateForStoring([NotNull] T svgElement,
                             [NotNull] Matrix matrix,
                             [NotNull] EplStream container);
  }
}