using System.Svg;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.ZPL
{
  [PublicAPI]
  public abstract class SvgElementTranslatorBase<TSvgElement> : SvgElementTranslatorBase<ZplStream, TSvgElement>
    where TSvgElement : SvgElement {}
}