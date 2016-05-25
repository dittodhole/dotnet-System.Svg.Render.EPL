using System.Drawing.Drawing2D;
using System.Svg;
using JetBrains.Annotations;

namespace Svg.Contrib.Render
{
  [PublicAPI]
  public abstract class SvgElementTranslatorBase<TContainer, TSvgElement> : ISvgElementTranslator<TContainer, TSvgElement>
    where TSvgElement : SvgElement
  {
    void ISvgElementTranslator<TContainer>.Translate([NotNull] SvgElement svgElement,
                                                     [NotNull] Matrix matrix,
                                                     [NotNull] Container<TContainer> container) => this.Translate((TSvgElement) svgElement,
                                                                                                       matrix,
                                                                                                       container);

    public abstract void Translate([NotNull] TSvgElement svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] Container<TContainer> container);
  }
}