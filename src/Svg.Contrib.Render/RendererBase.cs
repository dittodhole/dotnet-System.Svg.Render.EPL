using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

// ReSharper disable VirtualMemberNeverOverriden.Global

namespace Svg.Contrib.Render
{
  [PublicAPI]
  public abstract class RendererBase<TContainer>
    where TContainer : Container
  {
    // TODO maybe switch to HybridDictionary - in this scenario we have just a bunch of translators, ... but ... community?!
    [NotNull]
    [ItemNotNull]
    private IDictionary<Type, ISvgElementTranslator<TContainer>> SvgElementTranslators { get; } = new Dictionary<Type, ISvgElementTranslator<TContainer>>();

    /// <exception cref="ArgumentNullException"><paramref name="type" /> is <see langword="null" />.</exception>
    [CanBeNull]
    [Pure]
    protected virtual ISvgElementTranslator<TContainer> GetTranslator([NotNull] Type type)
    {
      if (type == null)
      {
        throw new ArgumentNullException(nameof(type));
      }

      if (!this.SvgElementTranslators.TryGetValue(type,
                                                  out ISvgElementTranslator<TContainer> svgElementTranslator))
      {
        return null;
      }

      return svgElementTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgElementTranslator" /> is <see langword="null" />.</exception>
    public virtual void RegisterTranslator<TSvgElement>([NotNull] ISvgElementTranslator<TContainer, TSvgElement> svgElementTranslator)
      where TSvgElement : SvgElement
    {
      this.SvgElementTranslators[typeof(TSvgElement)] = svgElementTranslator ?? throw new ArgumentNullException(nameof(svgElementTranslator));
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgDocument" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public abstract TContainer GetTranslation([NotNull] SvgDocument svgDocument,
                                              [NotNull] Matrix viewMatrix);

    /// <exception cref="ArgumentNullException"><paramref name="svgElement" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container" /> is <see langword="null" />.</exception>
    protected virtual void TranslateSvgElementAndChildren([NotNull] SvgElement svgElement,
                                                          [NotNull] Matrix sourceMatrix,
                                                          [NotNull] Matrix viewMatrix,
                                                          [NotNull] TContainer container)
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

      var svgVisualElement = svgElement as SvgVisualElement;
      if (svgVisualElement != null)
      {
        // TODO consider performance here w/ the cast
        if (!svgVisualElement.Visible)
        {
          return;
        }
      }

      if (svgElement.Transforms.Any())
      {
        sourceMatrix = sourceMatrix.Clone();
        sourceMatrix.Multiply(svgElement.Transforms.GetMatrix());
      }

      this.TranslateSvgElement(svgElement,
                               sourceMatrix,
                               viewMatrix,
                               container);

      foreach (var child in svgElement.Children)
      {
        this.TranslateSvgElementAndChildren(child,
                                            sourceMatrix,
                                            viewMatrix,
                                            container);
      }
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgElement" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="sourceMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="viewMatrix" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="container" /> is <see langword="null" />.</exception>
    protected virtual void TranslateSvgElement([NotNull] SvgElement svgElement,
                                               [NotNull] Matrix sourceMatrix,
                                               [NotNull] Matrix viewMatrix,
                                               [NotNull] TContainer container)
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

      var type = svgElement.GetType();

      var svgElementTranslator = this.GetTranslator(type);

      svgElementTranslator?.Translate(svgElement,
                                      sourceMatrix,
                                      viewMatrix,
                                      container);
    }
  }
}
