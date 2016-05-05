using System.Drawing.Drawing2D;
using Anotar.LibLog;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public abstract class SvgElementTranslator
  {
    internal abstract void TranslateUntyped(object untypedInstance,
                                            Matrix matrix,
                                            int targetDpi,
                                            out Matrix newMatrix,
                                            out object translation);
  }

  public abstract class SvgElementTranslator<T> : SvgElementTranslator
    where T : SvgElement
  {
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitCalculator" /> is <see langword="null" />.</exception>
    protected SvgElementTranslator(SvgUnitCalculator svgUnitCalculator)
    {
      if (svgUnitCalculator == null)
      {
        throw new ArgumentNullException(nameof(svgUnitCalculator));
      }

      this.SvgUnitCalculator = svgUnitCalculator;
    }

    protected SvgUnitCalculator SvgUnitCalculator { get; }

    internal override void TranslateUntyped(object untypedInstance,
                                            Matrix matrix,
                                            int targetDpi,
                                            out Matrix newMatrix,
                                            out object translation)
    {
      if (untypedInstance == null)
      {
        LogTo.Error($"{nameof(untypedInstance)} is null");
        translation = null;
        newMatrix = matrix;
        return;
      }

      var instance = untypedInstance as T;
      if (instance == null)
      {
        LogTo.Error($"tried to translate {untypedInstance.GetType()} with {this.GetType()}");
        translation = null;
        newMatrix = matrix;
        return;
      }

      this.Translate(instance,
                     matrix,
                     targetDpi,
                     out newMatrix,
                     out translation);
    }

    public void Translate(T instance,
                          Matrix matrix,
                          int targetDpi,
                          out Matrix newMatrix,
                          out object translation)
    {
      if (instance == null)
      {
        LogTo.Error($"{nameof(instance)} is null");
        translation = null;
        newMatrix = null;
        return;
      }

      if (matrix == null)
      {
        LogTo.Error($"{nameof(matrix)} is null");
        translation = null;
        newMatrix = null;
        return;
      }

      newMatrix = this.SvgUnitCalculator.MultiplyTransformationsIntoNewMatrix(instance,
                                                                              matrix);

      translation = this.Translate(instance,
                                   newMatrix,
                                   targetDpi);
    }

    public virtual object Translate([NotNull] T instance,
                                    [NotNull] Matrix matrix,
                                    int targetDpi)
    {
      return null;
    }
  }
}