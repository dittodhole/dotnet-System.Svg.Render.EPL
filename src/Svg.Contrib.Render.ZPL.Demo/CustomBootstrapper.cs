using JetBrains.Annotations;

namespace Svg.Contrib.Render.ZPL.Demo
{
  [PublicAPI]
  public class CustomBootstrapper : DefaultBootstrapper
  {
    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected override ZPL.ZplTransformer CreateZplTransformer([NotNull] SvgUnitReader svgUnitReader)
    {
      return new ZplTransformer(svgUnitReader);
    }

    //[NotNull]
    //[Pure]
    //[MustUseReturnValue]
    //protected override System.Svg.Render.ZPL.SvgImageTranslator CreateSvgImageTranslator([NotNull] System.Svg.Render.ZPL.ZplTransformer zplTransformer,
    //                                                                                     [NotNull] ZplCommands zplCommands)
    //{
    //  return new SvgImageTranslator(zplTransformer,
    //                                zplCommands);
    //}
  }
}