using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Drawing.Drawing2D;
using System.Linq;
using JetBrains.Annotations;

namespace System.Svg.Render
{
  public abstract class SvgDocumentTranslatorBase : SvgElementTranslatorBase<SvgDocument>
  {
    protected SvgDocumentTranslatorBase([NotNull] ISvgUnitCalculator svgUnitCalculator)
      : base(svgUnitCalculator) {}

    [NotNull]
    private ConcurrentDictionary<Type, ISvgElementTranslator> SvgElementTranslators { get; } = new ConcurrentDictionary<Type, ISvgElementTranslator>();

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
          this.AddHiddenTranslation(svgElement,
                                    translations);
#endif
          return;
        }
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
        this.AddFailedTranslation(svgElement,
                                  translations,
                                  translation);
#endif
        return;
      }

      if (translation != null)
      {
        this.AddTranslation(svgElement,
                            translations,
                            translation);
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

    protected abstract void AddTranslation(SvgElement svgElement,
                                           ICollection<object> translations,
                                           object translation);

    protected abstract void AddFailedTranslation(SvgElement svgElement,
                                                 ICollection<object> translations,
                                                 object translation);

    protected abstract void AddHiddenTranslation(SvgElement svgElement,
                                                 ICollection<object> translations);

    private bool TryTranslateSvgElement([NotNull] SvgElement svgElement,
                                        [NotNull] Matrix matrix,
                                        int targetDpi,
                                        out Matrix newMatrix,
                                        out object translation)
    {
      var type = svgElement.GetType();

      ISvgElementTranslator svgElementTranslator;
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

    public void RegisterTranslator<T>(ISvgElementTranslator<T> svgElementTranslator) where T : SvgElement
    {
      this.SvgElementTranslators[typeof(T)] = svgElementTranslator;
    }
  }
}