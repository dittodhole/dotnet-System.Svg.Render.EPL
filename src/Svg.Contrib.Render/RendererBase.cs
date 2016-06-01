using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
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

    [CanBeNull]
    [Pure]
    [MustUseReturnValue]
    protected virtual ISvgElementTranslator<TContainer> GetTranslator([NotNull] Type type)
    {
      ISvgElementTranslator<TContainer> svgElementTranslator;
      // ReSharper disable ExceptionNotDocumentedOptional
      if (!this.SvgElementTranslators.TryGetValue(type,
                                                  out svgElementTranslator))
        // ReSharper restore ExceptionNotDocumentedOptional
      {
        return null;
      }

      return svgElementTranslator;
    }

    public virtual void RegisterTranslator<TSvgElement>([NotNull] ISvgElementTranslator<TContainer, TSvgElement> svgElementTranslator) where TSvgElement : SvgElement
    {
      // ReSharper disable ExceptionNotDocumentedOptional
      this.SvgElementTranslators[typeof(TSvgElement)] = svgElementTranslator;
      // ReSharper restore ExceptionNotDocumentedOptional
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    public abstract TContainer GetTranslation([NotNull] SvgDocument svgDocument,
                                              [NotNull] Matrix viewMatrix);

    protected virtual void TranslateSvgElementAndChildren([NotNull] SvgElement svgElement,
                                                          [NotNull] Matrix sourceMatrix,
                                                          [NotNull] Matrix viewMatrix,
                                                          [NotNull] TContainer container)
    {
      var svgVisualElement = svgElement as SvgVisualElement;
      if (svgVisualElement != null)
      {
        // TODO consider performance here w/ the cast
        if (!svgVisualElement.Visible)
        {
          return;
        }
      }

      sourceMatrix = this.MultiplyTransformationsIntoNewMatrix(svgElement,
                                                               sourceMatrix);

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

    protected virtual void TranslateSvgElement([NotNull] SvgElement svgElement,
                                               [NotNull] Matrix sourceMatrix,
                                               [NotNull] Matrix viewMatrix,
                                               [NotNull] TContainer container)
    {
      var type = svgElement.GetType();

      var svgElementTranslator = this.GetTranslator(type);

      svgElementTranslator?.Translate(svgElement,
                                      sourceMatrix,
                                      viewMatrix,
                                      container);
    }
  }
}