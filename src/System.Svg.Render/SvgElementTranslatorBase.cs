using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public abstract class SvgElementTranslatorBase<TContainer, TSvgElement> : ISvgElementTranslator<TContainer, TSvgElement>
    where TSvgElement : SvgElement
  {
    void ISvgElementTranslator<TContainer>.Translate([NotNull] SvgElement svgElement,
                                                     [NotNull] Matrix matrix,
                                                     [NotNull] TContainer container) => this.Translate((TSvgElement) svgElement,
                                                                                                       matrix,
                                                                                                       container);

    public abstract void Translate([NotNull] TSvgElement svgElement,
                                   [NotNull] Matrix matrix,
                                   [NotNull] TContainer container);
  }
}