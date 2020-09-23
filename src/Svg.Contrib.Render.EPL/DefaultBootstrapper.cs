using System;
using System.Drawing.Drawing2D;
using JetBrains.Annotations;

namespace Svg.Contrib.Render.EPL
{
  [PublicAPI]
  public class DefaultBootstrapper
  {
    [NotNull]
    [Pure]
    protected virtual SvgUnitReader CreateSvgUnitReader() => new SvgUnitReader();

    [NotNull]
    [Pure]
    public virtual EplTransformer CreateEplTransformer()
    {
      var svgUnitReader = this.CreateSvgUnitReader();
      var eplTransformer = this.CreateEplTransformer(svgUnitReader);

      return eplTransformer;
    }

    /// <exception cref="ArgumentNullException"><paramref name="svgUnitReader" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual EplTransformer CreateEplTransformer([NotNull] SvgUnitReader svgUnitReader)
    {
      if (svgUnitReader == null)
      {
        throw new ArgumentNullException(nameof(svgUnitReader));
      }

      var eplTransformer = new EplTransformer(svgUnitReader);

      return eplTransformer;
    }

    /// <exception cref="ArgumentNullException"><paramref name="eplTransformer" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual Matrix CreateViewMatrix([NotNull] EplTransformer eplTransformer,
                                           float sourceDpi,
                                           float destinationDpi,
                                           ViewRotation viewRotation = ViewRotation.Normal)
    {
      if (eplTransformer == null)
      {
        throw new ArgumentNullException(nameof(eplTransformer));
      }

      var magnificationFactor = destinationDpi / sourceDpi;

      var viewMatrix = eplTransformer.CreateViewMatrix(magnificationFactor,
                                                       viewRotation);

      return viewMatrix;
    }

    /// <exception cref="ArgumentNullException"><paramref name="eplCommands" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual EplRenderer CreateEplRenderer([NotNull] EplCommands eplCommands,
                                                    PrinterCodepage printerCodepage = PrinterCodepage.Dos850,
                                                    int countryCode = 850)
    {
      if (eplCommands == null)
      {
        throw new ArgumentNullException(nameof(eplCommands));
      }

      var eplRenderer = new EplRenderer(eplCommands,
                                        printerCodepage,
                                        countryCode);

      return eplRenderer;
    }

    [NotNull]
    [Pure]
    protected virtual EplCommands CreateEplCommands() => new EplCommands();

    /// <exception cref="ArgumentNullException"><paramref name="eplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplCommands" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual SvgLineTranslator CreateSvgLineTranslator([NotNull] EplTransformer eplTransformer,
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

      var svgLineTranslator = new SvgLineTranslator(eplTransformer,
                                                    eplCommands);

      return svgLineTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="eplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplCommands" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="svgUnitReader" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual SvgRectangleTranslator CreateSvgRectangleTranslator([NotNull] EplTransformer eplTransformer,
                                                                          [NotNull] EplCommands eplCommands,
                                                                          [NotNull] SvgUnitReader svgUnitReader)
    {
      if (eplTransformer == null)
      {
        throw new ArgumentNullException(nameof(eplTransformer));
      }
      if (eplCommands == null)
      {
        throw new ArgumentNullException(nameof(eplCommands));
      }
      if (svgUnitReader == null)
      {
        throw new ArgumentNullException(nameof(svgUnitReader));
      }

      var svgRectangleTranslator = new SvgRectangleTranslator(eplTransformer,
                                                              eplCommands,
                                                              svgUnitReader);

      return svgRectangleTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="eplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplCommands" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual SvgTextBaseTranslator<SvgText> CreateSvgTextTranslator([NotNull] EplTransformer eplTransformer,
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

      var svgTextBaseTranslator = new SvgTextBaseTranslator<SvgText>(eplTransformer,
                                                                     eplCommands);

      return svgTextBaseTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="eplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplCommands" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual SvgTextBaseTranslator<SvgTextSpan> CreateSvgTextSpanTranslator([NotNull] EplTransformer eplTransformer,
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

      var svgTextSpanTranslator = new SvgTextBaseTranslator<SvgTextSpan>(eplTransformer,
                                                                         eplCommands);

      return svgTextSpanTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="eplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplCommands" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual SvgPathTranslator CreateSvgPathTranslator([NotNull] EplTransformer eplTransformer,
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

      var svgPathTranslator = new SvgPathTranslator(eplTransformer,
                                                    eplCommands);

      return svgPathTranslator;
    }

    /// <exception cref="ArgumentNullException"><paramref name="eplTransformer" /> is <see langword="null" />.</exception>
    /// <exception cref="ArgumentNullException"><paramref name="eplCommands" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    protected virtual SvgImageTranslator CreateSvgImageTranslator([NotNull] EplTransformer eplTransformer,
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

    /// <exception cref="ArgumentNullException"><paramref name="eplTransformer" /> is <see langword="null" />.</exception>
    [NotNull]
    [Pure]
    public virtual EplRenderer CreateEplRenderer([NotNull] EplTransformer eplTransformer,
                                                 PrinterCodepage printerCodepage = PrinterCodepage.Dos850,
                                                 int countryCode = 850)
    {
      if (eplTransformer == null)
      {
        throw new ArgumentNullException(nameof(eplTransformer));
      }

      var svgUnitReader = this.CreateSvgUnitReader();
      var eplCommands = this.CreateEplCommands();
      var eplRenderer = this.CreateEplRenderer(eplCommands,
                                               printerCodepage,
                                               countryCode);
      var svgLineTranslator = this.CreateSvgLineTranslator(eplTransformer,
                                                           eplCommands);
      var svgRectangleTranslator = this.CreateSvgRectangleTranslator(eplTransformer,
                                                                     eplCommands,
                                                                     svgUnitReader);
      var svgTextTranslator = this.CreateSvgTextTranslator(eplTransformer,
                                                           eplCommands);
      var svgTextSpanTranslator = this.CreateSvgTextSpanTranslator(eplTransformer,
                                                                   eplCommands);
      var svgPathTranslator = this.CreateSvgPathTranslator(eplTransformer,
                                                           eplCommands);
      var svgImageTranslator = this.CreateSvgImageTranslator(eplTransformer,
                                                             eplCommands);

      eplRenderer.RegisterTranslator(svgLineTranslator);
      eplRenderer.RegisterTranslator(svgRectangleTranslator);
      eplRenderer.RegisterTranslator(svgTextTranslator);
      eplRenderer.RegisterTranslator(svgTextSpanTranslator);
      eplRenderer.RegisterTranslator(svgPathTranslator);
      eplRenderer.RegisterTranslator(svgImageTranslator);

      return eplRenderer;
    }
  }
}
