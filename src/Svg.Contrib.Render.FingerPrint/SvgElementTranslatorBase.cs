using JetBrains.Annotations;

namespace Svg.Contrib.Render.FingerPrint
{
  [PublicAPI]
  public abstract class SvgElementTranslatorBase<TSvgElement> : SvgElementTranslatorBase<FingerPrintContainer, TSvgElement>
    where TSvgElement : SvgElement {}
}