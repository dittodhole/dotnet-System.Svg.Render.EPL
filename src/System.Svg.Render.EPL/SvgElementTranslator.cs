using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public abstract class SvgElementTranslator
  {
    internal abstract void TranslateUntyped([NotNull] object untypedInstance,
                                            [NotNull] Matrix matrix,
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

    [NotNull]
    protected SvgUnitCalculator SvgUnitCalculator { get; }

    internal override void TranslateUntyped([NotNull] object untypedInstance,
                                            [NotNull] Matrix matrix,
                                            int targetDpi,
                                            out Matrix newMatrix,
                                            out object translation)
    {
      this.Translate((T) untypedInstance,
                     matrix,
                     targetDpi,
                     out newMatrix,
                     out translation);
    }

    private void Translate([NotNull] T instance,
                           [NotNull] Matrix matrix,
                           int targetDpi,
                           out Matrix newMatrix,
                           out object translation)
    {
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