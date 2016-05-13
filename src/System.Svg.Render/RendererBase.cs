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
    // TODO maybe switch to HybridDictionary - in this scenario we have just a bunch of translators, ... but ... community?!
    [NotNull]
    private ConcurrentDictionary<Type, ISvgElementTranslator> SvgElementTranslators { get; } = new ConcurrentDictionary<Type, ISvgElementTranslator>();

    [NotNull]
    public abstract IEnumerable<byte> GetTranslation([NotNull] SvgDocument instance);

    [NotNull]
    protected IEnumerable<byte> GetTranslation([NotNull] SvgDocument instance,
                                               [NotNull] Matrix viewMatrix)
    {
      var parentMatrix = new Matrix();
      var result = this.TranslateSvgElementAndChildren(instance,
                                                       parentMatrix,
                                                       viewMatrix);

      return result;
    }

    [NotNull]
    private IEnumerable<byte> TranslateSvgElementAndChildren([NotNull] SvgElement svgElement,
                                                             [NotNull] Matrix parentMatrix,
                                                             [NotNull] Matrix viewMatrix)
    {
      var svgVisualElement = svgElement as SvgVisualElement;
      if (svgVisualElement != null)
      {
        // TODO consider performance here w/ the cast
        if (!svgVisualElement.Visible)
        {
          return Enumerable.Empty<byte>();
        }
      }

      var matrix = this.MultiplyTransformationsIntoNewMatrix(svgElement,
                                                             parentMatrix);

      // TODO write unit-test for dat shit :zzz:

      var translation = this.TranslateSvgElement(svgElement,
                                                 matrix,
                                                 viewMatrix);

      var result = translation.Concat(svgElement.Children.SelectMany(child => this.TranslateSvgElementAndChildren(child,
                                                                                                                  matrix,
                                                                                                                  viewMatrix)));

      return result;
    }

    [NotNull]
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

    protected virtual IEnumerable<byte> TranslateSvgElement([NotNull] SvgElement svgElement,
                                                            [NotNull] Matrix matrix,
                                                            [NotNull] Matrix viewMatrix)
    {
      var type = svgElement.GetType();

      ISvgElementTranslator svgElementTranslator;
      if (!this.SvgElementTranslators.TryGetValue(type,
                                                  out svgElementTranslator))
      {
        return null;
      }

      matrix = matrix.Clone();
      matrix.Multiply(viewMatrix,
                      MatrixOrder.Append);

      return svgElementTranslator.TranslateUntyped(svgElement,
                                                   matrix);
    }

    public void RegisterTranslator<T>([NotNull] ISvgElementTranslator<T> svgElementTranslator) where T : SvgElement
    {
      this.SvgElementTranslators[typeof(T)] = svgElementTranslator;
    }
  }
}