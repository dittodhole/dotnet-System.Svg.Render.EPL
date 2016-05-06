using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using Anotar.LibLog;
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

    public new string Translate([NotNull] SvgDocument instance,
                                [NotNull] Matrix matrix,
                                int targetDpi)
    {
      var translations = new LinkedList<string>();

      this.TranslateSvgElementAndChildren(instance,
                                          matrix,
                                          targetDpi,
                                          translations);

      var translation = string.Join(Environment.NewLine,
                                    translations);

      return translation;
    }

    private void TranslateSvgElementAndChildren([NotNull] SvgElement svgElement,
                                                [NotNull] Matrix matrix,
                                                int targetDpi,
                                                [NotNull] ICollection<string> translations)
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
      this.TranslateSvgElement(svgElement,
                               matrix,
                               targetDpi,
                               out newMatrix,
                               out translation);
      if (translation != null)
      {
#if DEBUG
        translations.Add($"; <{svgElement.ID}>");
#endif
        translations.Add(translation.ToString());
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

    private void TranslateSvgElement([NotNull] SvgElement svgElement,
                                     [NotNull] Matrix matrix,
                                     int targetDpi,
                                     out Matrix newMatrix,
                                     out object translation)
    {
      var type = svgElement.GetType();
      var svgElementTranslator = this.GetSvgElementTranslator(type);
      if (svgElementTranslator == null)
      {
        translation = null;
        newMatrix = matrix;
        return;
      }

      svgElementTranslator.TranslateUntyped(svgElement,
                                            matrix,
                                            targetDpi,
                                            out newMatrix,
                                            out translation);
    }

    private SvgElementTranslator GetSvgElementTranslator([NotNull] Type type)
    {
      SvgElementTranslator svgElementTranslator;
      if (this.SvgElementTranslators.TryGetValue(type,
                                                 out svgElementTranslator))
      {
        return svgElementTranslator;
      }

      return null;
    }

    public void RegisterTranslator<T>(SvgElementTranslator<T> svgElementTranslator) where T : SvgElement
    {
      this.SvgElementTranslators[typeof(T)] = svgElementTranslator;
    }
  }
}