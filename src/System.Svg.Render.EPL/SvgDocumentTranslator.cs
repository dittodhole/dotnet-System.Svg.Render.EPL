using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using Anotar.LibLog;

namespace System.Svg.Render.EPL
{
  public class SvgDocumentTranslator : SvgElementTranslator<SvgDocument>
  {
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitCalculator" /> is <see langword="null" />.</exception>
    public SvgDocumentTranslator(SvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator) {}

    // TODO maybe switch to HybridDictionary - in this scenario we have just a bunch of translators, ... but ... community?!
    private ConcurrentDictionary<Type, SvgElementTranslator> SvgElementTranslators { get; } = new ConcurrentDictionary<Type, SvgElementTranslator>();

    public string Translate(SvgDocument instance,
                            int targetDpi)
    {
      var translation = this.Translate(instance,
                                       new Matrix(),
                                       targetDpi);

      return translation;
    }

    /// <exception cref="ArgumentNullException"><paramref name="matrix" /> is <see langword="null" />.</exception>
    public new string Translate(SvgDocument instance,
                                Matrix matrix,
                                int targetDpi)
    {
      if (matrix == null)
      {
        throw new ArgumentNullException(nameof(matrix));
      }

      var translations = new LinkedList<string>();

      this.TranslateSvgElementAndChildren(instance,
                                          matrix,
                                          targetDpi,
                                          translations);

      var translation = string.Join(Environment.NewLine,
                                    translations);

      return translation;
    }

    private void TranslateSvgElementAndChildren(SvgElement svgElement,
                                                Matrix matrix,
                                                int targetDpi,
                                                ICollection<string> translations)
    {
      var svgVisualElement = svgElement as SvgVisualElement;
      if (svgVisualElement != null)
      {
        // TODO consider performance here w/ the cast
        if (!svgVisualElement.Visible)
        {
          return;
        }
      }

      object translation;
      Matrix newMatrix;
      this.TranslateSvgElement(svgElement,
                               matrix,
                               targetDpi,
                               out translation,
                               out newMatrix);
      if (translation != null)
      {
        translations.Add(translation.ToString());
      }

      foreach (var child in svgElement.Children)
      {
        this.TranslateSvgElementAndChildren(child,
                                            newMatrix,
                                            targetDpi,
                                            translations);
      }
    }

    protected virtual void TranslateSvgElement(SvgElement svgElement,
                                               Matrix matrix,
                                               int targetDpi,
                                               out object translation,
                                               out Matrix newMatrix)
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
                                            out translation,
                                            out newMatrix);
    }

    protected virtual SvgElementTranslator GetSvgElementTranslator(Type type)
    {
      if (type == null)
      {
        LogTo.Error($"{nameof(type)} is null");
        return null;
      }

      SvgElementTranslator svgElementTranslator;
      this.SvgElementTranslators.TryGetValue(type,
                                             out svgElementTranslator);
      return svgElementTranslator;
    }

    public void RegisterTranslator<T>(SvgElementTranslator<T> svgElementTranslator) where T : SvgElement
    {
      this.SvgElementTranslators[typeof(T)] = svgElementTranslator;
    }
  }
}