﻿using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public abstract class SvgElementTranslator
  {
    public abstract bool TryTranslateUntyped([NotNull] object untypedInstance,
                                             [NotNull] Matrix matrix,
                                             int targetDpi,
                                             out Matrix newMatrix,
                                             out object translation);
  }

  public abstract class SvgElementTranslator<T> : SvgElementTranslator
    where T : SvgElement
  {
    protected SvgElementTranslator([NotNull] SvgUnitCalculator svgUnitCalculator)
    {
      this.SvgUnitCalculator = svgUnitCalculator;
    }

    [NotNull]
    protected SvgUnitCalculator SvgUnitCalculator { get; }

    public override bool TryTranslateUntyped([NotNull] object untypedInstance,
                                             [NotNull] Matrix matrix,
                                             int targetDpi,
                                             out Matrix newMatrix,
                                             out object translation)
    {
      var success = this.Translate((T) untypedInstance,
                                   matrix,
                                   targetDpi,
                                   out newMatrix,
                                   out translation);

      return success;
    }

    private bool Translate([NotNull] T instance,
                           [NotNull] Matrix matrix,
                           int targetDpi,
                           out Matrix newMatrix,
                           out object translation)
    {
      newMatrix = this.SvgUnitCalculator.MultiplyTransformationsIntoNewMatrix(instance,
                                                                              matrix);

      var success = this.TryTranslate(instance,
                                      newMatrix,
                                      targetDpi,
                                      out translation);

      return success;
    }

    public virtual bool TryTranslate([NotNull] T instance,
                                     [NotNull] Matrix matrix,
                                     int targetDpi,
                                     out object translation)
    {
      translation = null;
      return true;
    }
  }
}