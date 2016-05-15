using System.Collections.Generic;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public abstract class SvgElementTranslatorBase<T> : ISvgElementTranslator<T>
    where T : SvgElement
  {
    IEnumerable<byte> ISvgElementTranslator.Translate([NotNull] SvgElement svgElement,
                                                      [NotNull] Matrix matrix)
    {
      var result = this.Translate((T) svgElement,
                                  matrix);

      return result;
    }

    public abstract IEnumerable<byte> Translate([NotNull] T svgElement,
                                                [NotNull] Matrix matrix);
  }
}