using System.Drawing.Drawing2D;
using JetBrains.Annotations;

// ReSharper disable UnusedMemberInSuper.Global

namespace System.Svg.Render.EPL
{
  public interface ISvgElementToInternalMemoryTranslator : ISvgElementTranslator<EplStream>
  {
    void TranslateForStoring([NotNull] SvgElement svgElement,
                             [NotNull] Matrix matrix,
                             [NotNull] EplStream container);
  }

  public interface ISvgElementToInternalMemoryTranslator<T> : ISvgElementToInternalMemoryTranslator,
                                                              ISvgElementTranslator<EplStream, T>
    where T : SvgElement
  {
    void TranslateForStoring([NotNull] T svgElement,
                             [NotNull] Matrix matrix,
                             [NotNull] EplStream container);
  }
}