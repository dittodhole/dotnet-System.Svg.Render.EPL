using System.Drawing.Drawing2D;
using Anotar.LibLog;

namespace System.Svg.Render.EPL
{
  public abstract class SvgElementTranslator
  {
    internal abstract object TranslateUntyped(object untypedInstance,
                                              Matrix matrix,
                                              int targetDpi);
  }

  public abstract class SvgElementTranslator<T> : SvgElementTranslator
    where T : SvgElement
  {
    internal override object TranslateUntyped(object untypedInstance,
                                              Matrix matrix,
                                              int targetDpi)
    {
      if (untypedInstance == null)
      {
        LogTo.Error($"{nameof(untypedInstance)} is null");
        return null;
      }

      var instance = untypedInstance as T;
      if (instance == null)
      {
        LogTo.Error($"tried to translate {untypedInstance.GetType()} with {this.GetType()}");
        return null;
      }

      var translation = this.Translate(instance,
                                       matrix,
                                       targetDpi);

      return translation;
    }


    public abstract object Translate(T instance,
                                     Matrix matrix,
                                     int targetDpi);
  }
}