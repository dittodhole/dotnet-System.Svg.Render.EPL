using System;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.ZPL.Demo
{
  [PublicAPI]
  public class CustomBootstrapper : DefaultBootstrapper
  {
    /// <exception cref="ArgumentNullException"><paramref name="zplTransformer"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="zplCommands"/> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected override ZPL.SvgImageTranslator CreateSvgImageTranslator([NotNull] ZplTransformer zplTransformer,
                                                                       [NotNull] ZplCommands zplCommands)
    {
      if (zplTransformer == null)
      {
        throw new ArgumentNullException(nameof(zplTransformer));
      }
      if (zplCommands == null)
      {
        throw new ArgumentNullException(nameof(zplCommands));
      }

      var svgImageTranslator = new SvgImageTranslator(zplTransformer,
                                                      zplCommands);

      return svgImageTranslator;
    }
  }
}