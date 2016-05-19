using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  [PublicAPI]
  public abstract class SvgElementTranslatorBase<TSvgElement> : SvgElementTranslatorBase<EplStream, TSvgElement>
    where TSvgElement : SvgElement {}
}