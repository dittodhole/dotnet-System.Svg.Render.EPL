using System;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using Svg;
using Svg.Transforms;
using JetBrains.Annotations;

namespace Svg.Contrib.Render
{
  [PublicAPI]
  public abstract class RendererBase
  {
    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected Matrix MultiplyTransformationsIntoNewMatrix([NotNull] ISvgTransformable svgTransformable,
                                                          [NotNull] Matrix matrix)
    {
      var result = default(Matrix);
      foreach (var transformation in svgTransformable.Transforms)
      {
        var transformationType = transformation.GetType();
        if (!this.IsTransformationAllowed(svgTransformable,
                                          transformationType))
        {
          continue;
        }

        var matrixToMultiply = transformation.Matrix;
        if (matrixToMultiply == null)
        {
          continue;
        }

        if (result == null)
        {
          result = matrix.Clone();
        }

        result.Multiply(matrixToMultiply,
                        MatrixOrder.Append);
      }

      return result ?? matrix;
    }

    [Pure]
    [MustUseReturnValue]
    protected virtual bool IsTransformationAllowed([NotNull] ISvgTransformable svgTransformable,
                                                   [NotNull] Type type)
    {
      if (type == typeof(SvgMatrix))
      {
        return true;
      }
      if (type == typeof(SvgRotate))
      {
        return true;
      }
      if (type == typeof(SvgScale))
      {
        return true;
      }
      if (type == typeof(SvgTranslate))
      {
        return true;
      }

      return false;
    }
  }

  [PublicAPI]
  public abstract class RendererBase<TContainer> : RendererBase
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
    public abstract TContainer GetTranslation([NotNull] SvgDocument svgDocument);

    protected virtual void TranslateSvgElementAndChildren([NotNull] SvgElement svgElement,
                                                          [NotNull] Matrix parentMatrix,
                                                          [NotNull] Matrix viewMatrix,
                                                          [NotNull] Container<TContainer> container)
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

      var matrix = this.MultiplyTransformationsIntoNewMatrix(svgElement,
                                                             parentMatrix);

      this.TranslateSvgElement(svgElement,
                               matrix,
                               viewMatrix,
                               container);

      foreach (var child in svgElement.Children)
      {
        this.TranslateSvgElementAndChildren(child,
                                            matrix,
                                            viewMatrix,
                                            container);
      }
    }

    protected virtual void TranslateSvgElement([NotNull] SvgElement svgElement,
                                               [NotNull] Matrix matrix,
                                               [NotNull] Matrix viewMatrix,
                                               [NotNull] Container<TContainer> container)
    {
      var type = svgElement.GetType();

      var svgElementTranslator = this.GetTranslator(type);
      if (svgElementTranslator == null)
      {
        return;
      }

      matrix = matrix.Clone();
      matrix.Multiply(viewMatrix,
                      MatrixOrder.Append);

      svgElementTranslator.Translate(svgElement,
                                     matrix,
                                     container);
    }
  }
}