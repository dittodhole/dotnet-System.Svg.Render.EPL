using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing.Drawing2D;
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

    public abstract string GetTranslation([NotNull] SvgDocument instance);

    public string GetTranslation([NotNull] SvgDocument instance,
                                 [NotNull] Matrix viewMatrix)
    {
      var translations = new LinkedList<object>();

      var parentMatrix = new Matrix();
      this.TranslateSvgElementAndChildren(instance,
                                          parentMatrix,
                                          viewMatrix,
                                          translations);

      var result = string.Join(Environment.NewLine,
                               translations);
      return result;
    }

    private void TranslateSvgElementAndChildren([NotNull] SvgElement svgElement,
                                                [NotNull] Matrix parentMatrix,
                                                [NotNull] Matrix viewMatrix,
                                                [NotNull] ICollection<object> translations)
    {
      var svgVisualElement = svgElement as SvgVisualElement;
      if (svgVisualElement != null)
      {
        // TODO consider performance here w/ the cast
        if (!svgVisualElement.Visible)
        {
          this.AddHiddenTranslation(svgElement,
                                    translations);
          return;
        }
      }

      var matrix = this.MultiplyTransformationsIntoNewMatrix(svgElement,
                                                             parentMatrix);

      // TODO write unit-test for dat shit :zzz:

      object translation;
      this.TranslateSvgElement(svgElement,
                               matrix,
                               viewMatrix,
                               out translation);

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
                                            matrix,
                                            viewMatrix,
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

    private void TranslateSvgElement([NotNull] SvgElement svgElement,
                                     [NotNull] Matrix matrix,
                                     [NotNull] Matrix viewMatrix,
                                     out object translation)
    {
      var type = svgElement.GetType();

      ISvgElementTranslator svgElementTranslator;
      if (!this.SvgElementTranslators.TryGetValue(type,
                                                  out svgElementTranslator))
      {
        translation = null;
        return;
      }

      matrix = matrix.Clone();
      matrix.Multiply(viewMatrix,
                      MatrixOrder.Append);

      svgElementTranslator.TranslateUntyped(svgElement,
                                            matrix,
                                            out translation);
    }

    public void RegisterTranslator<T>(ISvgElementTranslator<T> svgElementTranslator) where T : SvgElement
    {
      this.SvgElementTranslators[typeof(T)] = svgElementTranslator;
    }

    protected abstract void AddTranslation([NotNull] SvgElement svgElement,
                                           [NotNull] ICollection<object> translations,
                                           [NotNull] object translation);

    [Conditional("DEBUG")]
    protected abstract void AddHiddenTranslation([NotNull] SvgElement svgElement,
                                                 [NotNull] ICollection<object> translations);
  }
}