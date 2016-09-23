using System;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace Svg.Contrib.Render
{
  [PublicAPI]
  public interface ISvgElementTranslator<in TContainer>
    where TContainer : Container
  {
    /// <exception cref="ArgumentNullException"><paramref name="svgElement" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container" /> is <see langword="null" />.</exception>
    void Translate([NotNull] SvgElement svgElement,
                   [NotNull] Matrix sourceMatrix,
                   [NotNull] Matrix viewMatrix,
                   [NotNull] TContainer container);
  }

  [PublicAPI]
  public interface ISvgElementTranslator<in TContainer, in TSvgElement> : ISvgElementTranslator<TContainer>
    where TContainer : Container
    where TSvgElement : SvgElement
  {
    /// <exception cref="ArgumentNullException"><paramref name="svgElement" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container" /> is <see langword="null" />.</exception>
    void Translate([NotNull] TSvgElement svgElement,
                   [NotNull] Matrix sourceMatrix,
                   [NotNull] Matrix viewMatrix,
                   [NotNull] TContainer container);
  }
}
