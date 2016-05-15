using System.Collections.Generic;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public abstract class SvgElementToInternalMemoryTranslator<T> : SvgElementTranslatorBase<T>,
                                                                  ISvgElementToInternalMemoryTranslator<T>
    where T : SvgElement
  {
    public abstract IEnumerable<byte> TranslateForStoring([NotNull] T svgElement,
                                                          [NotNull] Matrix matrix);

    public bool AssumeStoredInInternalMemory { get; set; }

    public IEnumerable<byte> TranslateUntypedForStoring([NotNull] SvgElement svgElement,
                                                        [NotNull] Matrix matrix)
    {
      var result = this.TranslateForStoring((T) svgElement,
                                            matrix);

      return result;
    }
  }
}