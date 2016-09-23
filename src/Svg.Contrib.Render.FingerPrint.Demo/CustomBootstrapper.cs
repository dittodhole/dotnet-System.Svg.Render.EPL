using System;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.FingerPrint.Demo
{
  [PublicAPI]
  public class CustomBootstrapper : DefaultBootstrapper
  {
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintTransformer"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintCommands"/> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected override FingerPrint.SvgTextBaseTranslator<SvgTextSpan> CreateSvgTextSpanTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                                                  [NotNull] FingerPrintCommands fingerPrintCommands)
    {
      if (fingerPrintTransformer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintTransformer));
      }
      if (fingerPrintCommands == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintCommands));
      }

      var svgTextSpanTranslator = new SvgTextBaseTranslator<SvgTextSpan>(fingerPrintTransformer,
                                                                         fingerPrintCommands);

      return svgTextSpanTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintTransformer"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintCommands"/> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected override FingerPrint.SvgTextBaseTranslator<SvgText> CreateSvgTextTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                                          [NotNull] FingerPrintCommands fingerPrintCommands)
    {
      if (fingerPrintTransformer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintTransformer));
      }
      if (fingerPrintCommands == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintCommands));
      }

      var svgTextBaseTranslator = new SvgTextBaseTranslator<SvgText>(fingerPrintTransformer,
                                                                     fingerPrintCommands);

      return svgTextBaseTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintTransformer"/> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="fingerPrintCommands"/> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected override FingerPrint.SvgImageTranslator CreateSvgImageTranslator([NotNull] FingerPrintTransformer fingerPrintTransformer,
                                                                               [NotNull] FingerPrintCommands fingerPrintCommands)
    {
      if (fingerPrintTransformer == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintTransformer));
      }
      if (fingerPrintCommands == null)
      {
        throw new ArgumentNullException(nameof(fingerPrintCommands));
      }

      var svgImageTranslator = new SvgImageTranslator(fingerPrintTransformer,
                                                      fingerPrintCommands);

      return svgImageTranslator;
    }
  }
}