using System.Collections.Concurrent;
using System.Collections.Generic;
using Anotar.LibLog;

namespace System.Svg.Render.EPL
{
  public class SvgDocumentTranslator : SvgElementTranslator<SvgDocument>
  {
    // TODO maybe switch to HybridDictionary - in this scenario we have just a bunch of translators, ... but ... community?!
    private ConcurrentDictionary<Type, SvgElementTranslator> SvgElementTranslators { get; } = new ConcurrentDictionary<Type, SvgElementTranslator>();

    public override object Translate(SvgDocument instance,
                                     int targetDpi)
    {
      var translations = new LinkedList<object>();

      this.TranslateSvgElementAndChildren(instance,
                                          targetDpi,
                                          translations);

      var translation = string.Join(Environment.NewLine,
                                    translations);

      return translation;
    }

    private void TranslateSvgElementAndChildren(SvgElement svgElement,
                                                int targetDpi,
                                                ICollection<object> translations)
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

      this.TranslateSvgElement(svgElement,
                               targetDpi,
                               translations);

      foreach (var child in svgElement.Children)
      {
        this.TranslateSvgElementAndChildren(child,
                                            targetDpi,
                                            translations);
      }
    }

    protected virtual void TranslateSvgElement(SvgElement svgElement,
                                               int targetDpi,
                                               ICollection<object> translations)
    {
      var type = svgElement.GetType();
      var svgElementTranslator = this.GetSvgElementTranslator(type);
      if (svgElementTranslator == null)
      {
        return;
      }

      var translation = svgElementTranslator.TranslateUntyped(svgElement,
                                                              targetDpi);
      if (translation != null)
      {
        translations.Add(translation);
      }
    }

    public SvgElementTranslator GetSvgElementTranslator<T>() where T : SvgElement
    {
      return this.GetSvgElementTranslator(typeof(T));
    }

    public SvgElementTranslator GetSvgElementTranslator(Type type)
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

    public void AddSvgElementTranslator<T>(SvgElementTranslator<T> svgElementTranslator) where T : SvgElement
    {
      this.SvgElementTranslators[typeof(T)] = svgElementTranslator;
    }

    public void RemoveSvgElementTranslator<T>() where T : SvgElement
    {
      SvgElementTranslator svgElementTranslator;
      this.SvgElementTranslators.TryRemove(typeof(T),
                                           out svgElementTranslator);
    }
  }
}