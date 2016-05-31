using JetBrains.Annotations;

namespace Svg.Contrib.Render.FingerPrint.Demo
{
  [PublicAPI]
  public class CustomBootstrapper : DefaultBootstrapper
  {
    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected override FingerPrint.SvgTextBaseTranslator<SvgTextSpan> CreateSvgTextSpanTranslator(FingerPrintTransformer fingerPrintTransformer,
                                                                                                  FingerPrintCommands fingerPrintCommands) => new SvgTextBaseTranslator<SvgTextSpan>(fingerPrintTransformer,
                                                                                                                                                                                     fingerPrintCommands);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected override FingerPrint.SvgTextBaseTranslator<SvgText> CreateSvgTextTranslator(FingerPrintTransformer fingerPrintTransformer,
                                                                                          FingerPrintCommands fingerPrintCommands) => new SvgTextBaseTranslator<SvgText>(fingerPrintTransformer,
                                                                                                                                                                         fingerPrintCommands);
  }
}