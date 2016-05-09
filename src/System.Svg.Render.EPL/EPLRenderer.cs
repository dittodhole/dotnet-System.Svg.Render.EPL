using System.Collections.Generic;
using System.Drawing;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class EPLRenderer : RendererBase
  {
    public EPLRenderer([NotNull] ISvgUnitCalculator svgUnitCalculator,
                       Point origin)
      : base(svgUnitCalculator,
             origin) {}

    protected override void AddTranslation(SvgElement svgElement,
                                           ICollection<object> translations,
                                           object translation)
    {
#if DEBUG
      translations.Add($"; <{svgElement.ID}>");
#endif
      translations.Add(translation);
#if DEBUG
      translations.Add($"; </{svgElement.ID}>");
#endif
    }

    protected override void AddFailedTranslation(SvgElement svgElement,
                                                 ICollection<object> translations,
                                                 object translation)
    {
      translations.Add($"; <{svgElement.ID}>");
      translations.Add(translation ?? "; translation failed");
      translations.Add($"; </{svgElement.ID}>");
    }

    protected override void AddHiddenTranslation(SvgElement svgElement,
                                                 ICollection<object> translations)
    {
      translations.Add($"; <{svgElement.ID} is hidden />");
    }
  }
}