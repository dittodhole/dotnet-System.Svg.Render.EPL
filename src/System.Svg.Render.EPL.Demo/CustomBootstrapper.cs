using JetBrains.Annotations;

namespace System.Svg.Render.EPL.Demo
{
  [PublicAPI]
  public class CustomBootstrapper : DefaultBootstrapper
  {
    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected override EPL.EplTransformer CreateEplTransformer([NotNull] SvgUnitReader svgUnitReader)
    {
      return new EplTransformer(svgUnitReader);
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected override EPL.SvgImageTranslator CreateSvgImageTranslator([NotNull] EPL.EplTransformer eplTransformer,
                                                                       [NotNull] EplCommands eplCommands)
    {
      return new SvgImageTranslator(eplTransformer,
                                    eplCommands);
    }
  }
}