using JetBrains.Annotations;

namespace Svg.Contrib.Render.ZPL.Demo
{
  [PublicAPI]
  public class CustomBootstrapper : DefaultBootstrapper
  {
    [NotNull]
    [Pure]
    [MustUseReturnValue]
    protected override ZPL.SvgImageTranslator CreateSvgImageTranslator([NotNull] ZplTransformer zplTransformer,
                                                                       [NotNull] ZplCommands zplCommands) => new SvgImageTranslator(zplTransformer,
                                                                                                                                    zplCommands);
  }
}