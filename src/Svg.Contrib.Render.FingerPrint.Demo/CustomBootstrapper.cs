using JetBrains.Annotations;

namespace Svg.Contrib.Render.FingerPrint.Demo
{
  [PublicAPI]
  public class CustomBootstrapper : DefaultBootstrapper
  {
    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected override FingerPrint.SvgTextBaseTranslator<SvgTextSpan> CreateSvgTextSpanTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                                                  [NotNull] FingerPrintCommands fingerPrintCommands) => new SvgTextBaseTranslator<SvgTextSpan>(fingerPrintTransformer,
                                                                                                                                                                                               fingerPrintCommands);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected override FingerPrint.SvgTextBaseTranslator<SvgText> CreateSvgTextTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                                          [NotNull] FingerPrintCommands fingerPrintCommands) => new SvgTextBaseTranslator<SvgText>(fingerPrintTransformer,
                                                                                                                                                                                   fingerPrintCommands);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected override FingerPrint.SvgImageTranslator CreateSvgImageTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                               [NotNull] FingerPrintCommands fingerPrintCommands) => new SvgImageTranslator(fingerPrintTransformer,
                                                                                                                                                            fingerPrintCommands);
  }
}