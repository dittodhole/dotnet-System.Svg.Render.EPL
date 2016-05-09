using System.Collections.Generic;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class EPLRenderer : RendererBase
  {
    public EPLRenderer([NotNull] ISvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator) {}

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

    public override string GetTranslation(SvgDocument instance,
                                          int targetDpi)
    {
      var viewMatrix = new Matrix(0f,
                                  1f,
                                  1f,
                                  0f,
                                  0f,
                                  0f);

      return this.GetTranslation(instance,
                                 viewMatrix,
                                 targetDpi);
    }
  }
}