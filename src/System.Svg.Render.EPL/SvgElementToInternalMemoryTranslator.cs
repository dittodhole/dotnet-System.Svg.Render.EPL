using System.Collections.Generic;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public abstract class SvgElementToInternalMemoryTranslator<T> : SvgElementTranslatorBase<T>,
                                                                  ISvgElementToInternalMemoryTranslator<T>
    where T : SvgElement
  {
    public abstract IEnumerable<byte> TranslateForStoring([NotNull] T instance,
                                                          [NotNull] Matrix matrix);

    public bool AssumeStoredInInternalMemory { get; set; }

    public IEnumerable<byte> TranslateUntypedForStoring([NotNull] object untypedInstance,
                                                        [NotNull] Matrix matrix)
    {
      var result = this.TranslateForStoring((T) untypedInstance,
                                            matrix);

      return result;
    }
  }
}