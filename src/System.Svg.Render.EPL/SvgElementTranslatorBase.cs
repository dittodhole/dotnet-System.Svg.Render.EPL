namespace System.Svg.Render.EPL
{
  public abstract class SvgElementTranslatorBase<TSvgElement> : SvgElementTranslatorBase<EplStream, TSvgElement>
    where TSvgElement : SvgElement {}
}