using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  [PublicAPI]
  public interface ISvgElementTranslator<T>
  {
    void Translate([NotNull] SvgElement svgElement,
                   [NotNull] Matrix matrix,
                   [NotNull] Container<T> container);
  }

  [PublicAPI]
  public interface ISvgElementTranslator<T, TSvgElement> : ISvgElementTranslator<T>
    where TSvgElement : SvgElement
  {
    void Translate([NotNull] TSvgElement svgElement,
                   [NotNull] Matrix matrix,
                   [NotNull] Container<T> container);
  }
}