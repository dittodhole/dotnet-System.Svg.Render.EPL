using JetBrains.Annotations;

namespace System.Svg.Render.ZPL
{
  [PublicAPI]
  public abstract class SvgElementTranslatorBase<TSvgElement> : SvgElementTranslatorBase<ZplStream, TSvgElement>
    where TSvgElement : SvgElement {}
}