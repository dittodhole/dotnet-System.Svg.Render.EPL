using System;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.EPL.Demo
{
  [PublicAPI]
  public class CustomBootstrapper : DefaultBootstrapper
  {
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitReader"/> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected override EPL.EplTransformer CreateEplTransformer([NotNull] SvgUnitReader svgUnitReader)
    {
      if (svgUnitReader == null)
      {
        throw new ArgumentNullException(nameof(svgUnitReader));
      }

      var eplTransformer = new EplTransformer(svgUnitReader);

      return eplTransformer;
    }

    /// <exception cref="ArgumentNullException"><paramref name="eplTransformer"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplCommands"/> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected override EPL.SvgImageTranslator CreateSvgImageTranslator([NotNull] EPL.EplTransformer eplTransformer,
                                                                       [NotNull] EplCommands eplCommands)
    {
      if (eplTransformer == null)
      {
        throw new ArgumentNullException(nameof(eplTransformer));
      }
      if (eplCommands == null)
      {
        throw new ArgumentNullException(nameof(eplCommands));
      }

      var svgImageTranslator = new SvgImageTranslator(eplTransformer,
                                                      eplCommands);

      return svgImageTranslator;
    }

    [NotNull]
    [Pure]
    protected override EPL.SvgTextBaseTranslator<SvgTextSpan> CreateSvgTextSpanTranslator(EPL.EplTransformer eplTransformer,
                                                                                          EplCommands eplCommands) => new SvgTextBaseTranslator<SvgTextSpan>(eplTransformer,
                                                                                                                                                             eplCommands);

    [NotNull]
    [Pure]
    protected override EPL.SvgTextBaseTranslator<SvgText> CreateSvgTextTranslator(EPL.EplTransformer eplTransformer,
                                                                                  EplCommands eplCommands) => new SvgTextBaseTranslator<SvgText>(eplTransformer,
                                                                                                                                                 eplCommands);
  }
}