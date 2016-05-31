using JetBrains.Annotations;

namespace Svg.Contrib.Render.EPL.Demo
{
  [PublicAPI]
  public class CustomBootstrapper : DefaultBootstrapper
  {
    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected override EPL.EplTransformer CreateEplTransformer([NotNull] SvgUnitReader svgUnitReader) => new EplTransformer(svgUnitReader);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected override EPL.SvgImageTranslator CreateSvgImageTranslator([NotNull] EPL.EplTransformer eplTransformer,
                                                                       [NotNull] EplCommands eplCommands) => new SvgImageTranslator(eplTransformer,
                                                                                                                                    eplCommands);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected override EPL.SvgTextBaseTranslator<SvgTextSpan> CreateSvgTextSpanTranslator(EPL.EplTransformer eplTransformer,
                                                                                          EplCommands eplCommands) => new SvgTextBaseTranslator<SvgTextSpan>(eplTransformer,
                                                                                                                                                             eplCommands);

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected override EPL.SvgTextBaseTranslator<SvgText> CreateSvgTextTranslator(EPL.EplTransformer eplTransformer,
                                                                                  EplCommands eplCommands) => new SvgTextBaseTranslator<SvgText>(eplTransformer,
                                                                                                                                                 eplCommands);
  }
}