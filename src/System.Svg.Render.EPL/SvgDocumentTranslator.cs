using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

namespace System.Svg.Render.EPL
{
  public class SvgDocumentTranslator : SvgElementTranslator<SvgDocument>
  {
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitCalculator" /> is <see langword="null" />.</exception>
    public SvgDocumentTranslator(SvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator) {}

    // TODO maybe switch to HybridDictionary - in this scenario we have just a bunch of translators, ... but ... community?!
    [NotNull]
    private ConcurrentDictionary<Type, SvgElementTranslator> SvgElementTranslators { get; } = new ConcurrentDictionary<Type, SvgElementTranslator>();

    public string Translate(SvgDocument instance,
                            int targetDpi)
    {
      var translation = this.Translate(instance,
                                       new Matrix(),
                                       targetDpi);

      return translation;
    }

    public string Translate([NotNull] SvgDocument instance,
                            [NotNull] Matrix matrix,
                            int targetDpi)
    {
      ICollection<object> translations = new LinkedList<object>();

      this.TranslateSvgElementAndChildren(instance,
                                          matrix,
                                          targetDpi,
                                          translations);

      if (translations.Any())
      {
        return string.Join(Environment.NewLine,
                           translations);
      }

      return null;
    }

    private void TranslateSvgElementAndChildren([NotNull] SvgElement svgElement,
                                                [NotNull] Matrix matrix,
                                                int targetDpi,
                                                [NotNull] ICollection<object> translations)
    {
      var svgVisualElement = svgElement as SvgVisualElement;
      if (svgVisualElement != null)
      {
        // TODO consider performance here w/ the cast
        if (!svgVisualElement.Visible)
        {
#if DEBUG
          translations.Add($"; <{svgElement.ID} is hidden />");
#endif
          return;
        }
      }

      if (svgElement is SvgTextSpan)
      {
        // TODO remove this in a later version, this should be internal of SvgTextTranslator
        return;
      }

      object translation;
      Matrix newMatrix;
      if (!this.TryTranslateSvgElement(svgElement,
                                       matrix,
                                       targetDpi,
                                       out newMatrix,
                                       out translation))
      {
#if DEBUG
        translations.Add($"; <{svgElement.ID}>");
        translations.Add(translation ?? "; translation failed");
        translations.Add($"; </{svgElement.ID}>");
#endif
        return;
      }

      if (translation != null)
      {
#if DEBUG
        translations.Add($"; <{svgElement.ID}>");
#endif
        translations.Add(translation);
#if DEBUG
        translations.Add($"; </{svgElement.ID}>");
#endif
      }

      foreach (var child in svgElement.Children)
      {
        if (child == null)
        {
          continue;
        }

        this.TranslateSvgElementAndChildren(child,
                                            newMatrix,
                                            targetDpi,
                                            translations);
      }
    }

    private bool TryTranslateSvgElement([NotNull] SvgElement svgElement,
                                        [NotNull] Matrix matrix,
                                        int targetDpi,
                                        out Matrix newMatrix,
                                        out object translation)
    {
      var type = svgElement.GetType();
      SvgElementTranslator svgElementTranslator;
      if (!this.SvgElementTranslators.TryGetValue(type,
                                                  out svgElementTranslator))
      {
        newMatrix = matrix;
        translation = null;
        return true;
      }

      return svgElementTranslator.TryTranslateUntyped(svgElement,
                                                      matrix,
                                                      targetDpi,
                                                      out newMatrix,
                                                      out translation);
    }

    public void RegisterTranslator<T>(SvgElementTranslator<T> svgElementTranslator) where T : SvgElement
    {
      this.SvgElementTranslators[typeof(T)] = svgElementTranslator;
    }
  }
}