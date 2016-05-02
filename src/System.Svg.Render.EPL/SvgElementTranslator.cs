namespace System.Svg.Render.EPL
{
  public abstract class SvgElementTranslator<T>
    where T : SvgElement
  {
    public abstract object Translate(T instance,
                                     int targetDpi);
  }
}