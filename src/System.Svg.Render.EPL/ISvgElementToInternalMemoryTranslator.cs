using System.Collections.Generic;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public interface ISvgElementToInternalMemoryTranslator : ISvgElementTranslator
  {
    IEnumerable<byte> TranslateForStoring([NotNull] SvgElement svgElement,
                                          [NotNull] Matrix matrix);
  }

  public interface ISvgElementToInternalMemoryTranslator<T> : ISvgElementToInternalMemoryTranslator,
                                                              ISvgElementTranslator<T>
    where T : SvgElement
  {
    IEnumerable<byte> TranslateForStoring([NotNull] T svgElement,
                                          [NotNull] Matrix matrix);
  }
}