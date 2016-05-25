using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace Svg.Contrib.Render
{
  [PublicAPI]
  public interface ISvgElementTranslator<in TContainer>
    where TContainer : Container
  {
    void Translate([NotNull] SvgElement svgElement,
                   [NotNull] Matrix matrix,
                   [NotNull] TContainer container);
  }

  [PublicAPI]
  public interface ISvgElementTranslator<in TContainer, in TSvgElement> : ISvgElementTranslator<TContainer>
    where TContainer : Container
    where TSvgElement : SvgElement
  {
    void Translate([NotNull] TSvgElement svgElement,
                   [NotNull] Matrix matrix,
                   [NotNull] TContainer container);
  }
}