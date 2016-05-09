using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Svg.Transforms;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public abstract class RendererBase
  {
    protected RendererBase([NotNull] ISvgUnitCalculator svgUnitCalculator)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
    }

    [NotNull]
    private ISvgUnitCalculator SvgUnitCalculator { get; }

    // TODO maybe switch to HybridDictionary - in this scenario we have just a bunch of translators, ... but ... community?!
    [NotNull]
    private ConcurrentDictionary<Type, ISvgElementTranslator> SvgElementTranslators { get; } = new ConcurrentDictionary<Type, ISvgElementTranslator>();

    public string GetTranslation([NotNull] SvgDocument instance,
                                 int targetDpi)
    {
      var translation = this.GetTranslation(instance,
                                            new Matrix(),
                                            targetDpi);

      return translation;
    }

    public string GetTranslation([NotNull] SvgDocument instance,
                                 [NotNull] Matrix viewMatrix,
                                 int targetDpi)
    {
      var translations = new LinkedList<object>();

      var parentMatrix = new Matrix();
      this.TranslateSvgElementAndChildren(instance,
                                          parentMatrix,
                                          viewMatrix,
                                          targetDpi,
                                          translations);

      if (translations.Any())
      {
        return string.Join(Environment.NewLine,
                           translations);
      }

      return null;
    }

    private void TranslateSvgElementAndChildren([NotNull] SvgElement svgElement,
                                                [NotNull] Matrix parentMatrix,
                                                [NotNull] Matrix viewMatrix,
                                                int targetDpi,
                                                [NotNull] ICollection<object> translations)
    {
      var svgVisualElement = svgElement as SvgVisualElement;
      if (svgVisualElement != null)
      {
        // TODO consider performance here w/ the cast
        if (!svgVisualElement.Visible)
        {
#if DEBUG
          this.AddHiddenTranslation(svgElement,
                                    translations);
#endif
          return;
        }
      }

      parentMatrix = this.MultiplyTransformationsIntoNewMatrix(svgElement,
                                                               parentMatrix);

      var matrix = viewMatrix.Clone();
      matrix.Multiply(parentMatrix,
                      MatrixOrder.Append);

      object translation;
      if (!this.TryTranslateSvgElement(svgElement,
                                       matrix,
                                       targetDpi,
                                       out translation))
      {
#if DEBUG
        this.AddFailedTranslation(svgElement,
                                  translations,
                                  translation);
#endif
        return;
      }

      if (translation != null)
      {
        this.AddTranslation(svgElement,
                            translations,
                            translation);
      }

      foreach (var child in svgElement.Children)
      {
        if (child == null)
        {
          continue;
        }

        this.TranslateSvgElementAndChildren(child,
                                            parentMatrix,
                                            viewMatrix,
                                            targetDpi,
                                            translations);
      }
    }

    private Matrix MultiplyTransformationsIntoNewMatrix([NotNull] ISvgTransformable svgTransformable,
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

    private bool TryTranslateSvgElement([NotNull] SvgElement svgElement,
                                        [NotNull] Matrix matrix,
                                        int targetDpi,
                                        out object translation)
    {
      var type = svgElement.GetType();

      ISvgElementTranslator svgElementTranslator;
      if (!this.SvgElementTranslators.TryGetValue(type,
                                                  out svgElementTranslator))
      {
        translation = null;
        return true;
      }

      return svgElementTranslator.TryTranslateUntyped(svgElement,
                                                      matrix,
                                                      targetDpi,
                                                      out translation);
    }

    public void RegisterTranslator<T>(ISvgElementTranslator<T> svgElementTranslator) where T : SvgElement
    {
      this.SvgElementTranslators[typeof(T)] = svgElementTranslator;
    }

    protected abstract void AddTranslation(SvgElement svgElement,
                                           ICollection<object> translations,
                                           object translation);

    protected abstract void AddFailedTranslation(SvgElement svgElement,
                                                 ICollection<object> translations,
                                                 object translation);

    protected abstract void AddHiddenTranslation(SvgElement svgElement,
                                                 ICollection<object> translations);
  }
}