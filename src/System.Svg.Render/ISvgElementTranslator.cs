using System.Collections.Generic;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public interface ISvgElementTranslator
  {
    IEnumerable<byte> TranslateUntyped([NotNull] SvgElement svgElement,
                                       [NotNull] Matrix matrix);
  }

  public interface ISvgElementTranslator<T> : ISvgElementTranslator
    where T : SvgElement
  {
    IEnumerable<byte> Translate([NotNull] T svgElement,
                                [NotNull] Matrix matrix);
  }
}