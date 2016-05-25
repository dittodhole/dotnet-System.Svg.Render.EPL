using System.Svg;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public abstract class SvgElementTranslatorBase<TSvgElement> : SvgElementTranslatorBase<EplStream, TSvgElement>
    where TSvgElement : SvgElement {}
}