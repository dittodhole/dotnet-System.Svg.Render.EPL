using JetBrains.Annotations;

namespace System.Svg.Render.EPL.Demo
{
  [PublicAPI]
  public class CustomBootstrapper : DefaultBootstrapper
  {
    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected override System.Svg.Render.EPL.EplTransformer CreateEplTransformer([NotNull] SvgUnitReader svgUnitReader)
    {
      return new EplTransformer(svgUnitReader);
    }

    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected override System.Svg.Render.EPL.SvgImageTranslator CreateSvgImageTranslator([NotNull] System.Svg.Render.EPL.EplTransformer eplTransformer,
                                                                                         [NotNull] EplCommands eplCommands,
                                                                                         bool assumeStoredInInternalMemory)
    {
      return new SvgImageTranslator(eplTransformer,
                                    eplCommands)
             {
               AssumeStoredInInternalMemory = assumeStoredInInternalMemory
             };
    }
  }
}