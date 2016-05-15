using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public abstract class SvgElementToInternalMemoryTranslator<TSvgElement> : SvgElementTranslatorBase<TSvgElement>,
                                                                            ISvgElementToInternalMemoryTranslator<TSvgElement>
    where TSvgElement : SvgElement
  {
    public bool AssumeStoredInInternalMemory { get; set; } = false;

    public abstract void TranslateForStoring([NotNull] TSvgElement svgElement,
                                             [NotNull] Matrix matrix,
                                             [NotNull] EplStream container);

    void ISvgElementToInternalMemoryTranslator.TranslateForStoring([NotNull] SvgElement svgElement,
                                                                   [NotNull] Matrix matrix,
                                                                   [NotNull] EplStream container) => this.TranslateForStoring((TSvgElement) svgElement,
                                                                                                                              matrix,
                                                                                                                              container);
  }
}