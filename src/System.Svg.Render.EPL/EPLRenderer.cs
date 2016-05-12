using System.Collections.Generic;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class EPLRenderer : RendererBase
  {
    public EPLRenderer([NotNull] ISvgUnitCalculator svgUnitCalculator,
                       [NotNull] Matrix viewMatrix,
                       PrinterCodepage printerCodepage = PrinterCodepage.Dos347,
                       int countryCode = 1)
      : base(svgUnitCalculator)
    {
      this.ViewMatrix = viewMatrix;
      this.PrinterCodepage = printerCodepage;
      this.CountryCode = countryCode;
    }

    [NotNull]
    private Matrix ViewMatrix { get; }

    [NotNull]
    private PrinterCodepage PrinterCodepage { get; }

    [NotNull]
    private int CountryCode { get; }

    protected override void AddTranslation([NotNull] SvgElement svgElement,
                                           [NotNull] ICollection<object> translations,
                                           [NotNull] object translation)
    {
#if DEBUG
      translations.Add($"; <{svgElement.ID}>");
#endif
      translations.Add(translation);
#if DEBUG
      translations.Add($"; </{svgElement.ID}>");
#endif
    }

    protected override void AddHiddenTranslation([NotNull] SvgElement svgElement,
                                                 [NotNull] ICollection<object> translations)
    {
      translations.Add($"; <{svgElement.ID} is hidden />");
    }

    public override string GetTranslation([NotNull] SvgDocument instance)
    {
      var translation = this.GetTranslation(instance,
                                            this.ViewMatrix);

      translation = $@"R0,0
ZT
{translation}
P1
";

      return translation;
    }
  }
}