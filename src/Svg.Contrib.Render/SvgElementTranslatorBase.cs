using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace Svg.Contrib.Render
{
  [PublicAPI]
  public abstract class SvgElementTranslatorBase<TContainer, TSvgElement> : ISvgElementTranslator<TContainer, TSvgElement>
    where TSvgElement : SvgElement
    where TContainer : Container
  {
    void ISvgElementTranslator<TContainer>.Translate([NotNull] SvgElement svgElement,
                                                     [NotNull] Matrix sourceMatrix,
                                                     [NotNull] Matrix viewMatrix,
                                                     [NotNull] TContainer container) => this.Translate((TSvgElement) svgElement,
                                                                                                       sourceMatrix,
                                                                                                       viewMatrix,
                                                                                                       container);

    public abstract void Translate([NotNull] TSvgElement svgElement,
                                   [NotNull] Matrix sourceMatrix,
                                   [NotNull] Matrix viewMatrix,
                                   [NotNull] TContainer container);
  }
}