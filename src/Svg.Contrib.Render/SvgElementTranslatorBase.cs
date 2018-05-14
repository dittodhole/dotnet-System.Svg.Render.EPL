using System;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace Svg.Contrib.Render
{
  [PublicAPI]
  public abstract class SvgElementTranslatorBase<TContainer, TSvgElement> : ISvgElementTranslator<TContainer, TSvgElement>
    where TSvgElement : SvgElement
    where TContainer : Container
  {
    /// <exception cref="ArgumentNullException"><paramref name="svgElement" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container" /> is <see langword="null" />.</exception>
    void ISvgElementTranslator<TContainer>.Translate(SvgElement svgElement,
                                                     Matrix sourceMatrix,
                                                     Matrix viewMatrix,
                                                     TContainer container)
    {
      if (svgElement == null)
      {
        throw new ArgumentNullException(nameof(svgElement));
      }
      if (sourceMatrix == null)
      {
        throw new ArgumentNullException(nameof(sourceMatrix));
      }
      if (viewMatrix == null)
      {
        throw new ArgumentNullException(nameof(viewMatrix));
      }
      if (container == null)
      {
        throw new ArgumentNullException(nameof(container));
      }

      this.Translate((TSvgElement) svgElement,
                     sourceMatrix,
                     viewMatrix,
                     container);
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgElement" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container" /> is <see langword="null" />.</exception>
    public abstract void Translate(TSvgElement svgElement,
                                   Matrix sourceMatrix,
                                   Matrix viewMatrix,
                                   TContainer container);
  }
}
